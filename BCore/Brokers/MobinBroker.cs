using BotCore.Data;
using BotCore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BCore.Brokers
{
    class MobinBroker : IBroker
    {
        public void Login()
        {
            throw new NotImplementedException();
        }

        public void SendOrder()
        {
            throw new NotImplementedException();
        }

        private async Task<OrderOpt> SendOrderTask(OrderOpt order)
        {
            DateTime duration = DateTime.Now;
            DateTime sendNow;
            string orderID;
            do
            {
                // OrderId = 3020080500180099 , OrderID = 0
                sendNow = DateTime.Now;
                if ((sendNow.Subtract(order.WhenSent).TotalMilliseconds > 300) || order.TrySend == 0)
                {
                    order.WhenSent = DateTime.Now;
                    order.TrySend++;
                    orderID = await SendOrderItem(order);
                    if (orderID != "0")
                    {
                        order.OrderId = orderID;
                        order.Done = true;
                        // await hubContext.Clients.All.SendAsync("msg", $"{order.SymboleName},{order.Count},{order.Price},STAT:{order.Stat},OrderId:{order.OrderId}, Try:{order.TrySend}");
                    }
                }
            } while (!order.Done && sendNow.Subtract(duration).TotalSeconds <= 20);
            return order;
        }

        private async Task<string> SendOrderItem(BOrder order)
        {
            string orderID = "0";
            var req = new HttpRequestMessage(HttpMethod.Post, "https://api2.mobinsb.com/Web/V1/Order/Post");
            req.Headers.Add("Accept", "*/*");
            req.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            req.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            req.Headers.Add("Authorization", $"BasicAuthentication {apiToken}");
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

            OrderPayload payload = new OrderPayload();
            payload.IsSymbolCautionAgreement = false;
            payload.CautionAgreementSelected = false;
            payload.IsSymbolSepahAgreement = false;
            payload.SepahAgreementSelected = false;
            payload.orderCount = order.Count;
            payload.orderPrice = order.Price;
            payload.FinancialProviderId = 1;
            payload.minimumQuantity = 0;
            payload.maxShow = 0;
            payload.orderId = 0;
            payload.isin = order.SymboleCode;
            if (order.TypeOrder == "SELL")
                payload.orderSide = "86";
            else if (order.TypeOrder == "BUY")
                payload.orderSide = "65";
            payload.orderValidity = 74;
            payload.orderValiditydate = null;
            payload.shortSellIsEnabled = false;
            payload.shortSellIncentivePercent = 0;

            string str_payload = JsonConvert.SerializeObject(payload);
            req.Content = new StringContent(str_payload, System.Text.Encoding.UTF8, "application/json");
            var httpClient = httpClientFactory.CreateClient("configured-inner-handler");
            HttpResponseMessage httpResponse = await httpClient.SendAsync(req);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                // GetOpenOrder jsonObj = JsonConvert.DeserializeObject<GetOpenOrder>(output);
                orderID = httpResponse.Content.ReadAsStringAsync().Result;
                // "orderid": "3020080500180099",
                // {"Data":{"OrderId":0},"MessageDesc":null,"IsSuccessfull":true,"MessageCode":null,"Version":0}
            }
            return orderID;
        }

        private async Task GetOpenOrders()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "https://api2.mobinsb.com/Web/V1/Order/GetOpenOrder/OpenOrder");
            req.Headers.Add("Accept", "*/*");
            req.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            req.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            req.Headers.Add("Authorization", $"BasicAuthentication {apiToken}");
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

            var httpClient = httpClientFactory.CreateClient("configured-inner-handler");
            HttpResponseMessage httpResponse = await httpClient.SendAsync(req);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                string output = httpResponse.Content.ReadAsStringAsync().Result;
                GetOpenOrder jsonObj = JsonConvert.DeserializeObject<GetOpenOrder>(output);
            }
        }

        private async Task GetCustomerTodayOrders()
        {
            var req = new HttpRequestMessage(HttpMethod.Get, "https://api2.mobinsb.com/Web/V1/Order/GetTodayOrders/Customer/GetCustomerTodayOrders");
            req.Headers.Add("Accept", "*/*");
            req.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            req.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            req.Headers.Add("Authorization", $"BasicAuthentication {apiToken}");
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

            string output = "";
            var httpClient = httpClientFactory.CreateClient("configured-inner-handler");
            HttpResponseMessage httpResponse = await httpClient.SendAsync(req);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                output = httpResponse.Content.ReadAsStringAsync().Result;
                GetOpenOrder jsonObj = JsonConvert.DeserializeObject<GetOpenOrder>(output);
            }
        }

        private void GetOrdersFromDB()
        {
            if (!getOrdersFromDB)
            {
                using (var scope = Services.CreateScope())
                {
                    var scopedDbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    apiToken = scopedDbContext.BCookies.Where(c => c.Name == "apitoken").FirstOrDefault().Value;
                    mainOrders = scopedDbContext.BOrders.Where(t => t.CreatedDateTime.Date == DateTime.Today).ToList();
                    foreach (var o in mainOrders)
                    {
                        OrderOpt tmp = new OrderOpt();
                        tmp.Id = o.Id;
                        tmp.SymboleName = o.SymboleName;
                        tmp.SymboleCode = o.SymboleCode;
                        tmp.Count = o.Count;
                        tmp.Price = o.Price;
                        tmp.TrySend = 0;
                        tmp.WhenSent = DateTime.Now;
                        tmp.TypeOrder = o.TypeOrder;
                        tmp.Done = false;
                        tmp.OrderId = "0";
                        orderOpt.Add(tmp);
                    }
                    getOrdersFromDB = true;
                }
            }
        }

        public async Task<Object> TimeDiff()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://online.mobinsb.com/");
            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Connection", "keep-alive");
            request.Headers.Add("Pragma", "no-cache");
            request.Headers.Add("Host", "online.mobinsb.com");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");

            var httpClient = httpClientFactory.CreateClient();
            DateTime tserver = DateTime.Now;
            Stopwatch stopwatch = Stopwatch.StartNew();
            HttpResponseMessage httpResponse = await httpClient.SendAsync(request);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                stopwatch.Stop();
                // ViewBag.ElapsedTime = stopwatch.ElapsedMilliseconds;

                bool isDate = httpResponse.Headers.TryGetValues("Date", out IEnumerable<string> values);
                string GMT = "NULL";
                foreach (var v in values)
                    GMT = v;
                // Tue, 04 Aug 2020 15:48:30 GMT
                GMT = GMT.Remove(GMT.IndexOf(" GMT"));

                DateTime tmobin = DateTime.ParseExact(GMT, "ddd, dd MMM yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                // ViewBag.MobinTime = tmobin.ToString("yyyy-mm-dd HH:mm:ss.fff");
                TimeSpan tdif = tserver.Subtract(tmobin);
                // ViewBag.DiffTime = tdif.ToString("c");
                // ViewBag.ConvertedTime = tmobin.ToLocalTime().ToString("yyyy-mm-dd HH:mm:ss.fff");
            }
            // ViewBag.ServerTime = tserver.ToString("yyyy-mm-dd HH:mm:ss.fff");
            return View();
        }
    }
}
