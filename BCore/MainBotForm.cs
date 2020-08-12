using BCore.Data;
using BCore.DataModel;
using BCore.Forms;
using BCore.Lib;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
            InitializeComponent();
            db = new ApplicationDbContext();
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
                LoadStartAndEndTime(tb_hh.Text.Trim(), tb_mm.Text.Trim(), tb_ss.Text.Trim(), tb_ms.Text.Trim(), tb_duration.Text.Trim());
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
                }
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
            DateTime LastTrySendTime = DateTime.Now.AddMilliseconds(-interval);
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
            AppendTextBox(output);
        }

        /*private async Task<bool> SendOrderItem(BOrder order, string apiToken)
        {
            DateTime sent = DateTime.Now;
            Stopwatch stopwatch = Stopwatch.StartNew();

            HttpRequestMessage req = InitOrderReqHeader(order, apiToken);
            var httpClient = httpClientFactory.CreateClient();
            HttpResponseMessage httpResponse = await httpClient.SendAsync(req);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                stopwatch.Stop();
                // Console.WriteLine($"Sym: {order.SymboleCode}, Sent: {sent:HH:mm:ss.fff}, Elapsed: { stopwatch.ElapsedMilliseconds }");
                var t = httpResponse.Content.ReadAsStringAsync().Result;
                OrderRespond orderRespond = JsonConvert.DeserializeObject<OrderRespond>(t);
                Console.WriteLine("msg", $"Sym: {order.SymboleCode}, Sent: {sent:HH:mm:ss.fff}, Elapsed: { stopwatch.ElapsedMilliseconds }, Desc: {orderRespond.MessageDesc}");
                if (orderRespond.IsSuccessfull)
                {
                    return true;
                }
            }
            return false;
        }*/

        private static HttpRequestMessage InitOrderReqHeader(BOrder order, string ApiToken)
        {
            // https://api2.mobinsb.com/Web/V1/Order/Post
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "/Web/V1/Order/Post");
            req.Headers.Add("Authorization", $"BasicAuthentication {ApiToken}");

            var payload = new OrderPayload
            {
                IsSymbolCautionAgreement = false,
                CautionAgreementSelected = false,
                IsSymbolSepahAgreement = false,
                SepahAgreementSelected = false,
                orderCount = order.Count,
                orderPrice = order.Price,
                FinancialProviderId = 1,
                minimumQuantity = 0,
                maxShow = 0,
                orderId = 0,
                isin = order.SymboleCode,
                orderSide = (order.OrderType == "SELL" ? "86" : "65"), // SELL(86) , BUY(65)
                orderValidity = 74,
                orderValiditydate = null,
                shortSellIsEnabled = false,
                shortSellIncentivePercent = 0
            };

            string str_payload = JsonConvert.SerializeObject(payload);
            req.Content = new StringContent(str_payload, System.Text.Encoding.UTF8, "application/json");
            return req;
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

        private static HttpClient GetPresetHttpClientForSendOrders()
        {
            HttpClient HttpClientForSendOrder = new HttpClient
            {
                BaseAddress = new Uri("https://api2.mobinsb.com"),
            };
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Accept", "*/*");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Connection", "keep-alive");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Host", "api2.mobinsb.com");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Origin", "https://silver.mobinsb.com");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Pragma", "no-cache");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Referer", "https://silver.mobinsb.com/Home/Default/page-1");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            HttpClientForSendOrder.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            return HttpClientForSendOrder;
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
