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
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace BCore
{
    public partial class MainBotForm : Form
    {
        private readonly ApplicationDbContext db;
        private List<BOrder> LoadedOrders;
        private string ApiToken;
        private Task[] orderTasks_;
        private Thread[] orderThread;
        private Thread OrdersThread;
        // private Task OrdersTask;
        private DateTime _StartTime;
        private DateTime _EndTime;

        private HttpRequestMessage[] arr_req;
        private ThreadParamObject[] arr_params;
        private Thread[] arr_thread;
        private Task[] OrderTasks;
        private HttpClient sendhttp;
        static volatile object locker = new Object();
        private string resultOfThreads = "";
        private readonly JsonSerializerOptions serializeOptions;
        private int StepWait;

        public MainBotForm()
        {
            db = new ApplicationDbContext();
            serializeOptions = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
            };
            InitializeComponent();
        }

        private async void MainBotForm_Load(object sender, EventArgs e)
        {
            await LoadOrdersToListView();
        }

        private async void btn_load_Click(object sender, EventArgs e)
        {
            LoadedOrders = await db.BOrders.Where(o => o.CreatedDateTime.Date == DateTime.Today).OrderBy(d => d.CreatedDateTime).ToListAsync();
            ApiToken = (await db.BSettings.Where(c => c.Key == "apitoken").FirstOrDefaultAsync()).Value;
            sendhttp = MobinBroker.SetHttpClientForSendingOrdersStatic(ApiToken);
            if (LoadedOrders.Any())
            {
                await LoadOrdersToListView();
                LoadStartAndEndTime(tb_hh.Text.Trim(), tb_mm.Text.Trim(), tb_ss.Text.Trim(), tb_ms.Text.Trim(), tb_duration.Text.Trim());
                lbl_endTime.Text = $"End: {_EndTime:HH:mm:ss.fff}";
                InitHttpRequestMessageAndThreadArray(LoadedOrders, int.Parse(tb_interval.Text.Trim()), _StartTime, _EndTime);
                /*orderTasks = new Task[LoadedOrders.Count];
                orderThread = new Thread[LoadedOrders.Count];
                int step = int.Parse(tb_interval.Text.Trim()) / LoadedOrders.Count;
                int dot = 0;
                int idx = 0;
                foreach (var order in LoadedOrders)
                {
                    DateTime stime = _StartTime.AddMilliseconds(dot);
                    DateTime etime = _EndTime.AddMilliseconds(dot);
                    int intval = int.Parse(tb_interval.Text.Trim());
                    // orderTasks[idx] = new Task(() => SendOrderTask(new MobinBroker(ApiToken, order), stime, etime, intval));
                    orderThread[idx] = new Thread(() => SendOrderTask(new MobinBroker(ApiToken, order), stime, etime, intval));
                    idx++;
                    dot += step;
                }*/
                // OrdersTask = new Task(() => SendAllOrders(LoadedOrders, int.Parse(tb_interval.Text.Trim()), _StartTime, _EndTime));
                /*OrdersThread = new Thread(() => SendAllOrders(LoadedOrders, int.Parse(tb_interval.Text.Trim()), _StartTime, _EndTime))
                {
                    Priority = ThreadPriority.Highest
                };*/
            }
        }

        private async Task SendAllOrders(List<BOrder> orders, int mainInterval, DateTime start, DateTime end)
        {
            ReturnedResultObject obj = new ReturnedResultObject
            {
                ResStr = "",
                CeaseFire = false
            };
            int step = (mainInterval + orders.Count - 1) / orders.Count;
            int forloop = (int)((end.Subtract(start).TotalMilliseconds + step - 1) / step);
            int whichOne = 0;
            MobinBroker mobin = new MobinBroker
            {
                Token = ApiToken
            };
            // Thread.Sleep((int)start.Subtract(DateTime.Now).TotalMilliseconds - 20);
            await Task.Delay((int)start.Subtract(DateTime.Now).TotalMilliseconds - 20);
            for (int i = 0; i < forloop; i++)
            {
                // Task.Run(() => mobin.SendOrder(orders[whichOne++], obj));
                if (whichOne == orders.Count) whichOne = 0;
                await Task.Delay(step);
            }

            tb_logs.Invoke((MethodInvoker)delegate { tb_logs.Text = MobinBroker.resultOfThreads.Replace("\n", Environment.NewLine); });
        }

        private void SendOrderRequests()
        {
            try
            {
                int size = arr_params.Length;
                while (TimeSpan.Compare(DateTime.Now.TimeOfDay, _StartTime.TimeOfDay) < 0) ;
                //Console.WriteLine($"Start: {_StartTime:HH:mm:ss.fff}");
                for (int i = 0; i < size; i++)
                {
                    arr_thread[i].Start(arr_params[i]);
                    _StartTime = _StartTime.AddMilliseconds(StepWait);
                    while (TimeSpan.Compare(DateTime.Now.TimeOfDay, _StartTime.TimeOfDay) < 0) ;
                    //Console.WriteLine(_StartTime.ToString("HH:mm:ss.fff"));
                }
                Thread.Sleep(1000);
                tb_logs.Invoke((MethodInvoker)delegate { tb_logs.Text = resultOfThreads.Replace("\n", Environment.NewLine); });
            }
            catch (Exception ex)
            {
                tb_logs.Invoke((MethodInvoker)delegate { tb_logs.Text = ex.Message; });
            }
        }

        private void InitHttpRequestMessageAndThreadArray(List<BOrder> orders, int mainInterval, DateTime start, DateTime end)
        {
            StepWait = (mainInterval + orders.Count - 1) / orders.Count;
            int reqSize = (int)((end.Subtract(start).TotalMilliseconds + StepWait - 1) / StepWait);
            arr_req = new HttpRequestMessage[reqSize];
            arr_params = new ThreadParamObject[reqSize];
            arr_thread = new Thread[reqSize];
            int whichOne = 0;
            for (int i = 0; i < reqSize; i++)
            {
                if (whichOne == orders.Count) whichOne = 0;
                arr_params[i] = new ThreadParamObject
                {
                    ID = orders[whichOne].Id,
                    SYM = orders[whichOne].SymboleName,
                    REQ = MobinBroker.GetSendingOrderRequestMessageStatic(orders[whichOne])
                };
                arr_thread[i] = new Thread(new ParameterizedThreadStart(SendReq));
                whichOne++;
            }
        }

        public void SendReq(object p)
        {
            ThreadParamObject paramObject = (ThreadParamObject)p;
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
                    // obj.CeaseFire = orderRespond.IsSuccessfull;
                    result = $"T{Thread.CurrentThread.ManagedThreadId:D3} [{sent:HH:mm:ss.fff}][{_stopwatch.ElapsedMilliseconds:D3}ms] ID:{paramObject.ID} , {paramObject.SYM} , " +
                        $"[{orderRespond.IsSuccessfull}] Desc: {orderRespond.MessageDesc}\n";
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

        private string CalculateDiff(int[] dif)
        {
            string str = "";
            for (int j = 0; j < dif.Length - 1; j++)
            {
                if (dif[j] < dif[j + 1])
                    str += $"{dif[j + 1] - dif[j]} , ";
                else
                    str += $"{1000 - dif[j] + dif[j + 1]} , ";
            }
            return str;
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            ((Button)sender).Enabled = false;
            ((Button)sender).Text = "Running...";
            Task task = Task.Factory.StartNew(() => SendOrderRequests(), TaskCreationOptions.LongRunning);
            ((Button)sender).Text = "Start";
            ((Button)sender).Enabled = true;
            //OrdersThread.Start();
            //await EnableBtnOneThread();
        }

        private void SendOrderTask(MobinBroker mobin, DateTime startTime, DateTime endTime, int interval)
        {
            ReturnedResultObject obj = new ReturnedResultObject
            {
                ResStr = "",
                CeaseFire = false
            };
            string sym = mobin.Order.SymboleCode;
            int size = (int)Math.Ceiling(endTime.Subtract(startTime).TotalMilliseconds / interval);
            Thread.Sleep((int)startTime.Subtract(DateTime.Now).TotalMilliseconds);
            for (int i = 0; (i < size) && (!obj.CeaseFire); i++)
            {
                Task.Run(() => mobin.SendOrder(i, obj));
                Thread.Sleep(interval);
            }
            lock (locker)
            {
                tb_logs.Invoke((MethodInvoker)delegate { tb_logs.Text += obj.ResStr.Replace("\n", Environment.NewLine); });
            }
        }

        private async Task EnableBtnOneThread()
        {
            bool done = true;
            while (done)
            {
                if (OrdersThread.ThreadState == System.Threading.ThreadState.Stopped)
                    done = false;
                else
                    await Task.Delay(500);
            }
            btn_start.Invoke((MethodInvoker)delegate { btn_start.Enabled = true; btn_start.Text = "Start"; });
        }

        public async Task EnableBtn()
        {
            int ts = 0;
            int size = orderThread.Length;
            bool done = true;
            while (done)
            {
                for (int i = 0; i < orderThread.Length; i++)
                {
                    if (orderThread[i].ThreadState == System.Threading.ThreadState.Stopped)
                        ts++;
                }
                if (ts == size)
                    done = false;
                else
                {
                    ts = 0;
                    await Task.Delay(500);
                }
            }

            btn_start.Invoke((MethodInvoker)delegate { btn_start.Enabled = true; btn_start.Text = "Start"; });
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
            Account accountForm = new Account();
            accountForm.ShowDialog();
        }

        private void LoginMenuItemClick(object sender, EventArgs e)
        {
            Login loginForm = new Login();
            loginForm.ShowDialog();
        }

        private void SymboleMenuItemClick(object sender, EventArgs e)
        {
            SymboleForm symForm = new SymboleForm();
            symForm.ShowDialog();
        }

        private void OrderMenuItemClick(object sender, EventArgs e)
        {
            OrderForm orderForm = new OrderForm();
            orderForm.ShowDialog();
        }

        private void TestSendOrderOpenOrderMenuItemClick(object sender, EventArgs e)
        {
            SendOrderForm sendOrderForm = new SendOrderForm();
            // SendOrderForm sendOrderForm = BotServiceProvider.GetRequiredService<SendOrderForm>();
            sendOrderForm.ShowDialog();
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

        private void btn_sort_result_Click(object sender, EventArgs e)
        {
            List<string> myList = new List<string>();
            var arr = tb_logs.Text.Trim().Split(Environment.NewLine);
            foreach (var s in arr)
                myList.Add(s);
            myList = myList.OrderBy(p => p.Substring(5, p.IndexOf("]"))).ToList();
            tb_logs.Text = "";
            foreach (var l in myList)
                tb_logs.Text += l + Environment.NewLine;
        }
    }
}
