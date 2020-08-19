using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace BCore.Lib
{
    class Utilities
    {
        public static async Task<bool> CanRunTheApp(HttpClient http)
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

            HttpResponseMessage httpResponse = await http.SendAsync(request);
            if (httpResponse.IsSuccessStatusCode)
            {
                // Date: Wed, 19 Aug 2020 03:40:49 GMT
                bool isDate = httpResponse.Headers.TryGetValues("Date", out IEnumerable<string> values);
                string GMT = "NULL";
                foreach (var v in values)
                    GMT = v;
                // Tue, 04 Aug 2020 15:48:30 GMT
                GMT = GMT.Remove(GMT.IndexOf(" GMT"));
                DateTime tmobin = DateTime.ParseExact(GMT, "ddd, dd MMM yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                DateTime expire = new DateTime(2020, 08, 25);
                if (TimeSpan.Compare(tmobin.TimeOfDay, expire.TimeOfDay) == 1)
                    return false;
            }
            return true;
        }
    }
}
