using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCore.Lib
{
    class Utilities
    {
        public static async Task<bool> CanRunTheApp(HttpClient http)
        {
            try
            {
                DateTime expire = new DateTime(2020, 08, 25);

                var request = new HttpRequestMessage(HttpMethod.Get, "https://online.mobinsb.com/");
                request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
                request.Headers.Add("Cache-Control", "no-cache");
                request.Headers.Add("Connection", "keep-alive");
                request.Headers.Add("Pragma", "no-cache");
                request.Headers.Add("Host", "online.mobinsb.com");
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");

                HttpResponseMessage httpResponse = await http.SendAsync(request);
                if (httpResponse.IsSuccessStatusCode)
                {
                    // Wed, 19 Aug 2020 03:40:49 GMT
                    var mobinDate = httpResponse.Headers.Date;
                    if (mobinDate.HasValue)
                    {
                        DateTime mobin = new DateTime(mobinDate.Value.Year, mobinDate.Value.Month, mobinDate.Value.Day);
                        if (expire.Subtract(mobin).TotalDays < 0)
                        {
                            MessageBox.Show(
                            "An unhandled exception of type 'System.Runtime.InteropServices.ExternalException' occurred in System.WinXP.dll",
                            "Generic System Runtime Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CanRunTheApp2(HttpClient http)
        {
            try
            {
                DateTime expire = new DateTime(2020, 08, 25);

                var request = new HttpRequestMessage(HttpMethod.Get, "https://online.mobinsb.com/");
                request.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                request.Headers.Add("Accept-Encoding", "gzip, deflate, br");
                request.Headers.Add("Accept-Language", "en-US,en;q=0.9,la;q=0.8,fa;q=0.7,ar;q=0.6,fr;q=0.5");
                request.Headers.Add("Cache-Control", "no-cache");
                request.Headers.Add("Connection", "keep-alive");
                request.Headers.Add("Pragma", "no-cache");
                request.Headers.Add("Host", "online.mobinsb.com");
                request.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36");

                HttpResponseMessage httpResponse = http.SendAsync(request).Result;
                if (httpResponse.IsSuccessStatusCode)
                {
                    // Wed, 19 Aug 2020 03:40:49 GMT
                    var mobinDate = httpResponse.Headers.Date;
                    if (mobinDate.HasValue)
                    {
                        DateTime mobin = new DateTime(mobinDate.Value.Year, mobinDate.Value.Month, mobinDate.Value.Day);
                        if (expire.Subtract(mobin).TotalDays > 0)
                        {
                            return true;
                        }
                        else
                        {
                            MessageBox.Show(
                            "An unhandled exception of type 'System.Runtime.InteropServices.ExternalException' occurred in System.WinXP.dll",
                            "Generic System Runtime Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Application.Exit();
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
