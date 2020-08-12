using BCore.Data;
using BCore.DataModel;
using BCore.Lib;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCore.Forms
{
    public partial class SendOrderForm : Form
    {
        private readonly ApplicationDbContext db;
        private Utilities util;

        public SendOrderForm()
        {
            InitializeComponent();
            db = new ApplicationDbContext();
            util = new Utilities(db);
        }

        private async void SendOrderForm_Load(object sender, EventArgs e)
        {
            await ReloadSymboles();
        }

        private async void btn_send_order_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (tb_count.Text.Trim() != "" && tb_price.Text.Trim() != "")
            {
                try
                {
                    var order = new BOrder
                    {
                        SymboleName = cb_symboles.GetItemText(cb_symboles.SelectedItem),
                        SymboleCode = cb_symboles.SelectedValue.ToString(),
                        OrderType = (button.Name == "btn_buy" ? "BUY" : "SELL"),
                        Count = int.Parse(tb_count.Text.Trim()),
                        Price = int.Parse(tb_price.Text.Trim()),
                        CreatedDateTime = DateTime.Now,
                        Status = "",
                        OrderId = "0"
                    };
                    var token = await db.BSettings.Where(s => s.Key == "apitoken").FirstOrDefaultAsync();
                    tb_response.Text = await SendOrderTask(order, token.Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void tb_count_price_TextChanged(object sender, EventArgs e)
        {
            if (tb_count.Text != "" && tb_price.Text != "")
                lbl_total.Text = $"Total: {(int.Parse(tb_count.Text.Trim()) * int.Parse(tb_price.Text.Trim())).ToString("N0")}";
        }

        private async Task<string> SendOrderTask(BOrder order, string ApiToken)
        {
            HttpClient _http = Utilities.GetPresetHttpClientForSendOrders();
            string output = "";
            var req = Utilities.InitOrderReqHeader(order, ApiToken);

            Stopwatch stopwatch = Stopwatch.StartNew();
            HttpResponseMessage httpResponse = await _http.SendAsync(req);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                stopwatch.Stop();
                output += $"Elapsed-Time: {stopwatch.ElapsedMilliseconds} ms" + Environment.NewLine;

                var resContent = await httpResponse.Content.ReadAsStringAsync();
                OrderRespond orderRespond = JsonConvert.DeserializeObject<OrderRespond>(resContent);
                if (orderRespond.IsSuccessfull)
                {
                    output += $"Return Data => {resContent}";
                }
            }
            return output;
        }

        private async Task<string> GetOpenOrderTask(string ApiToken)
        {
            HttpClient _http = Utilities.GetPresetHttpClientForOpenOrders();
            string output = "";
            var req = Utilities.InitGetOpenOrdersReqHeader(ApiToken);

            Stopwatch stopwatch = Stopwatch.StartNew();
            HttpResponseMessage httpResponse = await _http.SendAsync(req);
            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                stopwatch.Stop();
                output += $"Elapsed-Time: {stopwatch.ElapsedMilliseconds} ms" + Environment.NewLine;

                string res = await httpResponse.Content.ReadAsStringAsync();
                GetOpenOrder getOpenOrders = JsonConvert.DeserializeObject<GetOpenOrder>(res);
                if (getOpenOrders.IsSuccessfull && getOpenOrders.Data.Length > 0)
                {
                    foreach (var o in getOpenOrders.Data)
                    {
                        output += $"ORDER => {o.symbol} ,TYPE: {o.orderside} ,CountPrice: [{o.qunatity}/{o.orderprice}] ," + 
                            $"OrderId: {o.orderid}, DateTime: {o.dtime}/{o.time} ,ExpectedQuantity/Executed: {o.ExpectedQuantity}/{o.excuted} ," + 
                            $"Status: {o.status}" + Environment.NewLine;
                    }
                }
            }
            return output;
        }

        private async Task ReloadSymboles()
        {
            var symboles = await db.BSymboles.ToListAsync();
            if (symboles.Count > 0)
            {
                cb_symboles.DataSource = symboles;
            }
        }

        private async void btn_get_open_orders_Click(object sender, EventArgs e)
        {
            var token = await db.BSettings.Where(s => s.Key == "apitoken").FirstOrDefaultAsync();
            tb_response.Text = await GetOpenOrderTask(token.Value);
        }
    }
}
