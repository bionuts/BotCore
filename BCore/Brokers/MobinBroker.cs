using BCore.Data;
using BCore.DataModel;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

namespace BCore.Lib
{
    class MobinBroker
    {
        private readonly ApplicationDbContext db;
        private HttpClient SendHttpClient;
        private HttpClient GeneralHttpClient;
        private Stopwatch stopwatch;
        private readonly JsonSerializerOptions serializeOptions;
        private readonly HttpClientHandler httpHandler;
        static volatile object locker = new Object();
        public static string resultOfThreads = "";

        public string Token { get; set; }
        public BOrder Order { get; set; }
        public long SendingOrderElapsedTime { get; private set; }
        public bool SendingOrderIsSuccessfull { get; private set; } = false;
        public string SendingOrderMessageDesc { get; private set; } = "";

        public MobinBroker()
        {
            db = new ApplicationDbContext();
            httpHandler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli,
                AllowAutoRedirect = false,
                UseProxy = false,
                Proxy = null
            };
            serializeOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };
            GeneralHttpClient = new HttpClient(httpHandler);
            SetHttpClientForSendingOrders();
        }

        public MobinBroker(string token, BOrder order)
        {
            db = new ApplicationDbContext();
            httpHandler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli,
                AllowAutoRedirect = false,
                UseProxy = false,
                Proxy = null
            };
            serializeOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };
            GeneralHttpClient = new HttpClient(httpHandler);
            SetHttpClientForSendingOrders();
            Token = token;
            Order = order;
        }

        public async Task<bool> InitCookies()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://silver.mobinsb.com/login");
            request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            request.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Connection", "keep-alive");
            request.Headers.Add("Pragma", "no-cache");
            request.Headers.Add("Referer", "https://silver.mobinsb.com/");
            request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");

            HttpResponseMessage httpResponse = await GeneralHttpClient.SendAsync(request);
            if (httpResponse.IsSuccessStatusCode)
            {
                var cookies = ExtractCookiesFromHeader(httpResponse.Headers);
                await SaveCookiesToDataBase(cookies);
                return true;
            }
            return false;
        }

        public async Task<Bitmap> GetCaptcha()
        {
            Random r = new Random();
            string capchaUrl = "https://silver.mobinsb.com/" + Math.Floor((r.NextDouble() * 10000000) + 1) + "/Account/Captcha?postfix=" + Math.Floor((r.NextDouble() * 10000000) + 1);
            var req = new HttpRequestMessage(HttpMethod.Get, capchaUrl);
            req.Headers.Add("Accept", "image/webp,image/apng,image/*,*/*;q=0.8");
            req.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            req.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            req.Headers.Add("Cache-Control", "no-cache");
            req.Headers.Add("Connection", "keep-alive");
            req.Headers.Add("Pragma", "no-cache");
            req.Headers.Add("Host", "silver.mobinsb.com");
            req.Headers.Add("Referer", "https://silver.mobinsb.com/login");
            req.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");

            var tmp1 = await db.BSettings.Where(s => s.Key == "ASP.NET_Session").FirstOrDefaultAsync();
            var tmp2 = await db.BSettings.Where(s => s.Key == "TS0102390e").FirstOrDefaultAsync();
            if (tmp1.Value != "" || tmp2.Value != "")
            {
                string t1 = tmp1.Key + "=" + tmp1.Value + "; ";
                string t2 = tmp2.Key + "=" + tmp2.Value + "; ";
                string temp = (t1 + t2).Trim();
                req.Headers.Add("Cookie", temp.Substring(0, temp.LastIndexOf(";")));
            }
            var httpResponse = await GeneralHttpClient.SendAsync(req);
            var imgStream = await httpResponse.Content.ReadAsStreamAsync();
            return new Bitmap(imgStream);
        }

        public async Task<string> Login(string username, string password, string captcha)
        {
            var loginRequest = new HttpRequestMessage(HttpMethod.Post, "https://silver.mobinsb.com/login");
            loginRequest.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            loginRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            loginRequest.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            loginRequest.Headers.Add("Cache-Control", "no-cache");
            loginRequest.Headers.Add("Connection", "keep-alive");
            loginRequest.Headers.Add("Pragma", "no-cache");
            loginRequest.Headers.Add("Host", "silver.mobinsb.com");
            loginRequest.Headers.Add("Origin", "https://silver.mobinsb.com");
            loginRequest.Headers.Add("Referer", "https://silver.mobinsb.com/login");
            loginRequest.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");

            var tmp1 = await db.BSettings.Where(s => s.Key == "ASP.NET_Session").FirstOrDefaultAsync();
            var tmp2 = await db.BSettings.Where(s => s.Key == "TS0102390e").FirstOrDefaultAsync();
            if (tmp1.Value != "" || tmp2.Value != "")
            {
                string t1 = tmp1.Key + "=" + tmp1.Value + "; ";
                string t2 = tmp2.Key + "=" + tmp2.Value + "; ";
                string temp = (t1 + t2).Trim();
                loginRequest.Headers.Add("Cookie", temp.Substring(0, temp.LastIndexOf(";")));
            }

            FormUrlEncodedContent formData = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("capcha",captcha)
            });
            loginRequest.Content = formData;
            HttpResponseMessage loginResponse = await GeneralHttpClient.SendAsync(loginRequest);
            if (loginResponse.StatusCode == HttpStatusCode.Redirect)
            {
                var cookies = ExtractCookiesFromHeader(loginResponse.Headers);
                await SaveCookiesToDataBase(cookies);
                return await GetApiTokenAfterLogin();
            }
            return "";
        }

        private async Task<string> GetApiTokenAfterLogin()
        {
            var homeRequest = new HttpRequestMessage(HttpMethod.Get, "https://silver.mobinsb.com/Home/Default/page-1");
            homeRequest.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            homeRequest.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            homeRequest.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            homeRequest.Headers.Add("Cache-Control", "no-cache");
            homeRequest.Headers.Add("Connection", "keep-alive");
            homeRequest.Headers.Add("Pragma", "no-cache");
            homeRequest.Headers.Add("Host", "silver.mobinsb.com");
            homeRequest.Headers.Add("Referer", "https://silver.mobinsb.com/login");
            homeRequest.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");

            string t_str = await GetLoginCookies();
            homeRequest.Headers.Add("Cookie", t_str.Substring(0, t_str.LastIndexOf(";")));

            HttpResponseMessage homeResponse = await GeneralHttpClient.SendAsync(homeRequest);
            if (homeResponse.StatusCode == HttpStatusCode.OK)
            {
                string output = await homeResponse.Content.ReadAsStringAsync();

                string ft = "tokenManger.saveToken('";
                int apistart = output.IndexOf(ft);
                int apiend = output.IndexOf("');", apistart + ft.Length);
                string apiToken = output.Substring(apistart + ft.Length, apiend - apistart - ft.Length);

                var apitokenItem = await db.BSettings.Where(s => s.Key == "apitoken").FirstOrDefaultAsync();
                apitokenItem.Value = apiToken;
                db.BSettings.Update(apitokenItem);
                await db.SaveChangesAsync();

                // var BourseCode = 'جها48390';
                int start = output.IndexOf("var BourseCode = '");
                int end = output.IndexOf("';", start);
                string bourseCode = output.Substring(start + 18, end - (start + 18));
                return $"{apiToken}@@@{bourseCode}";
            }
            return "";
        }

        private async Task<string> GetLoginCookies()
        {
            var tmp1 = await db.BSettings.Where(s => s.Key == "ASP.NET_Session").FirstOrDefaultAsync();
            var tmp2 = await db.BSettings.Where(s => s.Key == "TS0102390e").FirstOrDefaultAsync();
            var tmp3 = await db.BSettings.Where(s => s.Key == "Token").FirstOrDefaultAsync();
            var tmp4 = await db.BSettings.Where(s => s.Key == ".ASPXAUTH").FirstOrDefaultAsync();

            string t1 = tmp1.Key + "=" + tmp1.Value + "; ";
            string t2 = tmp2.Key + "=" + tmp2.Value + "; ";
            string t3 = tmp3.Key + "=" + tmp3.Value + "; ";
            string t4 = tmp4.Key + "=" + tmp4.Value + "; ";

            return (t1 + t2 + t3 + t4).Trim();
        }

        public HttpRequestMessage GetSendingOrderRequestMessage()
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "/Web/V1/Order/Post");
            req.Headers.Add("Authorization", $"BasicAuthentication {Token}");
            var payload = new OrderPayload
            {
                CautionAgreementSelected = false,
                FinancialProviderId = 1,
                IsSymbolCautionAgreement = false,
                IsSymbolSepahAgreement = false,
                SepahAgreementSelected = false,
                isin = Order.SymboleCode,
                maxShow = 0,
                minimumQuantity = 0,
                orderCount = Order.Count,
                orderId = 0,
                orderPrice = Order.Price,
                orderSide = (Order.OrderType == "SELL" ? "86" : "65"), // SELL(86) , BUY(65)
                orderValidity = 74,
                orderValiditydate = null,
                shortSellIncentivePercent = 0,
                shortSellIsEnabled = false
            };
            string str_payload = JsonSerializer.Serialize(payload);
            req.Content = new StringContent(str_payload, Encoding.UTF8, "application/json");
            return req;
        }

        public static HttpRequestMessage GetSendingOrderRequestMessageStatic(BOrder order)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "/Web/V1/Order/Post");
            // req.Headers.Add("Authorization", $"BasicAuthentication {token}");
            var payload = new OrderPayload
            {
                CautionAgreementSelected = false,
                FinancialProviderId = 1,
                IsSymbolCautionAgreement = false,
                IsSymbolSepahAgreement = false,
                SepahAgreementSelected = false,
                isin = order.SymboleCode,
                maxShow = 0,
                minimumQuantity = 0,
                orderCount = order.Count,
                orderId = 0,
                orderPrice = order.Price,
                orderSide = (order.OrderType == "SELL" ? "86" : "65"), // SELL(86) , BUY(65)
                orderValidity = 74,
                orderValiditydate = null,
                shortSellIncentivePercent = 0,
                shortSellIsEnabled = false
            };
            string str_payload = JsonSerializer.Serialize(payload);
            req.Content = new StringContent(str_payload, Encoding.UTF8, "application/json");
            return req;
        }

        public async void SendOrder(int tryCount, ReturnedResultObject obj)
        {
            string result;
            DateTime sent;
            HttpRequestMessage req = GetSendingOrderRequestMessage();
            try
            {
                sent = DateTime.Now;
                stopwatch = Stopwatch.StartNew();
                HttpResponseMessage httpResponse = await SendHttpClient.SendAsync(req);
                stopwatch.Stop();
                if (httpResponse.IsSuccessStatusCode)
                {
                    string content = await httpResponse.Content.ReadAsStringAsync();
                    OrderRespond orderRespond = JsonSerializer.Deserialize<OrderRespond>(content, serializeOptions);
                    obj.CeaseFire = orderRespond.IsSuccessfull;
                    result = $"[{sent:HH:mm:ss.fff}] , ID:{Order.Id}, Sym: {Order.SymboleName,-10}, Elps: {stopwatch.ElapsedMilliseconds:D3}ms, " +
                    $"End:[{DateTime.Now:HH:mm:ss.fff}] , Desc: {orderRespond.MessageDesc}, Done: {orderRespond.IsSuccessfull}, " +
                    $"T_{Thread.CurrentThread.ManagedThreadId}\n";
                }
                else
                {
                    result = $"T_{Thread.CurrentThread.ManagedThreadId}, Sym: {Order.SymboleName},Sent: {sent:HH:mm:ss.fff},T_{Thread.CurrentThread.ManagedThreadId},  Error: {httpResponse.StatusCode}\n";
                }
            }
            catch (Exception ex)
            {
                result = $"T_{Thread.CurrentThread.ManagedThreadId}, Sym: {Order.SymboleName},Sent: {DateTime.Now:HH:mm:ss.fff}, Error: {ex.Message}\n";
            }
            lock (locker)
            {
                obj.ResStr += result;
            }
        }

        public void SendOrder(BOrder order, ReturnedResultObject obj)
        {
            string result;
            DateTime sent;
            Stopwatch _stopwatch = new Stopwatch();
            HttpRequestMessage req = GetSendingOrderRequestMessageStatic(order);
            try
            {
                sent = DateTime.Now;
                _stopwatch.Start();
                HttpResponseMessage httpResponse = SendHttpClient.SendAsync(req).Result;
                _stopwatch.Stop();
                if (httpResponse.IsSuccessStatusCode)
                {
                    string content = httpResponse.Content.ReadAsStringAsync().Result;
                    OrderRespond orderRespond = JsonSerializer.Deserialize<OrderRespond>(content, serializeOptions);
                    obj.CeaseFire = orderRespond.IsSuccessfull;
                    result = $"T{Thread.CurrentThread.ManagedThreadId:D3} [{sent:HH:mm:ss.fff}][{_stopwatch.ElapsedMilliseconds:D3}ms] ID:{order.Id},{order.SymboleName} , " +
                        $"[{orderRespond.IsSuccessfull}] Desc: {orderRespond.MessageDesc}\n";
                }
                else
                {
                    result = $"T_{Thread.CurrentThread.ManagedThreadId}, Sym: {order.SymboleName},Sent: {sent:HH:mm:ss.fff},T_{Thread.CurrentThread.ManagedThreadId},  Error: {httpResponse.StatusCode}\n";
                }
            }
            catch (Exception ex)
            {
                result = $"T_{Thread.CurrentThread.ManagedThreadId}, Sym: {order.SymboleName},Sent: {DateTime.Now:HH:mm:ss.fff}, Error: {ex.Message}\n";
            }
            lock (locker)
            {
                resultOfThreads += result;
            }
        }

        public async Task<string> SendOrder()
        {
            string result;
            DateTime sent;
            HttpRequestMessage req = GetSendingOrderRequestMessage();
            try
            {
                sent = DateTime.Now;
                stopwatch = Stopwatch.StartNew();
                HttpResponseMessage httpResponse = await SendHttpClient.SendAsync(req);
                stopwatch.Stop();
                if (httpResponse.IsSuccessStatusCode)
                {
                    string content = await httpResponse.Content.ReadAsStringAsync();
                    OrderRespond orderRespond = JsonSerializer.Deserialize<OrderRespond>(content, serializeOptions);
                    result = $"[{sent:HH:mm:ss.fff}] , ID:{Order.Id}, Sym: {Order.SymboleName,-10}, Elps: {stopwatch.ElapsedMilliseconds:D3}ms, " +
                    $"End:[{DateTime.Now:HH:mm:ss.fff}] , Desc: {orderRespond.MessageDesc}, Done: {orderRespond.IsSuccessfull}, " +
                    $"T_{Thread.CurrentThread.ManagedThreadId}\n";
                }
                else
                {
                    result = $"T_{Thread.CurrentThread.ManagedThreadId}, Sym: {Order.SymboleName},Sent: {sent:HH:mm:ss.fff},T_{Thread.CurrentThread.ManagedThreadId},  Error: {httpResponse.StatusCode}\n";
                }
            }
            catch (Exception ex)
            {
                result = $"T_{Thread.CurrentThread.ManagedThreadId}, Sym: {Order.SymboleName},Sent: {DateTime.Now:HH:mm:ss.fff}, Error: {ex.Message}\n";
            }
            return result;
        }

        public async Task<string> GetOpenOrders()
        {
            GetOpenOrder openOrders = null;
            var req = new HttpRequestMessage(HttpMethod.Get, "https://api2.mobinsb.com/Web/V1/Order/GetOpenOrder/OpenOrder");
            req.Headers.Add("Accept", "*/*");
            req.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            req.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            req.Headers.Add("Authorization", $"BasicAuthentication {Token}");
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

            string result = "";
            Stopwatch stopwatch = Stopwatch.StartNew();
            HttpResponseMessage httpResponse = await GeneralHttpClient.SendAsync(req);
            if (httpResponse.IsSuccessStatusCode)
            {
                stopwatch.Stop();
                string content = await httpResponse.Content.ReadAsStringAsync();
                openOrders = JsonSerializer.Deserialize<GetOpenOrder>(content);
                result = $"\nElapsedTime: {stopwatch.ElapsedMilliseconds}ms, Done: {openOrders.IsSuccessfull}\n";
                result += $"Desc: {openOrders.MessageDesc}\n";
                foreach (var o in openOrders.Data)
                {
                    result += $"Sym: {o.symbol}, Price: {o.orderprice}, Count: {o.qunatity}\n";
                }
            }
            return result;
        }

        private void SetHttpClientForSendingOrders()
        {
            SendHttpClient = new HttpClient(httpHandler)
            {
                BaseAddress = new Uri("https://api2.mobinsb.com")
            };
            SendHttpClient.DefaultRequestHeaders.Add("Accept", "*/*");
            SendHttpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            SendHttpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            SendHttpClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            SendHttpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            SendHttpClient.DefaultRequestHeaders.Add("Host", "api2.mobinsb.com");
            SendHttpClient.DefaultRequestHeaders.Add("Origin", "https://silver.mobinsb.com");
            SendHttpClient.DefaultRequestHeaders.Add("Pragma", "no-cache");
            SendHttpClient.DefaultRequestHeaders.Add("Referer", "https://silver.mobinsb.com/Home/Default/page-1");
            SendHttpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            SendHttpClient.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            SendHttpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");
            SendHttpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            SendHttpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }

        public static HttpClient SetHttpClientForSendingOrdersStatic(string token)
        {
            HttpClientHandler httpHandler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.Brotli,
                AllowAutoRedirect = false,
                UseProxy = false,
                Proxy = null
            };
            HttpClient http = new HttpClient(httpHandler)
            {
                BaseAddress = new Uri("https://api2.mobinsb.com")
            };
            http.DefaultRequestHeaders.Add("Accept", "*/*");
            http.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            http.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            http.DefaultRequestHeaders.Add("Authorization", $"BasicAuthentication {token}");
            http.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            http.DefaultRequestHeaders.Add("Connection", "keep-alive");
            http.DefaultRequestHeaders.Add("Host", "api2.mobinsb.com");
            http.DefaultRequestHeaders.Add("Origin", "https://silver.mobinsb.com");
            http.DefaultRequestHeaders.Add("Pragma", "no-cache");
            http.DefaultRequestHeaders.Add("Referer", "https://silver.mobinsb.com/Home/Default/page-1");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            http.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");
            http.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            http.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            return http;
        }

        private async Task SaveCookiesToDataBase(List<CookieItem> cookies)
        {
            foreach (var cookie in cookies)
            {
                if (cookie.Name == ".ASPXAUTH" && string.IsNullOrEmpty(cookie.Expires))
                {
                    var session = await db.BSettings.Where(s => s.Key == ".ASPXAUTH_").FirstOrDefaultAsync();
                    if (session != null)
                    {
                        session.Value = cookie.Value;
                        db.BSettings.Update(session);
                    }
                }
                else
                {
                    var session = await db.BSettings.Where(s => s.Key == cookie.Name).FirstOrDefaultAsync();
                    if (session != null)
                    {
                        session.Value = cookie.Value;
                        db.BSettings.Update(session);
                    }
                }
            }
            await db.SaveChangesAsync();
        }

        private List<CookieItem> ExtractCookiesFromHeader(HttpResponseHeaders headers)
        {
            var cookies = new List<CookieItem>();
            foreach (var header in headers)
            {
                if (header.Key == "Set-Cookie")
                {
                    foreach (var val in header.Value)
                    {
                        // ASP.NET_Session=uktcewouf3fdxvhdbungw5bl; path=/; HttpOnly; SameSite=Lax; Expires=Wed, 21 Oct 2015 07:28:00 GMT
                        // "expires", "max-age", "path", "domain", "samesite", "HttpOnly", "Secure"
                        var parts = val.Trim().Split("; ");
                        var tcookie = new CookieItem
                        {
                            Name = parts[0].Substring(0, parts[0].IndexOf("=")),
                            Value = parts[0].Substring(parts[0].IndexOf("=") + 1)
                        };
                        foreach (var p in parts)
                        {
                            if (p.ToLower().Contains("expires"))
                            {
                                tcookie.Expires = p.Substring(8);
                            }
                            else if (p.ToLower().Contains("max-age"))
                            {
                                tcookie.MaxAge = p.Substring(8);
                            }
                            else if (p.ToLower().Contains("path"))
                            {
                                // path=/
                                tcookie.Path = p.Substring(5);
                            }
                            else if (p.ToLower().Contains("domain"))
                            {
                                tcookie.Domain = p.Substring(6);
                            }
                            else if (p.ToLower().Contains("samesite"))
                            {
                                tcookie.SameSite = p.Substring(9);
                            }
                            else if (p.ToLower().Contains("httponly"))
                            {
                                tcookie.HttpOnly = "HttpOnly";
                            }
                            else if (p.ToLower().Contains("secure"))
                            {
                                tcookie.Secure = "Secure";
                            }
                        }
                        cookies.Add(tcookie);
                    }
                }
            }
            return cookies;
        }
    }
}
