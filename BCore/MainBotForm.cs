using BCore.Data;
using BCore.DataModel;
using BCore.Forms;
using BCore.Lib;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCore
{
    public partial class MainBotForm : Form
    {
        private readonly ApplicationDbContext db;
        private List<BOrder> LoadedOrders;
        private string ApiToken;
        private Task[] orderTasks;
        private Thread[] orderThread;
        private DateTime _StartTime;
        private DateTime _EndTime;
        static volatile object locker = new Object();

        public MainBotForm()
        {
            db = new ApplicationDbContext();
            InitializeComponent();
        }

        private async void MainBotForm_Load(object sender, EventArgs e)
        {
            await LoadOrdersToListView();
        }

        private async void btn_load_Click(object sender, EventArgs e)
        {
            LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today).OrderBy(d => d.CreatedDateTime).ToListAsync();
            ApiToken = (await db.BSettings.Where(c => c.Key == "apitoken").FirstOrDefaultAsync()).Value;
            if (LoadedOrders.Any())
            {
                await LoadOrdersToListView();
                LoadStartAndEndTime(tb_hh.Text.Trim(), tb_mm.Text.Trim(), tb_ss.Text.Trim(), tb_ms.Text.Trim(), tb_duration.Text.Trim());
                lbl_endTime.Text = $"End: {_EndTime:HH:mm:ss.fff}";

                // orderTasks = new Task[LoadedOrders.Count];
                orderThread = new Thread[LoadedOrders.Count];
                int step = int.Parse(tb_interval.Text.Trim()) / LoadedOrders.Count;
                int dot = 0;
                int idx = 0;
                foreach (var order in LoadedOrders)
                {
                    DateTime stime = _StartTime.AddMilliseconds(dot);
                    DateTime etime = _EndTime.AddMilliseconds(dot);
                    int intval = int.Parse(tb_interval.Text.Trim());
                    // orderTasks[idx] = new Task(() => SendOrderTask(new MobinBroker(ApiToken, order), stime, etime, intval));
                    orderThread[idx] = new Thread(() => SendOrderTask(new MobinBroker(ApiToken, order), stime, etime, intval));
                    idx++;
                    dot += step;
                }
            }
        }

        private async void btn_start_Click(object sender, EventArgs e)
        {
            ((Button)sender).Text = "Running...";
            ((Button)sender).Enabled = false;
            /*for (int i = 0; i < orderTasks.Length; i++)
                orderTasks[i].Start();*/
            for (int i = 0; i < orderThread.Length; i++)
                orderThread[i].Start();
            await EnableBtn();
        }

        private void SendOrderTask(MobinBroker mobin, DateTime startTime, DateTime endTime, int interval)
        {
            ReturnedResultObject obj = new ReturnedResultObject
            {
                ResStr = "",
                CeaseFire = false
            };
            string sym = mobin.Order.SymboleCode;
            int size = (int)Math.Ceiling(endTime.Subtract(startTime).TotalMilliseconds / interval);
            Thread.Sleep((int)startTime.Subtract(DateTime.Now).TotalMilliseconds);
            for (int i = 0; (i < size) && (!obj.CeaseFire); i++)
            {
                Task.Run(() => mobin.SendOrder(i, obj));
                Thread.Sleep(interval);
            }
            lock (locker)
            {
                tb_logs.Invoke((MethodInvoker)delegate { tb_logs.Text += obj.ResStr.Replace("\n", Environment.NewLine); });
            }
        }

        public async Task EnableBtn()
        {
            int ts = 0;
            int size = orderThread.Length;
            bool done = true;
            while (done)
            {
                for (int i = 0; i < orderThread.Length; i++)
                {
                    if (orderThread[i].ThreadState == System.Threading.ThreadState.Stopped)
                        ts++;
                }
                if (ts == size)
                    done = false;
                else
                {
                    ts = 0;
                    await Task.Delay(500);
                }
            }

            btn_start.Invoke((MethodInvoker)delegate { btn_start.Enabled = true; btn_start.Text = "Start"; });
        }

        public async Task LoadOrdersToListView()
        {
            lv_orders.Items.Clear();
            LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today).OrderBy(d => d.CreatedDateTime).ToListAsync();
            foreach (var order in LoadedOrders)
            {
                decimal templong = order.Count * order.Price;
                var row = new string[]
                {
                    order.Id.ToString(),
                    order.SymboleName,
                    order.Count.ToString("N0"),
                    order.Price.ToString("N0"),
                    templong.ToString("N0"),
                    order.OrderType,
                    "0",
                    "0"
                };
                var lv_item = new ListViewItem(row)
                {
                    UseItemStyleForSubItems = false
                };
                lv_item.SubItems[5].BackColor = order.OrderType == "BUY" ? Color.LightGreen : Color.Red;
                lv_orders.Items.Add(lv_item);
            }
        }

        private void LoadStartAndEndTime(string h, string m, string s, string ms, string duration)
        {
            DateTime tempNow = DateTime.Now;
            this._StartTime = new DateTime(tempNow.Year, tempNow.Month, tempNow.Day,
                (h != "" ? int.Parse(h) : 8),
                (m != "" ? int.Parse(m) : 29),
                (s != "" ? int.Parse(s) : 58),
                (ms != "" ? int.Parse(ms) : 800));
            this._EndTime = _StartTime.AddSeconds((duration != "" ? double.Parse(duration) : 2.2));
        }

        private void lbl_starttime_Click(object sender, EventArgs e)
        {
            tb_hh.Text = DateTime.Now.Hour.ToString();
            tb_mm.Text = DateTime.Now.Minute.ToString();
            tb_ss.Text = DateTime.Now.Second.ToString();
            tb_ms.Text = DateTime.Now.Millisecond.ToString();
        }

        public void AppendTextBox(string value)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(AppendTextBox), new object[] { value });
                return;
            }
            tb_logs.Text += value.Replace("\n", Environment.NewLine);
        }

        private void AccountMenuItemClick(object sender, EventArgs e)
        {
            Account accountForm = new Account();
            accountForm.ShowDialog();
        }

        private void LoginMenuItemClick(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.ShowDialog();
        }

        private void SymboleMenuItemClick(object sender, EventArgs e)
        {
            SymboleForm symForm = new SymboleForm();
            symForm.ShowDialog();
        }

        private void OrderMenuItemClick(object sender, EventArgs e)
        {
            OrderForm orderForm = new OrderForm();
            orderForm.ShowDialog();
        }

        private void TestSendOrderOpenOrderMenuItemClick(object sender, EventArgs e)
        {
            SendOrderForm sendOrderForm = new SendOrderForm();
            // SendOrderForm sendOrderForm = BotServiceProvider.GetRequiredService<SendOrderForm>();
            sendOrderForm.ShowDialog();
        }

        private async void btn_delete_orders_Click(object sender, EventArgs e)
        {
            if (lv_orders.SelectedItems.Count > 0)
            {
                foreach (ListViewItem sitem in lv_orders.SelectedItems)
                {
                    var id = int.Parse(sitem.SubItems[0].Text);
                    var item = await db.BOrders.FindAsync(id);
                    if (item != null)
                    {
                        db.BOrders.Remove(item);
                    }
                }
                await db.SaveChangesAsync();
                await LoadOrdersToListView();
            }
        }

        private void btn_sort_result_Click(object sender, EventArgs e)
        {
            List<string> myList = new List<string>();
            var arr = tb_logs.Text.Trim().Split(Environment.NewLine);
            foreach (var s in arr)
                myList.Add(s);
            myList = myList.OrderBy(p => p.Substring(1, p.IndexOf("]"))).ToList();
            tb_logs.Text = "";
            foreach (var l in myList)
                tb_logs.Text += l + Environment.NewLine;
        }
    }
}
