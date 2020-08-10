using BCore.Data;
using BCore.DataModel;
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
        private DateTime MainStartTime;


        public MainBotForm()
        {
            InitializeComponent();
            db = new ApplicationDbContext();
        }

        private async void MainBotForm_Load(object sender, EventArgs e)
        {
            LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today).ToListAsync();
        }

        private async void btn_load_Click(object sender, EventArgs e)
        {
            lv_orders.Items.Clear();
            LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today).ToListAsync();
            ApiToken = (await db.BCookies.Where(c => c.Name == "apitoken").FirstOrDefaultAsync()).Value;
            if (LoadedOrders.Any())
            {
                LoadOrdersToListView();
                LoadStartTime(tb_hh.Text.Trim(), tb_mm.Text.Trim(), tb_ss.Text.Trim(), tb_ms.Text.Trim());
                lbl_startTime.Text = $"{MainStartTime:HH:mm:ss.fff}";
                OrdersTasksList = new List<Task>();
                foreach (var order in LoadedOrders)
                {
                    OrdersTasksList.Add(new Task(() => SendOrderTask(order, double.Parse(tb_duration.Text.Trim()), int.Parse(tb_interval.Text.Trim()), ApiToken), TaskCreationOptions.LongRunning));
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
                while (TimeSpan.Compare(DateTime.Now.TimeOfDay, MainStartTime.TimeOfDay) < 0) // -1  if  t1 is shorter than t2.
                {
                    Console.WriteLine($"Time: {DateTime.Now:HH:mm:ss.fff}");
                }
                foreach (var task in OrdersTasksList)
                    task.Start();
            }, TaskCreationOptions.LongRunning);
        }

        private void SendOrderTask(BOrder order, double duration, int interval, string ApiToken)
        {
            DateTime sendNow;
            DateTime LastTrySendTime;
            DateTime START_POINT = DateTime.Now;
            LastTrySendTime = START_POINT.AddMilliseconds(-interval);
            HttpClient httpClient = httpClientFactory.CreateClient();
            HttpRequestMessage req;
            int sentCount = 0;
            do
            {
                sendNow = DateTime.Now;
                if (sendNow.Subtract(LastTrySendTime).TotalMilliseconds >= interval)
                {
                    // SendOrderItem(order, ApiToken);

                    req = InitOrderReqHeader(order, ApiToken);
                    HttpResponseMessage httpResponse = httpClient.SendAsync(req);
                    LastTrySendTime = DateTime.Now;
                    sentCount++;
                }
            } while (sendNow.Subtract(START_POINT).TotalSeconds <= duration);
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
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "https://api2.mobinsb.com/Web/V1/Order/Post");
            req.Headers.Add("Accept", "*/*");
            req.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            req.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            req.Headers.Add("Authorization", $"BasicAuthentication {ApiToken}");
            req.Headers.Add("Cache-Control", "no-cache");
            req.Headers.Add("Connection", "keep-alive");
            req.Headers.Add("Host", "api2.mobinsb.com");
            req.Headers.Add("Origin", "https://silver.mobinsb.com");
            req.Headers.Add("Pragma", "no-cache");
            req.Headers.Add("Referer", "https://silver.mobinsb.com/Home/Default/page-1");
            req.Headers.Add("Sec-Fetch-Dest", "empty");
            req.Headers.Add("Sec-Fetch-Mode", "cors");
            req.Headers.Add("Sec-Fetch-Site", "same-site");
            req.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            req.Headers.Add("X-Requested-With", "XMLHttpRequest");

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
                orderSide = (order.TypeOrder == "SELL" ? "86" : "65"), // SELL(86) , BUY(65)
                orderValidity = 74,
                orderValiditydate = null,
                shortSellIsEnabled = false,
                shortSellIncentivePercent = 0
            };

            string str_payload = JsonConvert.SerializeObject(payload);
            req.Content = new StringContent(str_payload, System.Text.Encoding.UTF8, "application/json");
            return req;
        }

        private void LoadOrdersToListView()
        {
            foreach (var order in LoadedOrders)
            {
                var row = new string[]
                {
                            order.SymboleCode,
                            order.SymboleName,
                            order.Count.ToString("N0"),
                            order.Price.ToString("N0"),
                            (order.Count * order.Price).ToString("N0"),
                            order.TypeOrder,
                            order.Stat
                };
                var lv_item = new ListViewItem(row);
                lv_orders.Items.Add(lv_item);
            }
        }

        private void LoadStartTime(string h, string m, string s, string ms)
        {
            DateTime tempNow = DateTime.Now;
            this.MainStartTime = new DateTime(tempNow.Year, tempNow.Month, tempNow.Day,
                (h != "" ? int.Parse(h) : 8),
                (m != "" ? int.Parse(m) : 29),
                (s != "" ? int.Parse(s) : 58),
                (ms != "" ? int.Parse(ms) : 800));
        }
    }
}
