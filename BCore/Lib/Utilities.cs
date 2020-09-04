using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.RegularExpressions;
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
                DateTime expire = new DateTime(2020, 09, 15);

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
                }
                return false;
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
                DateTime expire = new DateTime(2020, 09, 15);

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

        public static bool GetTimeFromString(string input, out string output)
        {
            output = "";
            // d(1,1,2,'01:21:00')
            // d(1,1,'keyclock','typeclock','08:10:01');
            string pattern = @"(d\(1,1,2,'([0-1][0-9]|[2][0-3]):([0-5][0-9]):[0-5][0-9]'\);)|(d\(1,1,'keyclock','typeclock','([0-1][0-9]|[2][0-3]):([0-5][0-9]):[0-5][0-9]'\);)";
            string InnerPattern = @"([0-1][0-9]|[2][0-3]):([0-5][0-9]):[0-5][0-9]";
            Regex timeRegex = new Regex(pattern);
            Regex InnerTimeRegex = new Regex(InnerPattern);
            Match timeMatch;
            Match InnerTimeMatch;

            timeMatch = timeRegex.Match(input);
            if (timeMatch.Success)
            {
                InnerTimeMatch = InnerTimeRegex.Match(timeMatch.Value);
                output = InnerTimeMatch.Value;
                return true;
            }
            return false;
        }

        public static List<string> SortResult(string result)
        {
            List<string> lineList = new List<string>();
            var lines = result.Split("\n");
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line.Trim()))
                    lineList.Add(line);
            }
            lineList = lineList.OrderBy(p => p.Substring(1, p.IndexOf("]"))).ToList();
            return lineList;
        }

        public static List<string> CalDiff(List<string> SortedLines, DateTime start)
        {
            try
            {
                List<string> ExSortedLines = new List<string>();
                string pattern = @"\[([0-1][0-9]|[2][0-3]):([0-5][0-9]):[0-5][0-9].[0-9][0-9][0-9]\]";
                Regex timeRegex = new Regex(pattern);
                Match timeMatch;
                int startTimeMs = start.Millisecond;
                int startTime_fisrtMs_diff, preMs, recentMs;
                double sum = 0;

                string firstLine = SortedLines[0];
                timeMatch = timeRegex.Match(firstLine);
                if (timeMatch.Success)
                {
                    preMs = int.Parse(timeMatch.Value.Substring(timeMatch.Value.IndexOf(".") + 1, 3));
                    startTime_fisrtMs_diff = preMs >= startTimeMs ? preMs - startTimeMs : 1000 - startTimeMs + preMs;
                    ExSortedLines.Add(firstLine.Insert(firstLine.IndexOf("]") + 2, $"[{startTime_fisrtMs_diff:D3}] "));

                    int cnt = 0;
                    for (int idx = 1; idx < SortedLines.Count; idx++)
                    {
                        timeMatch = timeRegex.Match(SortedLines[idx]);
                        if (timeMatch.Success)
                        {
                            recentMs = int.Parse(timeMatch.Value.Substring(timeMatch.Value.IndexOf(".") + 1, 3));
                            if (recentMs >= preMs)
                            {
                                sum += recentMs - preMs;
                                ExSortedLines.Add(SortedLines[idx].Insert(SortedLines[idx].IndexOf("]") + 2, $"[{(recentMs - preMs):D3}] "));
                            }
                            else
                            {
                                sum += 1000 - preMs + recentMs;
                                ExSortedLines.Add(SortedLines[idx].Insert(SortedLines[idx].IndexOf("]") + 2, $"[{(1000 - preMs + recentMs):D3}] "));
                            }
                            preMs = recentMs;
                            cnt++;
                        }
                    }
                    ExSortedLines.Add($"Mean StepWait: {Math.Round(sum / cnt, 2)} ms");
                    return ExSortedLines;
                }
                return SortedLines;
            }
            catch
            {
                return SortedLines;
            }
        }
    }
}
