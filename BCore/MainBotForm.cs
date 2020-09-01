using BCore.Data;
using BCore.DataModel;
using BCore.Forms;
using BCore.Lib;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCore
{
    public partial class MainBotForm : Form
    {
        private MobinBroker MobinAgent;
        private readonly ApplicationDbContext db;
        private List<BOrder> LoadedOrders;
        private DateTime _StartTime;
        private DateTime _EndTime;
        private int Interval;
        private volatile bool can;

        private DateTime PcTime;
        private DateTime WsTime;
        private DateTime OrdersTime;
        private DateTime OptionTime;
        static volatile object locker = new object();

        private ThreadParamObject[] arr_params;
        private readonly HttpClient GenHttp;
        private int StepWait;

        public MainBotForm()
        {
            db = new ApplicationDbContext();
            GenHttp = new HttpClient();
            MobinAgent = new MobinBroker();
            InitializeComponent();
        }

        private async void MainBotForm_Load(object sender, EventArgs e)
        {
            await LoadOrdersToListView();
            // MobinAgent.Token = (await db.BSettings.Where(c => c.Key == "apitoken").FirstOrDefaultAsync()).Value;
            can = await Utilities.CanRunTheApp(GenHttp);
            if (can)
            {
                lbl_done.Text = "[con]";
                lbl_done.BackColor = Color.Green;
                WsTime = OrdersTime = OptionTime = DateTime.Now;
                /*if (await MobinAgent.CreateSessionForWebSocket() && await MobinAgent.MobinWebSocket.StartWebSocket(MobinAgent.LS_Phase, MobinAgent.LS_Session))
                    StartReceiveDataFromWS();*/
                // await MobinAgent.CreateSessionForWebSocket();
                /*await MobinAgent.MobinWebSocket.ConnectAsync();
                await MobinAgent.MobinWebSocket.SendInitMessages(MobinAgent.LS_Phase, MobinAgent.LS_Session);
                await MobinAgent.MobinWebSocket.GetClockMessages(MobinAgent.LS_Session);
                StartReceiveDataFromWS();*/
            }
            else
            {
                lbl_done.Text = "[discon]";
                lbl_done.BackColor = Color.Red;
            }
        }

        private async void btn_load_Click(object sender, EventArgs e)
        {
            if (can)
            {
                using (var db = new ApplicationDbContext())
                {
                    LoadedOrders = await db.BOrders
                        .Where(o => o.CreatedDateTime.Date == DateTime.Today)
                        .Include(x => x.OrderAccounts)
                        .ThenInclude(x => x.BAccount)
                        .OrderBy(d => d.CreatedDateTime)
                        .ToListAsync();

                    // var x = await dbt.BSettings.Where(c => c.Key == "apitoken").FirstAsync();
                    // MobinAgent.Token = x.Value;

                    if (LoadedOrders.Any())
                    {
                        LoadStartAndEndTime(tb_hh.Text.Trim(), tb_mm.Text.Trim(), tb_ss.Text.Trim(), tb_ms.Text.Trim(), tb_duration.Text.Trim());
                        Interval = int.Parse(tb_interval.Text.Trim());
                        InitHttpRequestMessageArray();
                    }
                }
            }
        }

        private async void InitHttpRequestMessageArray()
        {
            List<MultiUserRequest> multiUserRequests = new List<MultiUserRequest>();
            var Users = await (from order in db.BOrders
                               join ord_acc in db.BOrderAccounts on order.Id equals ord_acc.OrderID
                               join acc in db.BAccounts on ord_acc.UserId equals acc.Id
                               where order.CreatedDateTime.Date == DateTime.Today
                               select acc).Distinct().ToListAsync();

            foreach (var user in Users)
            {
                var Orders = await (from order in db.BOrders
                                    join ord_acc in db.BOrderAccounts on order.Id equals ord_acc.OrderID
                                    join acc in db.BAccounts on ord_acc.UserId equals acc.Id
                                    where order.CreatedDateTime.Date == DateTime.Today && acc.Id == user.Id
                                    select order).ToListAsync();
                multiUserRequests.Add(new MultiUserRequest { BAccount = user, Orders = Orders, Qline = 0 });
            }

            StepWait = (Interval + Users.Count - 1) / Users.Count;
            int reqSize = (int)((_EndTime.Subtract(_StartTime).TotalMilliseconds + StepWait - 1) / StepWait);
            arr_params = new ThreadParamObject[reqSize];

            int user_idx = 0;
            for (int j = 0; j < reqSize; j++)
            {
                arr_params[j] = new ThreadParamObject
                {
                    ID = multiUserRequests[user_idx++].Orders[multiUserRequests[user_idx++].Qline].Id,
                    SYM = multiUserRequests[user_idx++].Orders[multiUserRequests[user_idx++].Qline].SymboleName,
                    REQ = MobinAgent.GetSendingOrderRequestMessage(LoadedOrders[whichOne]),
                    WhichOne = whichOne,
                    Count = LoadedOrders[whichOne].Count--
                };
            }

            bool[] ceaseFire = new bool[LoadedOrders.Count];
            int whichOne = 0;
            for (int i = 0; i < reqSize; i++)
            {
                if (whichOne == LoadedOrders.Count) whichOne = 0;
                arr_params[i] = new ThreadParamObject
                {
                    ID = LoadedOrders[whichOne].Id,
                    SYM = LoadedOrders[whichOne].SymboleName,
                    REQ = MobinAgent.GetSendingOrderRequestMessage(LoadedOrders[whichOne]),
                    WhichOne = whichOne,
                    Count = LoadedOrders[whichOne].Count--
                };
                whichOne++;
            }
            MobinAgent.CeaseFire = ceaseFire;
        }

        private void SendOrderRequests()
        {
            try
            {
                int size = arr_params.Length;
                string times;
                Thread.Sleep((int)_StartTime.Subtract(DateTime.Now).TotalMilliseconds);
                for (int i = 0; i < size; i++)
                {
                    if (!MobinAgent.CeaseFire[arr_params[i].WhichOne])
                    {
                        times = $"WS:{WsTime:HH:mm:ss.fff}, Order:{OrdersTime:HH:mm:ss.fff}, Option:{OptionTime:HH:mm:ss.fff}";
                        Task.Factory.StartNew(() => MobinAgent.SendReqThread(arr_params[i], times));
                        Thread.Sleep(StepWait);
                    }
                }
                tb_logs.Invoke((MethodInvoker)delegate { tb_logs.Text = SortResult(MobinBroker.ResultOfThreads); });
            }
            catch (Exception ex)
            {
                tb_logs.Invoke((MethodInvoker)delegate { tb_logs.Text = $"[{ DateTime.Now:HH:mm:ss.fff}] {ex.Message}"; });
            }
        }

        private async void btn_start_Click(object sender, EventArgs e)
        {
            if (can)
            {
                MobinBroker.ResultOfThreads = "";
                ((Button)sender).Enabled = false;
                ((Button)sender).Text = "Running...";
                await Task.Factory.StartNew(() => SendOrderRequests());
                ((Button)sender).Text = "Start";
                ((Button)sender).Enabled = true;
            }
        }

        public async Task LoadOrdersToListView()
        {
            using (var db = new ApplicationDbContext())
            {
                lv_orders.Items.Clear();
                LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today)
                    .Include(x => x.OrderAccounts)
                    .ThenInclude(x => x.BAccount)
                    .OrderBy(d => d.CreatedDateTime).ToListAsync();
                string users;
                foreach (var order in LoadedOrders)
                {
                    decimal templong = order.Count * order.Price;
                    users = "";
                    foreach (var acc in order.OrderAccounts)
                    {
                        users += " , " + acc.BAccount.Name;
                    }

                    var row = new string[]
                    {
                    order.Id.ToString(),
                    order.SymboleName,
                    order.Count.ToString("N0"),
                    order.Price.ToString("N0"),
                    templong.ToString("N0"),
                    order.OrderType,
                    users.Substring(3)
                    };
                    var lv_item = new ListViewItem(row)
                    {
                        UseItemStyleForSubItems = false
                    };
                    lv_item.SubItems[5].BackColor = order.OrderType == "BUY" ? Color.LightGreen : Color.Red;
                    lv_orders.Items.Add(lv_item);
                }
            }
        }

        private void LoadStartAndEndTime(string h, string m, string s, string ms, string duration)
        {
            DateTime tempNow = DateTime.Now;
            _StartTime = new DateTime(tempNow.Year, tempNow.Month, tempNow.Day,
                (h != "" ? int.Parse(h) : 8),
                (m != "" ? int.Parse(m) : 29),
                (s != "" ? int.Parse(s) : 53),
                (ms != "" ? int.Parse(ms) : 500));
            _EndTime = _StartTime.AddSeconds((duration != "" ? double.Parse(duration) : 8.1));
            lbl_endTime.Text = $"End: {_EndTime:HH:mm:ss.fff}";
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
            try
            {
                string tmp = "";
                List<string> lineList = new List<string>();
                var lines = result.Split("\n");
                foreach (var line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                        lineList.Add(line);
                }

                lineList = lineList.OrderBy(p => p.Substring(1, p.IndexOf("]"))).ToList();
                foreach (var line in lineList)
                    tmp += line + Environment.NewLine;
                return tmp;
            }
            catch (Exception ex)
            {
                return ex.Message + Environment.NewLine + result;
            }
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
            PcTime = DateTime.Now;

            WsTime = WsTime.AddSeconds(1);
            OrdersTime = OrdersTime.AddSeconds(1);
            OptionTime = OptionTime.AddSeconds(1);

            lbl_pc_time.Text = $"PC: {PcTime:hh:mm:ss}";
            lbl_ws_time.Text = $"WS: {WsTime:hh:mm:ss} [{(int)PcTime.Subtract(WsTime).TotalMilliseconds} ms]";
            lbl_openorders_time.Text = $"Orders: {OrdersTime:hh:mm:ss} [{(int)PcTime.Subtract(OrdersTime).TotalMilliseconds} ms]";
            lbl_option_time.Text = $"Option: {OptionTime:hh:mm:ss} [{(int)PcTime.Subtract(OptionTime).TotalMilliseconds} ms]";
        }

        private async void StartReceiveDataFromWS()
        {
            string line;
            while (MobinAgent.MobinWebSocket.IS_OPEN)
            {
                line = await MobinAgent.MobinWebSocket.ReceiveDataFromWebSocket();
                if (Utilities.GetTimeFromString(line, out string tmp))
                {
                    string[] timeValues = tmp.Split(":");
                    WsTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                    DateTime.Now.Hour, int.Parse(timeValues[1]), int.Parse(timeValues[2]), WsTime.Millisecond);
                    SetWSLog(line + Environment.NewLine);
                }
            }
            tb_ws_logs.AppendText("Socket has been Closed!!!" + Environment.NewLine);
            tb_ws_logs.ScrollToCaret();
        }

        private void SetWSLog(string str)
        {
            lock (locker)
            {
                tb_ws_logs.AppendText(str);
                // tb_ws_logs.ScrollToCaret();
            }
        }

        private void timer_stay_tune_Tick(object sender, EventArgs e)
        {
            SetWSLog(MobinAgent.StayTuneHttpClient(ref OrdersTime) + Environment.NewLine);
        }

        private void timer_option_Tick(object sender, EventArgs e)
        {
            SetWSLog(MobinAgent.GetTimeBasedOnOptionHeader(ref OptionTime) + Environment.NewLine);
        }

        public void UpdateToken(string token)
        {
            MobinAgent.Token = token;
        }

        public static void DisplayTimerProperties()
        {
            // Display the timer frequency and resolution.
            if (Stopwatch.IsHighResolution)
            {
                Console.WriteLine("Operations timed using the system's high-resolution performance counter.");
            }
            else
            {
                Console.WriteLine("Operations timed using the DateTime class.");
            }

            long frequency = Stopwatch.Frequency;
            Console.WriteLine("  Timer frequency in ticks per second = {0}",
                frequency);
            long nanosecPerTick = (1000L * 1000L * 1000L) / frequency;
            Console.WriteLine("  Timer is accurate within {0} nanoseconds",
                nanosecPerTick);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            tb_logs.AppendText(await MobinAgent.MobinWebSocket.ConnectAsync());
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            tb_logs.AppendText(await MobinAgent.MobinWebSocket.SendInitMessages(MobinAgent.LS_Phase, MobinAgent.LS_Session));
        }
    }
}
