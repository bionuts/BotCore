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
using System.Timers;
using System.Windows.Forms;

namespace BCore
{
    public partial class MainBotForm : Form
    {
        private readonly ApplicationDbContext db;
        private List<BOrder> LoadedOrders;
        private string ApiToken;
        private DateTime _StartTime;
        private DateTime _EndTime;
        private DateTime ServerTime;
        private int intervalDuration;
        private bool can;

        private ThreadParamObject[] arr_params;
        private HttpClient sendhttp;
        private HttpClient GenHttp;
        static readonly object locker = new object();
        private string resultOfThreads = "";
        private readonly JsonSerializerOptions serializeOptions;
        private int StepWait;
        private static bool[] ceaseFire;

        public MainBotForm()
        {
            db = new ApplicationDbContext();
            serializeOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };
            GenHttp = new HttpClient();
            InitializeComponent();
        }

        private async void MainBotForm_Load(object sender, EventArgs e)
        {
            await LoadOrdersToListView();
            can = await Utilities.CanRunTheApp(GenHttp);
            if (can)
            {
                lbl_done.Text = "connected";
                lbl_done.BackColor = Color.Green;
            }
            else
            {
                lbl_done.Text = "disconnected";
                lbl_done.BackColor = Color.Red;
            }
        }

        private async void btn_load_Click(object sender, EventArgs e)
        {
            if (can)
            {
                LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today).OrderBy(d => d.CreatedDateTime).ToListAsync();
                ApiToken = (await db.BSettings.Where(c => c.Key == "apitoken").FirstOrDefaultAsync()).Value;
                sendhttp = MobinBroker.SetHttpClientForSendingOrdersStatic(ApiToken);
                if (LoadedOrders.Any())
                {
                    await LoadOrdersToListView();
                    LoadStartAndEndTime(tb_hh.Text.Trim(), tb_mm.Text.Trim(), tb_ss.Text.Trim(), tb_ms.Text.Trim(), tb_duration.Text.Trim());
                    lbl_endTime.Text = $"End: {_EndTime:HH:mm:ss.fff}";
                    intervalDuration = int.Parse(tb_interval.Text.Trim());
                    InitHttpRequestMessageAndThreadArray(LoadedOrders, intervalDuration, _StartTime, _EndTime);
                }
            }
        }

        private void SendOrderRequests()
        {
            try
            {
                int size = arr_params.Length;
                Thread.Sleep((int)_StartTime.Subtract(DateTime.Now).TotalMilliseconds);
                for (int i = 0; i < size; i++)
                {
                    if (ceaseFire[arr_params[i].WhichOne])
                        continue;
                    else
                    {
                        Task.Factory.StartNew(() => SendReq(arr_params[i])); // study here for beter lunch of thread from pool
                        Thread.Sleep(StepWait);
                    }
                }
                tb_logs.Invoke((MethodInvoker)delegate { tb_logs.Text = SortResult(resultOfThreads); });
            }
            catch (Exception ex)
            {
                tb_logs.Invoke((MethodInvoker)delegate { tb_logs.Text = ex.Message; });
            }
        }

        public void SendReq(ThreadParamObject paramObject)
        {
            string result;
            DateTime sent;
            Stopwatch _stopwatch = new Stopwatch();
            try
            {
                sent = DateTime.Now;
                _stopwatch.Start();
                HttpResponseMessage httpResponse = sendhttp.SendAsync(paramObject.REQ).Result;
                _stopwatch.Stop();
                if (httpResponse.IsSuccessStatusCode)
                {
                    string content = httpResponse.Content.ReadAsStringAsync().Result;
                    OrderRespond orderRespond = JsonSerializer.Deserialize<OrderRespond>(content, serializeOptions);
                    if (orderRespond.IsSuccessfull)
                        ceaseFire[paramObject.WhichOne] = true;
                    result = $"[{sent:HH:mm:ss.fff}] [{_stopwatch.ElapsedMilliseconds:D3}ms] [ID:{paramObject.ID}] => {paramObject.SYM:-10} " +
                        $",ThreadID: {Thread.CurrentThread.ManagedThreadId:D3} [{orderRespond.IsSuccessfull}] Desc: {orderRespond.MessageDesc}\n";
                }
                else
                {
                    result = $"T_{Thread.CurrentThread.ManagedThreadId}, Sym: {paramObject.SYM},Sent: {sent:HH:mm:ss.fff}, Error: {httpResponse.StatusCode}\n";
                }
            }
            catch (Exception ex)
            {
                result = $"T_{Thread.CurrentThread.ManagedThreadId}, Sym: {paramObject.SYM},Sent: {DateTime.Now:HH:mm:ss.fff}, Error: {ex.Message}\n";
            }
            lock (locker)
            {
                resultOfThreads += result;
            }
        }

        private void InitHttpRequestMessageAndThreadArray(List<BOrder> orders, int mainInterval, DateTime start, DateTime end)
        {
            StepWait = (mainInterval + orders.Count - 1) / orders.Count;
            int reqSize = (int)((end.Subtract(start).TotalMilliseconds + StepWait - 1) / StepWait);
            arr_params = new ThreadParamObject[reqSize];
            ceaseFire = new bool[orders.Count];
            int whichOne = 0;
            for (int i = 0; i < reqSize; i++)
            {
                if (whichOne == orders.Count) whichOne = 0;
                arr_params[i] = new ThreadParamObject
                {
                    ID = orders[whichOne].Id,
                    SYM = orders[whichOne].SymboleName,
                    REQ = MobinBroker.GetSendingOrderRequestMessageStatic(orders[whichOne]),
                    WhichOne = whichOne
                };
                whichOne++;
            }
        }

        private async void btn_start_Click(object sender, EventArgs e)
        {
            if (can)
            {
                ((Button)sender).Enabled = false;
                ((Button)sender).Text = "Running...";
                await Task.Factory.StartNew(() => SendOrderRequests());
                ((Button)sender).Text = "Start";
                ((Button)sender).Enabled = true;
            }
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
            if (can)
            {
                Account accountForm = new Account();
                accountForm.ShowDialog();
            }
        }

        private void LoginMenuItemClick(object sender, EventArgs e)
        {
            if (can)
            {
                Login loginForm = new Login();
                loginForm.ShowDialog();
            }
        }

        private void SymboleMenuItemClick(object sender, EventArgs e)
        {
            if (can)
            {
                SymboleForm symForm = new SymboleForm();
                symForm.ShowDialog();
            }
        }

        private void OrderMenuItemClick(object sender, EventArgs e)
        {
            if (can)
            {
                OrderForm orderForm = new OrderForm();
                orderForm.ShowDialog();
            }
        }

        private void TestSendOrderOpenOrderMenuItemClick(object sender, EventArgs e)
        {
            if (can)
            {
                SendOrderForm sendOrderForm = new SendOrderForm();
                sendOrderForm.ShowDialog();
            }
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

        private string SortResult(string result)
        {
            string tmp = "";
            List<string> lineList = new List<string>();
            var lines = result.Split("\n");
            foreach (var line in lines)
                lineList.Add(line);
            lineList = lineList.OrderBy(p => p.Substring(1, p.IndexOf("]"))).ToList();
            foreach (var line in lineList)
                tmp += line + Environment.NewLine;
            return tmp;
        }

        private void timer_cando_Tick(object sender, EventArgs e)
        {
            bool tmp = Utilities.CanRunTheApp2(GenHttp);
            lock (locker)
            {
                can = tmp;
            }
            if (can)
            {
                lbl_done.Text = "connected";
                lbl_done.BackColor = Color.Green;
            }
            else
            {
                lbl_done.Text = "disconnected";
                lbl_done.BackColor = Color.Red;
            }
        }
    }
}
