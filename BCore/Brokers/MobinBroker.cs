using BCore.DataModel;
using BotCore.Data;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BCore.Lib
{
    class MobinBroker
    {
        private HttpClient SendOpenHttpClient;
        private HttpRequestMessage SendOrderReqMsg;
        private HttpRequestMessage OpenOrdersReqMsg;
        private Stopwatch stopwatch;
        private readonly JsonSerializerOptions serializeOptions;
        private readonly HttpClientHandler httpHandler;
        
        public string Token { get; set; }
        public BOrder Order { get; set ; }

        public long ElapsedTime { get; private set; }
        public bool IsSuccessfull { get; private set; } = false;
        public string MessageDesc { get; private set; } = "";

        public MobinBroker()
        {
            httpHandler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli
            };
            serializeOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };
            GetHttpClientForSendOpenOrders();
            CreateSendingOrderRequest();
        }

        private void CreateSendingOrderRequest()
        {
            SendOrderReqMsg = new HttpRequestMessage(HttpMethod.Post, "/Web/V1/Order/Post"); // [https]://api2.mobinsb.com/Web/V1/Order/Post
            SendOrderReqMsg.Headers.Add("Authorization", $"BasicAuthentication {Token}");
            var payload = new OrderPayload
            {
                IsSymbolCautionAgreement = false,
                CautionAgreementSelected = false,
                IsSymbolSepahAgreement = false,
                SepahAgreementSelected = false,
                orderCount = Order.Count,
                orderPrice = Order.Price,
                FinancialProviderId = 1,
                minimumQuantity = 0,
                maxShow = 0,
                orderId = 0,
                isin = Order.SymboleCode,
                orderSide = (Order.OrderType == "SELL" ? "86" : "65"), // SELL(86) , BUY(65)
                orderValidity = 74,
                orderValiditydate = null,
                shortSellIsEnabled = false,
                shortSellIncentivePercent = 0
            };
            string str_payload = JsonSerializer.Serialize(payload);
            SendOrderReqMsg.Content = new StringContent(str_payload, Encoding.UTF8, "application/json");
        }

        public async Task SendOrder()
        {
            // {"Data":{"OrderId":0},"MessageDesc":null,"IsSuccessfull":true,"MessageCode":null,"Version":0}
            // {"Data": null,"MessageDesc": "وضعیت گروه اجازه این فعالیت را نمی دهد","IsSuccessfull": false,"MessageCode": null,"Version": 0}

            stopwatch = Stopwatch.StartNew();
            HttpResponseMessage httpResponse = await SendOpenHttpClient.SendAsync(SendOrderReqMsg);
            stopwatch.Stop();
            ElapsedTime = stopwatch.ElapsedMilliseconds;
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = await httpResponse.Content.ReadAsStringAsync();
                OrderRespond orderRespond = JsonSerializer.Deserialize<OrderRespond>(content, serializeOptions);
                IsSuccessfull = orderRespond.IsSuccessfull;
                MessageDesc = orderRespond.MessageDesc;
            }
        }

        public async Task<GetOpenOrder> GetOpenOrders()
        {
            GetOpenOrder openOrders = null;
            // [https]://api2.mobinsb.com/Web/V1/Order/GetOpenOrder/OpenOrder
            OpenOrdersReqMsg = new HttpRequestMessage(HttpMethod.Get, "/Web/V1/Order/GetOpenOrder/OpenOrder");
            OpenOrdersReqMsg.Headers.Add("Authorization", $"BasicAuthentication {Token}");

            Stopwatch stopwatch = Stopwatch.StartNew();
            HttpResponseMessage httpResponse = await SendOpenHttpClient.SendAsync(OpenOrdersReqMsg);
            if (httpResponse.IsSuccessStatusCode)
            {
                stopwatch.Stop();
                ElapsedTime = stopwatch.ElapsedMilliseconds;

                var res = await httpResponse.Content.ReadAsStringAsync();
                openOrders = JsonSerializer.Deserialize<GetOpenOrder>(res);
            }
            return openOrders;
        }

        private HttpClient GetHttpClientForSendOpenOrders()
        {
            SendOpenHttpClient = new HttpClient(httpHandler)
            {
                BaseAddress = new Uri("https://api2.mobinsb.com")
            };
            SendOpenHttpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            SendOpenHttpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            SendOpenHttpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            SendOpenHttpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            SendOpenHttpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            SendOpenHttpClient.DefaultRequestHeaders.Add("Host", "api2.mobinsb.com");
            SendOpenHttpClient.DefaultRequestHeaders.Add("Origin", "https://silver.mobinsb.com");
            SendOpenHttpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
            SendOpenHttpClient.DefaultRequestHeaders.Add("Referer", "https://silver.mobinsb.com/Home/Default/page-1");
            SendOpenHttpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            SendOpenHttpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            SendOpenHttpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");
            SendOpenHttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            SendOpenHttpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            return SendOpenHttpClient;
        }
    }
}
