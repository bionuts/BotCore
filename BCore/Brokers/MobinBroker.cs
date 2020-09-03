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
        public static readonly HttpClient SendHttpClient = SetHttpClientForSendingOrders();
        private readonly HttpClient GeneralHttpClient;
        public MobinWebSocket MobinWebSocket;
        private readonly JsonSerializerOptions serializeOptions;
        private readonly HttpClientHandler httpHandler;
        static volatile object locker = new object();

        public static string ResultOfThreads { get; set; } = "";
        // public static bool[] CeaseFire { get; set; }
        // public static Dictionary<KeyValuePair<int, int>, bool> CeaseFire;
        public static Dictionary<Tuple<int, int>, bool> CeaseFire;
        public string LS_Session { get; set; }
        public int LS_Phase { get; set; }
        public string Token { get; set; }
        public BOrder Order { get; set; }
        public long SendingOrderElapsedTime { get; private set; }
        public bool SendingOrderIsSuccessfull { get; private set; } = false;
        public string SendingOrderMessageDesc { get; private set; } = "";

        public MobinBroker()
        {
            db = new ApplicationDbContext();
            MobinWebSocket = new MobinWebSocket();
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

            loginRequest.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("capcha",captcha)
            });

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

        public HttpRequestMessage GetSendingOrderRequestMessage(BOrder order)
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

        public HttpRequestMessage GetSendingOrderRequestMessage(BOrder order, string token)
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "/Web/V1/Order/Post");
            req.Headers.Add("Authorization", $"BasicAuthentication {token}");
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

        public void SendReqThread(RequestsVector paramObject, string times)
        {
            string result;
            DateTime sent;
            Stopwatch _stopwatch = new Stopwatch();
            try
            {
                sent = DateTime.Now;
                Console.WriteLine($"IN: {sent:HH:mm:ss.fff}, T_{Thread.CurrentThread.ManagedThreadId}");
                _stopwatch.Start();
                HttpResponseMessage httpResponse = SendHttpClient.SendAsync(paramObject.REQ).Result;
                _stopwatch.Stop();
                if (httpResponse.IsSuccessStatusCode)
                {
                    string content = httpResponse.Content.ReadAsStringAsync().Result;
                    OrderRespond orderRespond = JsonSerializer.Deserialize<OrderRespond>(content, serializeOptions);
                    if (orderRespond.IsSuccessfull)
                        CeaseFire[paramObject.DicKey] = true;
                    result =
                        $"[{sent:HH:mm:ss.fff}] [{_stopwatch.ElapsedMilliseconds:D3}ms] [User:{paramObject.AccountName}]\t" +
                        $"[OID:{paramObject.OrderID}]\t[{paramObject.SYM}]\t[Count:{paramObject.Count}]\t[{times}]" +
                        $"\tT_{Thread.CurrentThread.ManagedThreadId}\t[{orderRespond.IsSuccessfull}]\tDesc: {orderRespond.MessageDesc}\n";
                }
                else
                {
                    result =
                        $"[{sent:HH:mm:ss.fff}] T_{Thread.CurrentThread.ManagedThreadId}\t[User:{paramObject.AccountName}]\t" +
                        $"[OID:{paramObject.OrderID}], Sym: {paramObject.SYM}, Error: {httpResponse.StatusCode}\n";
                }
            }
            catch (Exception ex)
            {
                result =
                    $"[{DateTime.Now:HH:mm:ss.fff}] T_{Thread.CurrentThread.ManagedThreadId}\t[User:{paramObject.AccountName}]\t" +
                    $"[OID:{paramObject.OrderID}], Sym: {paramObject.SYM},Error: {ex.Message}\n";
            }
            lock (locker)
            {
                ResultOfThreads += result;
            }
        }

        public async Task<string> SendOrder(BOrder order)
        {
            string result = "";
            DateTime sent;
            Stopwatch _stopwatch = new Stopwatch();
            HttpRequestMessage req = GetSendingOrderRequestMessage(order);
            try
            {
                sent = DateTime.Now;
                _stopwatch.Start();
                HttpResponseMessage httpResponse = await SendHttpClient.SendAsync(req);
                _stopwatch.Stop();
                if (httpResponse.IsSuccessStatusCode)
                {
                    string content = await httpResponse.Content.ReadAsStringAsync();
                    OrderRespond orderRespond = JsonSerializer.Deserialize<OrderRespond>(content, serializeOptions);
                    result =
                        $"[{sent:HH:mm:ss.fff}] [{_stopwatch.ElapsedMilliseconds:D3}ms]" +
                        $"[{order.SymboleName}][Count:{order.Count}]" +
                        $"[{orderRespond.IsSuccessfull}] Desc: {orderRespond.MessageDesc}\n";
                }
                else
                {
                    result =
                        $"[{sent:HH:mm:ss.fff}] Sym: {order.SymboleName}, Error: {httpResponse.StatusCode}\n";
                }
            }
            catch (Exception ex)
            {
                result =
                    $"[{DateTime.Now:HH:mm:ss.fff}] Sym: {order.SymboleName}, Error: {ex.Message}\n";
            }
            return result;
        }

        public async Task<string> GetOpenOrders()
        {
            try
            {
                string result = "";
                GetOpenOrder openOrders = null;

                var req = new HttpRequestMessage(HttpMethod.Get, "https://api2.mobinsb.com/Web/V1/Order/GetOpenOrder/OpenOrder");
                req.Headers.Add("Authorization", $"BasicAuthentication {Token}");

                Stopwatch stopwatch = Stopwatch.StartNew();
                HttpResponseMessage httpResponse = await SendHttpClient.SendAsync(req);
                stopwatch.Stop();

                if (httpResponse.IsSuccessStatusCode)
                {
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
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        private static HttpClient SetHttpClientForSendingOrders()
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

        public async Task<bool> CreateSessionForWebSocket()
        {
            var req = new HttpRequestMessage(HttpMethod.Post, "https://push2v7.etadbir.com/lightstreamer/create_session.js");
            req.Headers.Add("Accept", "*/*");
            req.Headers.Add("Accept-Encoding", "gzip, deflate, br");
            req.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            req.Headers.Add("Cache-Control", "no-cache");
            req.Headers.Add("Connection", "keep-alive");
            req.Headers.Add("Pragma", "no-cache");
            req.Headers.Add("Host", "push2v7.etadbir.com");
            req.Headers.Add("Origin", "https://silver.mobinsb.com");
            req.Headers.Add("Referer", "https://silver.mobinsb.com/Home/Default/page-1");
            req.Headers.Add("Sec-Fetch-Dest", "empty");
            req.Headers.Add("Sec-Fetch-Mode", "cors");
            req.Headers.Add("Sec-Fetch-Site", "cross-site");
            req.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");

            LS_Phase = 100 * (int)Math.Round(new Random().NextDouble() * 100);
            req.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("LS_op2", "create"),
                new KeyValuePair<string, string>("LS_phase", LS_Phase.ToString()),  // 100 * f.randomG(100) ==> randomG(a): math.round(math.random()*(a || 1E3 )) 1E3 = 1000
                new KeyValuePair<string, string>("LS_cause","new.api"),
                new KeyValuePair<string, string>("LS_polling","true"),
                new KeyValuePair<string, string>("LS_polling_millis","0"),
                new KeyValuePair<string, string>("LS_idle_millis","0"),
                //  , p = "LS_cid\x3dpcYgxn8m8 feOojyA1T681f3g2.pz479mDv\x26"
                new KeyValuePair<string, string>("LS_cid","pcYgxn8m8 feOojyA1T681f3g2.pz479mDv"),
                new KeyValuePair<string, string>("LS_adapter_set","STOCKLISTDEMO_REMOTE"),
                new KeyValuePair<string, string>("LS_user","777&msb01473118"),
                new KeyValuePair<string, string>("LS_password","777"),
                new KeyValuePair<string, string>("LS_container","lsc")
            });

            try
            {
                HttpResponseMessage res = await GeneralHttpClient.SendAsync(req);
                if (res.IsSuccessStatusCode)
                {
                    // setPhase(2501);start('S93912e248d8e9080M98eT1235238', null, 0, 50000, 'Lightstreamer HTTP Server', '86.57.3.186');loop(0);
                    string output = await res.Content.ReadAsStringAsync();
                    int start = output.IndexOf(";start('");
                    int end = output.IndexOf("',", start);
                    LS_Session = output.Substring(start + 8, end - (start + 8));
                    return true;
                }
                return false;
            }
            catch // (Exception ex)
            {
                // log data by ILogger
                return false;
            }
        }

        public string StayTuneHttpClient(ref DateTime order_time)
        {
            try
            {
                DateTime sent;
                Stopwatch stopwatch;

                var req = new HttpRequestMessage(HttpMethod.Get, "https://api2.mobinsb.com/Web/V1/Order/GetOpenOrder/OpenOrder");
                req.Headers.Add("Authorization", $"BasicAuthentication {Token}");

                sent = DateTime.Now;
                stopwatch = Stopwatch.StartNew();
                HttpResponseMessage httpResponse = SendHttpClient.SendAsync(req).Result;
                stopwatch.Stop();
                if (httpResponse.IsSuccessStatusCode)
                {
                    var mobinDate = httpResponse.Headers.Date;
                    order_time = new DateTime(
                            mobinDate.Value.Year,
                            mobinDate.Value.Month,
                            mobinDate.Value.Day,
                            mobinDate.Value.Hour,
                            mobinDate.Value.Minute,
                            mobinDate.Value.Second,
                            order_time.Millisecond).ToLocalTime();
                    // order_time = recv.ToLocalTime();
                    string content = httpResponse.Content.ReadAsStringAsync().Result;
                    GetOpenOrder openOrders = JsonSerializer.Deserialize<GetOpenOrder>(content);
                    return $"[{stopwatch.ElapsedMilliseconds:D3}ms][Recv: {order_time:HH:mm:ss}][Sent: {sent:HH:mm:ss.fff}] => Orders: {openOrders.Data.Length}, [{openOrders.IsSuccessfull}]";
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetTimeBasedOnOptionHeader(ref DateTime option_time)
        {
            HttpResponseMessage httpResponse = null;
            DateTime sent;
            Stopwatch stopwatch;
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Options, "https://api2.mobinsb.com/Web/V1/Order/Post");
            req.Headers.Add("Access-Control-Request-Headers", "authorization,content-type,x-requested-with");
            req.Headers.Add("Access-Control-Request-Method", "POST");
            try
            {
                sent = DateTime.Now;
                stopwatch = Stopwatch.StartNew();
                httpResponse = SendHttpClient.SendAsync(req).Result;
                stopwatch.Stop();
                if (httpResponse.StatusCode == HttpStatusCode.NoContent)
                {
                    var mobinDate = httpResponse.Headers.Date;
                    option_time = new DateTime(
                            mobinDate.Value.Year,
                            mobinDate.Value.Month,
                            mobinDate.Value.Day,
                            mobinDate.Value.Hour,
                            mobinDate.Value.Minute,
                            mobinDate.Value.Second,
                            option_time.Millisecond).ToLocalTime();
                    // recv = recv.ToLocalTime();
                    return $"[{stopwatch.ElapsedMilliseconds:D3}ms][Recv: {option_time:HH:mm:ss}][Sent: {sent:HH:mm:ss.fff}] => Options";
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (httpResponse != null) httpResponse.Dispose();
            }
        }

        public string GetTimeBasedOnLoginHeader(ref DateTime login_time)
        {
            try
            {
                // "ddd, dd MMM yyyy HH:mm:ss"
                DateTime sent;
                // DateTime recv;
                Stopwatch stopwatch;

                var req = new HttpRequestMessage(HttpMethod.Get, "https://silver.mobinsb.com/login");
                req.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                req.Headers.Add("Referer", "https://silver.mobinsb.com/");

                sent = DateTime.Now;
                stopwatch = Stopwatch.StartNew();
                HttpResponseMessage httpResponse = SendHttpClient.SendAsync(req).Result;
                stopwatch.Stop();
                if (httpResponse.IsSuccessStatusCode)
                {
                    var mobinDate = httpResponse.Headers.Date;
                    login_time = new DateTime(
                            mobinDate.Value.Year,
                            mobinDate.Value.Month,
                            mobinDate.Value.Day,
                            mobinDate.Value.Hour,
                            mobinDate.Value.Minute,
                            mobinDate.Value.Second,
                            login_time.Millisecond).ToLocalTime();
                    // recv = recv.ToLocalTime();
                    return $"[{stopwatch.ElapsedMilliseconds:D3}ms][Recv: {login_time:HH:mm:ss}][Sent: {sent:HH:mm:ss.fff}] => Login";
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
