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
        private DateTime _StartTime;
        private DateTime _EndTime;
        private int Interval;
        private volatile bool can;

        private DateTime PcTime;
        private DateTime WsTime;
        private DateTime OpenOrderTime;
        private DateTime OptionTime;
        private DateTime LoginTime;
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
            MobinAgent.Token = (await db.BSettings.Where(c => c.Key == "apitoken").FirstOrDefaultAsync()).Value;
            can = await Utilities.CanRunTheApp(GenHttp);
            if (can)
            {
                lbl_done.Text = "[connected]";
                lbl_done.BackColor = Color.Green;
                PcTime = WsTime = OpenOrderTime = OptionTime = LoginTime = DateTime.Now;
                if (await MobinAgent.CreateSessionForWebSocket() && await MobinAgent.MobinWebSocket.StartWebSocket(MobinAgent.LS_Phase, MobinAgent.LS_Session))
                    StartReceiveDataFromWS();
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
                if (LoadedOrders.Any())
                {
                    LoadStartAndEndTime(tb_hh.Text.Trim(), tb_mm.Text.Trim(), tb_ss.Text.Trim(), tb_ms.Text.Trim(), tb_duration.Text.Trim());
                    lbl_endTime.Text = $"End: {_EndTime:HH:mm:ss.fff}";
                    Interval = int.Parse(tb_interval.Text.Trim());
                    await InitHttpRequestMessageArray();
                }
            }
        }

        private async Task InitHttpRequestMessageArray()
        {
            MobinAgent.Token = (await db.BSettings.Where(c => c.Key == "apitoken").FirstOrDefaultAsync()).Value;
            StepWait = (Interval + LoadedOrders.Count - 1) / LoadedOrders.Count;
            int reqSize = (int)(_EndTime.Subtract(_StartTime).TotalMilliseconds / StepWait);
            reqSize = (reqSize / LoadedOrders.Count) * LoadedOrders.Count;
            arr_params = new ThreadParamObject[reqSize];
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
                    WhichOne = whichOne
                };
                whichOne++;
            }
            MobinAgent.CeaseFire = ceaseFire;
        }

        private void SendOrderRequests()
        {
            try
            {
                // Console.WriteLine($"MainThread: T{Thread.CurrentThread.ManagedThreadId:D3}");
                int size = arr_params.Length;
                Thread.Sleep((int)_StartTime.Subtract(DateTime.Now).TotalMilliseconds);
                for (int i = 0; i < size; i++)
                {
                    if (MobinAgent.CeaseFire[arr_params[i].WhichOne])
                        continue;
                    else
                    {
                        Task.Factory.StartNew(() => MobinAgent.SendReqThread(arr_params[i])); // study here for beter lunch of thread from pool
                        Thread.Sleep(StepWait);
                    }
                    // Console.WriteLine($"Out: {DateTime.Now:HH:mm:ss.fff}");
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
            {
                if (!string.IsNullOrEmpty(line))
                    lineList.Add(line);
            }

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
            WsTime = WsTime.AddSeconds(1);
            PcTime = DateTime.Now;

            lbl_pc_time.Text = PcTime.ToString("hh:mm:ss");
            lbl_ws_time.Text = WsTime.ToString("hh:mm:ss");


            lbl_time_diff.Text = $"{(int)PcTime.Subtract(WsTime).TotalMilliseconds} ms";
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
                    tb_ws_logs.ScrollToCaret();
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
            SetWSLog(MobinAgent.StayTuneHttpClient() + Environment.NewLine);
        }

        private void timer_option_Tick(object sender, EventArgs e)
        {
            SetWSLog(MobinAgent.GetTimeBasedOnOptionHeader() + Environment.NewLine);
        }

        private void timer_login_Tick(object sender, EventArgs e)
        {
            SetWSLog(MobinAgent.GetTimeBasedOnLoginHeader() + Environment.NewLine);
        }
    }
}
