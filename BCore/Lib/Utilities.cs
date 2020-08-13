using BCore.Data;
using BCore.DataModel;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BCore.Lib
{
    class Utilities
    {
        private readonly ApplicationDbContext db;

        public Utilities(ApplicationDbContext db)
        {
            this.db = db;
        }

        public static HttpClient GetPresetHttpClientForSendOrders()
        {
            HttpClient HttpClientForSendOrder = new HttpClient
            {
                BaseAddress = new Uri("https://api2.mobinsb.com")
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

        public static HttpClient GetPresetHttpClientForOpenOrders()
        {
            HttpClient OpenOrders = new HttpClient
            {
                BaseAddress = new Uri("https://api2.mobinsb.com")
            };
            OpenOrders.DefaultRequestHeaders.Add("Accept", "*/*");
            OpenOrders.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            OpenOrders.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            OpenOrders.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            OpenOrders.DefaultRequestHeaders.Add("Connection", "keep-alive");
            OpenOrders.DefaultRequestHeaders.Add("Host", "api2.mobinsb.com");
            OpenOrders.DefaultRequestHeaders.Add("Origin", "https://silver.mobinsb.com");
            OpenOrders.DefaultRequestHeaders.Add("Pragma", "no-cache");
            OpenOrders.DefaultRequestHeaders.Add("Referer", "https://silver.mobinsb.com/Home/Default/page-1");
            OpenOrders.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
            OpenOrders.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
            OpenOrders.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-site");
            OpenOrders.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            OpenOrders.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
            return OpenOrders;
        }

        public static HttpClient GetPresetHttpClientForInitCookies()
        {
            HttpClient HttpClientForInitCookies = new HttpClient
            {
                BaseAddress = new Uri("https://silver.mobinsb.com")
            };
            HttpClientForInitCookies.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            HttpClientForInitCookies.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            HttpClientForInitCookies.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            HttpClientForInitCookies.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            HttpClientForInitCookies.DefaultRequestHeaders.Add("Connection", "keep-alive");
            HttpClientForInitCookies.DefaultRequestHeaders.Add("Pragma", "no-cache");
            HttpClientForInitCookies.DefaultRequestHeaders.Add("Referer", "https://silver.mobinsb.com/");
            HttpClientForInitCookies.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            return HttpClientForInitCookies;
        }

        public static HttpClient GetPresetHttpClientForGetCaptcha()
        {
            HttpClient HttpClientForGetCaptcha = new HttpClient
            {
                BaseAddress = new Uri("https://silver.mobinsb.com")
            };
            HttpClientForGetCaptcha.DefaultRequestHeaders.Add("Accept", "image/webp,image/apng,image/*,*/*;q=0.8");
            HttpClientForGetCaptcha.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            HttpClientForGetCaptcha.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            HttpClientForGetCaptcha.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            HttpClientForGetCaptcha.DefaultRequestHeaders.Add("Connection", "keep-alive");
            HttpClientForGetCaptcha.DefaultRequestHeaders.Add("Pragma", "no-cache");
            HttpClientForGetCaptcha.DefaultRequestHeaders.Add("Host", "silver.mobinsb.com");
            HttpClientForGetCaptcha.DefaultRequestHeaders.Add("Referer", "https://silver.mobinsb.com/login");
            HttpClientForGetCaptcha.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            return HttpClientForGetCaptcha;
        }

        public static HttpClient GetPresetHttpClientForMobinLogin()
        {
            HttpClientHandler httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = false, // , UseCookies                
            };
            HttpClient HttpClientForMobinLogin = new HttpClient(httpClientHandler)
            {
                BaseAddress = new Uri("https://silver.mobinsb.com")
            };

            HttpClientForMobinLogin.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            HttpClientForMobinLogin.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            HttpClientForMobinLogin.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            HttpClientForMobinLogin.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            HttpClientForMobinLogin.DefaultRequestHeaders.Add("Connection", "keep-alive");
            HttpClientForMobinLogin.DefaultRequestHeaders.Add("Pragma", "no-cache");
            HttpClientForMobinLogin.DefaultRequestHeaders.Add("Host", "silver.mobinsb.com");
            HttpClientForMobinLogin.DefaultRequestHeaders.Add("Origin", "https://silver.mobinsb.com");
            HttpClientForMobinLogin.DefaultRequestHeaders.Add("Referer", "https://silver.mobinsb.com/login");
            HttpClientForMobinLogin.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            return HttpClientForMobinLogin;
        }

        public static HttpClient GetPresetHttpClientForMobinApiToken()
        {
            HttpClient MobinApiToken = new HttpClient
            {
                BaseAddress = new Uri("https://silver.mobinsb.com")
            };

            MobinApiToken.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            MobinApiToken.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            MobinApiToken.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
            MobinApiToken.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
            MobinApiToken.DefaultRequestHeaders.Add("Connection", "keep-alive");
            MobinApiToken.DefaultRequestHeaders.Add("Pragma", "no-cache");
            MobinApiToken.DefaultRequestHeaders.Add("Host", "silver.mobinsb.com");
            MobinApiToken.DefaultRequestHeaders.Add("Referer", "https://silver.mobinsb.com/login");
            MobinApiToken.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.105 Safari/537.36");
            return MobinApiToken;
        }

        public async Task InitCookies()
        {
            var httpClient = Utilities.GetPresetHttpClientForInitCookies();
            var request = new HttpRequestMessage(HttpMethod.Get, "/login");
            HttpResponseMessage httpResponse = await httpClient.SendAsync(request);
            var cookies = ExtractCookiesFromHeader(httpResponse.Headers);
            await SaveCookiesToDataBase(cookies);
        }

        public static HttpRequestMessage InitOrderReqHeader(BOrder order, string ApiToken)
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

        public static HttpRequestMessage InitGetOpenOrdersReqHeader(string ApiToken)
        {
            // https://api2.mobinsb.com/Web/V1/Order/GetOpenOrder/OpenOrder
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Get, "/Web/V1/Order/GetOpenOrder/OpenOrder");
            req.Headers.Add("Authorization", $"BasicAuthentication {ApiToken}");
            return req;
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

        public async Task<Bitmap> GetCaptcha()
        {
            var httpClient = GetPresetHttpClientForGetCaptcha();
            Random r = new Random();
            string capchaUrl = "/" + Math.Floor((r.NextDouble() * 10000000) + 1) + "/Account/Captcha?postfix=" + Math.Floor((r.NextDouble() * 10000000) + 1);
            var capchaRequest = new HttpRequestMessage(HttpMethod.Get, capchaUrl);

            var tmp1 = await db.BSettings.Where(s => s.Key == "ASP.NET_Session").FirstOrDefaultAsync();
            var tmp2 = await db.BSettings.Where(s => s.Key == "TS0102390e").FirstOrDefaultAsync();
            if (tmp1.Value != "" || tmp2.Value != "")
            {
                string t1 = tmp1.Key + "=" + tmp1.Value + "; ";
                string t2 = tmp2.Key + "=" + tmp2.Value + "; ";
                string temp = (t1 + t2).Trim();
                capchaRequest.Headers.Add("Cookie", temp.Substring(0, temp.LastIndexOf(";")));
            }
            var httpResponse = await httpClient.SendAsync(capchaRequest);
            var imgStream = await httpResponse.Content.ReadAsStreamAsync(); // ReadAsByteArrayAsync();
            return new Bitmap(imgStream);
        }

        public async Task<string> LoginToMobinBroker(string username, string password, string captcha)
        {
            var httpClient = GetPresetHttpClientForMobinLogin();
            var loginRequest = new HttpRequestMessage(HttpMethod.Post, "/login");

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
            HttpResponseMessage loginResponse = await httpClient.SendAsync(loginRequest);
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
            var httpClientApiToken = GetPresetHttpClientForMobinApiToken();
            var homeRequest = new HttpRequestMessage(HttpMethod.Get, "/Home/Default/page-1");

            string t_str = await GetLoginCookies();
            homeRequest.Headers.Add("Cookie", t_str.Substring(0, t_str.LastIndexOf(";")));

            HttpResponseMessage homeResponse = await httpClientApiToken.SendAsync(homeRequest);
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

        private void StartSendingOrder(BOrder order, string token, int interval, DateTime stopTime)
        {
            // Timer timer = new Timer(                );
            HttpClient httpClient = GetPresetHttpClientForSendOrders();
            DateTime LastTrySendTime = DateTime.Now.AddMilliseconds(-interval);
            HttpRequestMessage orderReq = InitOrderReqHeader(order, token);

            do
            {
                if (DateTime.Now.Subtract(LastTrySendTime).TotalMilliseconds >= interval)
                {
                    Task.Run(() => httpClient.SendAsync(orderReq));
                    LastTrySendTime = DateTime.Now;
                }
            } while (TimeSpan.Compare(DateTime.Now.TimeOfDay, stopTime.TimeOfDay) < 0);
        }

        private async Task<string> SendOrderItem(BOrder order, string ApiToken)
        {
            HttpClient _http = Utilities.GetPresetHttpClientForSendOrders();
            string output = "";
            var req = Utilities.InitOrderReqHeader(order, ApiToken);

            Stopwatch stopwatch = Stopwatch.StartNew();
            HttpResponseMessage httpResponse = await _http.SendAsync(req);
            
            // if (httpResponse.StatusCode == HttpStatusCode.OK)
            if (httpResponse.IsSuccessStatusCode)
            {
                stopwatch.Stop();
                output += $"Elapsed-Time: {stopwatch.ElapsedMilliseconds} ms" + Environment.NewLine;

                var resContent = await httpResponse.Content.ReadAsStringAsync();
                // {"Data":{"OrderId":0},"MessageDesc":null,"IsSuccessfull":true,"MessageCode":null,"Version":0}
                OrderRespond orderRespond = JsonConvert.DeserializeObject<OrderRespond>(resContent);
                if (orderRespond.IsSuccessfull)
                {
                    output += $"Return Data => {resContent}";
                }
            }
            return output;
        }
    }
}
