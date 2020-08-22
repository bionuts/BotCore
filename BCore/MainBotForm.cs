using BCore.Data;
using BCore.DataModel;
using BCore.Forms;
using BCore.Lib;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace BCore
{
    public partial class MainBotForm : Form
    {
        private MobinBroker MobinAgent;
        private readonly ApplicationDbContext db;
        private List<BOrder> LoadedOrders;
        private string ApiToken;
        private DateTime _StartTime;
        private DateTime _EndTime;
        private int intervalDuration;
        private volatile bool can;

        private DateTime ServerTime;
        private DateTime SystemTime;

        private ThreadParamObject[] arr_params;
        private HttpClient sendhttp;
        private HttpClient GenHttp;
        static readonly object locker = new object();
        private string resultOfThreads = "";
        private readonly JsonSerializerOptions serializeOptions;
        private int StepWait;
        private static bool[] ceaseFire;

        public MainBotForm()
        {
            db = new ApplicationDbContext();
            serializeOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };
            GenHttp = new HttpClient();
            MobinAgent = new MobinBroker();
            InitializeComponent();
        }

        private async void MainBotForm_Load(object sender, EventArgs e)
        {
            await LoadOrdersToListView();
            can = await Utilities.CanRunTheApp(GenHttp);
            if (can)
            {
                lbl_done.Text = "[connected]";
                lbl_done.BackColor = Color.Green;
                SystemTime = ServerTime = DateTime.Now;

                await MobinAgent.CreateSessionForWebSocket();
                if (await MobinAgent.MobinWebSocket.StartWebSocket(MobinAgent.LS_Phase, MobinAgent.LS_Session)) ReceiveData();
            }
            else
            {
                lbl_done.Text = "[disconnected]";
                lbl_done.BackColor = Color.Red;
            }
        }

        private async void btn_load_Click(object sender, EventArgs e)
        {
            if (can)
            {
                LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today).OrderBy(d => d.CreatedDateTime).ToListAsync();
                ApiToken = (await db.BSettings.Where(c => c.Key == "apitoken").FirstOrDefaultAsync()).Value;
                sendhttp = MobinBroker.SetHttpClientForSendingOrdersStatic(ApiToken);
                if (LoadedOrders.Any())
                {
                    await LoadOrdersToListView();
                    LoadStartAndEndTime(tb_hh.Text.Trim(), tb_mm.Text.Trim(), tb_ss.Text.Trim(), tb_ms.Text.Trim(), tb_duration.Text.Trim());
                    lbl_endTime.Text = $"End: {_EndTime:HH:mm:ss.fff}";
                    intervalDuration = int.Parse(tb_interval.Text.Trim());
                    InitHttpRequestMessageAndThreadArray(LoadedOrders, intervalDuration, _StartTime, _EndTime);
                }
            }
        }

        private void SendOrderRequests()
        {
            try
            {
                int size = arr_params.Length;
                Thread.Sleep((int)_StartTime.Subtract(DateTime.Now).TotalMilliseconds);
                for (int i = 0; i < size; i++)
                {
                    if (ceaseFire[arr_params[i].WhichOne])
                        continue;
                    else
                    {
                        Task.Factory.StartNew(() => SendReq(arr_params[i])); // study here for beter lunch of thread from pool
                        Thread.Sleep(StepWait);
                    }
                }
                tb_logs.Invoke((MethodInvoker)delegate { tb_logs.Text = SortResult(resultOfThreads); });
            }
            catch (Exception ex)
            {
                tb_logs.Invoke((MethodInvoker)delegate { tb_logs.Text = ex.Message; });
            }
        }

        public void SendReq(ThreadParamObject paramObject)
        {
            string result;
            DateTime sent;
            Stopwatch _stopwatch = new Stopwatch();
            try
            {
                sent = DateTime.Now;
                _stopwatch.Start();
                HttpResponseMessage httpResponse = sendhttp.SendAsync(paramObject.REQ).Result;
                _stopwatch.Stop();
                if (httpResponse.IsSuccessStatusCode)
                {
                    string content = httpResponse.Content.ReadAsStringAsync().Result;
                    OrderRespond orderRespond = JsonSerializer.Deserialize<OrderRespond>(content, serializeOptions);
                    if (orderRespond.IsSuccessfull)
                        ceaseFire[paramObject.WhichOne] = true;
                    result = $"[{sent:HH:mm:ss.fff}] [{_stopwatch.ElapsedMilliseconds:D3}ms] [ID:{paramObject.ID}] => {paramObject.SYM:-10} " +
                        $",ThreadID: {Thread.CurrentThread.ManagedThreadId:D3} [{orderRespond.IsSuccessfull}] Desc: {orderRespond.MessageDesc}\n";
                }
                else
                {
                    result = $"T_{Thread.CurrentThread.ManagedThreadId}, Sym: {paramObject.SYM},Sent: {sent:HH:mm:ss.fff}, Error: {httpResponse.StatusCode}\n";
                }
            }
            catch (Exception ex)
            {
                result = $"T_{Thread.CurrentThread.ManagedThreadId}, Sym: {paramObject.SYM},Sent: {DateTime.Now:HH:mm:ss.fff}, Error: {ex.Message}\n";
            }
            lock (locker)
            {
                resultOfThreads += result;
            }
        }

        private void InitHttpRequestMessageAndThreadArray(List<BOrder> orders, int mainInterval, DateTime start, DateTime end)
        {
            StepWait = (mainInterval + orders.Count - 1) / orders.Count;
            int reqSize = (int)((end.Subtract(start).TotalMilliseconds + StepWait - 1) / StepWait);
            arr_params = new ThreadParamObject[reqSize];
            ceaseFire = new bool[orders.Count];
            int whichOne = 0;
            for (int i = 0; i < reqSize; i++)
            {
                if (whichOne == orders.Count) whichOne = 0;
                arr_params[i] = new ThreadParamObject
                {
                    ID = orders[whichOne].Id,
                    SYM = orders[whichOne].SymboleName,
                    REQ = MobinBroker.GetSendingOrderRequestMessageStatic(orders[whichOne]),
                    WhichOne = whichOne
                };
                whichOne++;
            }
        }

        private async void btn_start_Click(object sender, EventArgs e)
        {
            if (can)
            {
                ((Button)sender).Enabled = false;
                ((Button)sender).Text = "Running...";
                await Task.Factory.StartNew(() => SendOrderRequests());
                ((Button)sender).Text = "Start";
                ((Button)sender).Enabled = true;
            }
        }

        public async Task LoadOrdersToListView()
        {
            lv_orders.Items.Clear();
            LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today).OrderBy(d => d.CreatedDateTime).ToListAsync();
            foreach (var order in LoadedOrders)
            {
                decimal templong = order.Count * order.Price;
                var row = new string[]
                {
                    order.Id.ToString(),
                    order.SymboleName,
                    order.Count.ToString("N0"),
                    order.Price.ToString("N0"),
                    templong.ToString("N0"),
                    order.OrderType,
                    "0",
                    "0"
                };
                var lv_item = new ListViewItem(row)
                {
                    UseItemStyleForSubItems = false
                };
                lv_item.SubItems[5].BackColor = order.OrderType == "BUY" ? Color.LightGreen : Color.Red;
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
            if (can)
            {
                Account accountForm = new Account();
                accountForm.ShowDialog();
            }
        }

        private void LoginMenuItemClick(object sender, EventArgs e)
        {
            if (can)
            {
                Login loginForm = new Login();
                loginForm.ShowDialog();
            }
        }

        private void SymboleMenuItemClick(object sender, EventArgs e)
        {
            if (can)
            {
                SymboleForm symForm = new SymboleForm();
                symForm.ShowDialog();
            }
        }

        private void OrderMenuItemClick(object sender, EventArgs e)
        {
            if (can)
            {
                OrderForm orderForm = new OrderForm();
                orderForm.ShowDialog();
            }
        }

        private void TestSendOrderOpenOrderMenuItemClick(object sender, EventArgs e)
        {
            if (can)
            {
                SendOrderForm sendOrderForm = new SendOrderForm();
                sendOrderForm.ShowDialog();
            }
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

        private string SortResult(string result)
        {
            string tmp = "";
            List<string> lineList = new List<string>();
            var lines = result.Split("\n");
            foreach (var line in lines)
                lineList.Add(line);
            lineList = lineList.OrderBy(p => p.Substring(1, p.IndexOf("]"))).ToList();
            foreach (var line in lineList)
                tmp += line + Environment.NewLine;
            return tmp;
        }

        private void timer_cando_Tick(object sender, EventArgs e)
        {
            bool tmp = Utilities.CanRunTheApp2(GenHttp);
            can = tmp;
            if (can)
            {
                lbl_done.Text = "[connected]";
                lbl_done.BackColor = Color.Green;
            }
            else
            {
                lbl_done.Text = "[disconnected]";
                lbl_done.BackColor = Color.Red;
            }
        }

        private void timer_real_time_Tick(object sender, EventArgs e)
        {
            SystemTime = DateTime.Now;
            lbl_system_time.Text = SystemTime.ToString("hh:mm:ss");
            ServerTime = ServerTime.AddSeconds(1);
            lbl_server_time.Text = ServerTime.ToString("hh:mm:ss");
        }

        private async Task<bool> StartWebSocket()
        {
            try
            {
                bool SessIsCreated = await MobinAgent.CreateSessionForWebSocket();
                if (SessIsCreated)
                {
                    await ClientWS.ConnectAsync(wsUri, CancellationToken.None);
                    if (ClientWS.State == WebSocketState.Open)
                    {
                        tb_ws_logs.AppendText("Connected" + Environment.NewLine);

                        string[] sends = new string[2]; // 8
                        sends[0] = $"bind_session{Environment.NewLine}LS_session={MobinAgent.LS_Session}&LS_phase={MobinAgent.LS_Phase + 2}&LS_cause=loop1&LS_container=lsc&";
                        sends[1] = $"control{Environment.NewLine}LS_mode=MERGE&LS_id=getclock&LS_schema=Key%20Type%20Value&LS_data_adapter=clock&LS_snapshot=false&LS_table=1&LS_req_phase=2&LS_win_phase=1&LS_op=add&LS_session={MobinAgent.LS_Session}&";
                        //sends[2] = $"control{Environment.NewLine}LS_mode=MERGE&LS_id=irx6xtpi0006_lightrlc%20irxzxoci0006_lightrlc%20irx6xslc0006_lightrlc%20irx6xs300006_lightrlc%20irxyxtpi0026_lightrlc&LS_schema=ISIN%20SymbolTitle%20LastIndexValue%20IndexChanges%20PercentVariation%20DayOfEvent&LS_data_adapter=TadbirLightRLC&LS_snapshot=true&LS_table=2&LS_req_phase=3&LS_win_phase=1&LS_op=add&";
                        //sends[3] = $"control{Environment.NewLine}LS_mode=MERGE&LS_id=iro1gdir0001_lightrlc%20iro1bsdr0001_lightrlc%20iro1bank0001_lightrlc%20iro3dcaz0001_lightrlc%20iro1fold0001_lightrlc%20iro1zarm0001_lightrlc%20iro7aptp0001_lightrlc%20iro3urpz0001_lightrlc%20iro3mahz0001_lightrlc%20iro3zobz0001_lightrlc%20irr1kshj0101_lightrlc%20iro1kshj0001_lightrlc%20iro1beka0001_lightrlc%20iro1sadr0001_lightrlc%20iro3gedz0001_lightrlc%20iro1tsan0001_lightrlc&LS_schema=LastTradedPrice%20LastTradedPriceVarPercent%20TotalNumberOfSharesTraded%20ClosingPrice%20BestBuyLimitPrice_1%20BestSellLimitPrice_1%20BestBuyLimitQuantity_1%20BestSellLimitQuantity_1%20ClosingPriceVarPercent%20SymbolStateId%20InstrumentCode&LS_data_adapter=TadbirLightRLC&LS_snapshot=true&LS_table=3&LS_req_phase=4&LS_win_phase=1&LS_op=add&";
                        //sends[4] = $"control{Environment.NewLine}LS_mode=RAW&LS_id=777_msb01473118_lightrlc&LS_schema=text0%20conditionalalert0%20refresh%20logout&LS_data_adapter=TadbirLightPrivateGatewayAdapter&LS_snapshot=false&LS_table=4&LS_req_phase=5&LS_win_phase=1&LS_op=add&";
                        //sends[5] = $"control{Environment.NewLine}LS_mode=RAW&LS_id=777_lightrlc&LS_schema=text0%20conditionalalert0%20refresh%20logout&LS_data_adapter=TadbirLightPrivateGatewayAdapter&LS_snapshot=false&LS_table=5&LS_req_phase=6&LS_win_phase=1&LS_op=add&";
                        //sends[6] = $"control{Environment.NewLine}LS_mode=RAW&LS_id=777_msb01473118_lightrlc&LS_schema=orderstatev4&LS_data_adapter=TadbirLightPrivateGatewayAdapter&LS_snapshot=false&LS_table=6&LS_req_phase=7&LS_win_phase=1&LS_op=add&";
                        //sends[7] = $"control{Environment.NewLine}LS_mode=MERGE&LS_id=textmessage_lightrlc&LS_schema=TextMessage%20TextMessageTitle%20TextMessageDate%20TextMessageTime&LS_data_adapter=TadbirLightRLC&LS_snapshot=true&LS_table=7&LS_req_phase=8&LS_win_phase=1&LS_op=add&";

                        foreach (var s in sends)
                        {
                            WS_SND_BUFFER = new ArraySegment<byte>(Encoding.UTF8.GetBytes(s));
                            await ClientWS.SendAsync(WS_SND_BUFFER, WebSocketMessageType.Text, true, CancellationToken.None);
                            tb_ws_logs.AppendText(s + Environment.NewLine);
                        }
                        return true;
                    }
                    else
                    {
                        tb_ws_logs.AppendText("Error: Socket cant Open!!!!" + Environment.NewLine);
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                tb_ws_logs.AppendText(ex.Message + Environment.NewLine);
                return false;
            }
        }

        // d(1,1,2,'01:21:00')
        static string pattern = @"d\(1,1,2,'([0-1][0-9]|[2][0-3]):([0-5][0-9]):[0-5][0-9]'\);";
        static string InnerPattern = @"([0-1][0-9]|[2][0-3]):([0-5][0-9]):[0-5][0-9]";
        Regex timeRegex = new Regex(pattern);
        Regex InnerTimeRegex = new Regex(InnerPattern);
        Match timeMatch;
        Match InnerTimeMatch;
        string wsRes = "";
        private async void ReceiveData()
        {
            while (ClientWS.State == WebSocketState.Open)
            {
                try
                {
                    wsRes = "";
                    var result = await ClientWS.ReceiveAsync(WS_BUFFER, CancellationToken.None);
                    wsRes += Encoding.UTF8.GetString(WS_BUFFER.Array, 0, result.Count);
                    while (!result.EndOfMessage)
                    {
                        wsRes += Encoding.UTF8.GetString(WS_BUFFER.Array, 0, result.Count);
                    }

                    timeMatch = timeRegex.Match(wsRes);
                    if (timeMatch.Success)
                    {
                        InnerTimeMatch = InnerTimeRegex.Match(timeMatch.Value);
                        string[] timeValues = InnerTimeMatch.Value.Split(":");
                        ServerTime = new DateTime(
                            DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            int.Parse(timeValues[0]), int.Parse(timeValues[1]), int.Parse(timeValues[2]), ServerTime.Millisecond);
                    }

                    tb_ws_logs.AppendText(wsRes + Environment.NewLine);
                    tb_ws_logs.ScrollToCaret();
                }
                catch (Exception ex)
                {
                    tb_ws_logs.AppendText(ex.Message + Environment.NewLine);
                    tb_ws_logs.ScrollToCaret();
                }
            }

        }

        private void timer_stay_tune_Tick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MobinAgent.Token))
            {
                tb_stay_tune.AppendText(MobinAgent.StayTuneHttpClient() + Environment.NewLine);
                tb_stay_tune.ScrollToCaret();
            }
        }
    }
}
