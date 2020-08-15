using BCore.Data;
using BCore.DataModel;
using BCore.Lib;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCore.Forms
{
    public partial class SendOrderForm : Form
    {
        private readonly ApplicationDbContext db;
        private MobinBroker mobin;
        private string token;

        public SendOrderForm()
        {
            mobin = new MobinBroker();
            db = new ApplicationDbContext();
            InitializeComponent();
        }

        private async void SendOrderForm_Load(object sender, EventArgs e)
        {
            await ReloadSymboles();
            token = (await db.BSettings.Where(s => s.Key == "apitoken").FirstOrDefaultAsync()).Value;
        }

        private async void btn_send_order_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (tb_count.Text.Trim() != "" && tb_price.Text.Trim() != "")
            {
                try
                {
                    mobin.Token = token;
                    var order1 = new BOrder
                    {
                        SymboleName = "ABADA",
                        SymboleCode = "IRO1NBAB0001",
                        OrderType = "BUY",
                        Count = int.Parse(tb_count.Text.Trim()),
                        Price = int.Parse(tb_price.Text.Trim()),
                        CreatedDateTime = DateTime.Now,
                        Status = "",
                        OrderId = "0"
                    };
                    mobin.Order = order1;
                    mobin.PresetSendingOrderReqMsg();
                    //var str = DateTime.Now.Millisecond.ToString("D3");
                    mobin.SendOrder();
                    //tb_response.Text += $"S_{str}, ElapsedTime: {mobin.SendingOrderElapsedTime}, Desc: {mobin.SendingOrderMessageDesc}{Environment.NewLine}";
                    await Task.Delay(250);
                    var order2 = new BOrder
                    {
                        SymboleName = "WebSA",
                        SymboleCode = "IRO1BSDR0001",
                        OrderType = "BUY",
                        Count = int.Parse(tb_count.Text.Trim()),
                        Price = int.Parse(tb_price.Text.Trim()),
                        CreatedDateTime = DateTime.Now,
                        Status = "",
                        OrderId = "0"
                    };
                    mobin.Order = order2;
                    mobin.PresetSendingOrderReqMsg();
                    //str = DateTime.Now.Millisecond.ToString("D3");
                    mobin.SendOrder();
                    //tb_response.Text += $"S_{str}, ElapsedTime: {mobin.SendingOrderElapsedTime}, Desc: {mobin.SendingOrderMessageDesc}{Environment.NewLine}";
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
            mobin.Token = token.Value;
            GetOpenOrder openOrder = await mobin.GetOpenOrders();
            tb_response.Text = mobin.SendingOrderMessageDesc + Environment.NewLine;
            foreach (var o in openOrder.Data)
            {
                tb_response.Text += o.symbol + Environment.NewLine;
            }
        }
    }
}
