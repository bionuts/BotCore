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
        private List<Task> OrdersTasksList;
        private DateTime _StartTime;
        private DateTime _EndTime;

        public MainBotForm()
        {
            db = new ApplicationDbContext();
            InitializeComponent();
        }

        private async void MainBotForm_Load(object sender, EventArgs e)
        {
            // LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today).ToListAsync();
            var junk = await db.BSettings.ToListAsync();
            await LoadOrdersToListView();
            lbl_path.Text = System.IO.Path.Combine(Environment.CurrentDirectory, "db.sqlite3");
        }

        private async void btn_load_Click(object sender, EventArgs e)
        {
            LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today).ToListAsync();
            ApiToken = (await db.BSettings.Where(c => c.Key == "apitoken").FirstOrDefaultAsync()).Value;
            if (LoadedOrders.Any())
            {
                await LoadOrdersToListView();
                /*LoadStartAndEndTime(tb_hh.Text.Trim(), tb_mm.Text.Trim(), tb_ss.Text.Trim(), tb_ms.Text.Trim(), tb_duration.Text.Trim());
                lbl_endTime.Text = $"End: {_EndTime:HH:mm:ss.fff}";
                OrdersTasksList = new List<Task>();
                foreach (var order in LoadedOrders)
                {
                    OrdersTasksList.Add(new Task(() => SendOrderTask(
                        Utilities.GetPresetHttpClientForSendOrders(),
                        order,
                        _EndTime,
                        int.Parse(tb_interval.Text.Trim()),
                        ApiToken), TaskCreationOptions.LongRunning));
                }*/
            }
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            StartService();
        }

        private void StartService()
        {
            Task.Factory.StartNew(() =>
            {
                while (TimeSpan.Compare(DateTime.Now.TimeOfDay, _StartTime.TimeOfDay) < 0) ; // -1  if  t1 is shorter than t2.                
                foreach (var orderTask in OrdersTasksList)
                    orderTask.Start();
                AppendTextBox($"[ Started: {DateTime.Now:HH:mm:ss.fff} ]");
            }, TaskCreationOptions.LongRunning);
        }

        private void SendOrderTask(HttpClient _http, BOrder order, DateTime endTime, int interval, string ApiToken)
        {
            /*DateTime LastTrySendTime = DateTime.Now.AddMilliseconds(-interval);
            string output = "";
            int sentCount = 0;
            do
            {
                if (DateTime.Now.Subtract(LastTrySendTime).TotalMilliseconds >= interval)
                {
                    _http.SendAsync(InitOrderReqHeader(order, ApiToken));
                    output += $"\nCode:{order.SymboleCode}, Sent=> {DateTime.Now:HH:mm:ss.fff}";
                    LastTrySendTime = DateTime.Now;
                    sentCount++;
                }
            } while (TimeSpan.Compare(DateTime.Now.TimeOfDay, endTime.TimeOfDay) < 0);
            AppendTextBox(output);*/
        }

        public async Task LoadOrdersToListView()
        {
            lv_orders.Items.Clear();
            LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today).ToListAsync();
            foreach (var order in LoadedOrders)
            {
                var row = new string[]
                {
                    order.Id.ToString(),
                    order.SymboleCode,
                    order.SymboleName,
                    order.Count.ToString("N0"),
                    order.Price.ToString("N0"),
                    (order.Count * order.Price).ToString("N0"),
                    order.OrderType,
                    order.Status
                };
                var lv_item = new ListViewItem(row);
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
    }
}
