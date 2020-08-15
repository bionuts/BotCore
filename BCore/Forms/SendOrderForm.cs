using BCore.Data;
using BCore.DataModel;
using BCore.Lib;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
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
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCore.Forms
{
    public partial class SendOrderForm : Form
    {
        private readonly ApplicationDbContext db;
        private MobinBroker mobin;

        public SendOrderForm()
        {
            mobin = new MobinBroker();
            db = new ApplicationDbContext();
            InitializeComponent();
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
                    mobin = new MobinBroker
                    {
                        Token = token.Value,
                        Order = order
                    };
                    await mobin.SendOrder();
                    tb_response.Text = $"ElapsedTime: {mobin.SendingOrderElapsedTime} {mobin.SendingOrderMessageDesc}";
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
            /*var a = new MobinBroker(http);
            a.Token = token.Value;
            GetOpenOrder openOrder = await a.GetOpenOrders();
            tb_response.Text = await GetOpenOrderTask(token.Value);*/
        }
    }
}
