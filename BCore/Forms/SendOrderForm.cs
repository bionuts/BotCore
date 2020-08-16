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
        private string token;

        public SendOrderForm()
        {
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
                    var order = new BOrder
                    {
                        SymboleName = cb_symboles.SelectedText,
                        SymboleCode = cb_symboles.SelectedValue.ToString(),
                        OrderType = ( button.Name == "BUY" ? "BUY" : "SELL"),
                        Count = int.Parse(tb_count.Text.Trim()),
                        Price = decimal.Parse(tb_price.Text.Trim()),
                        CreatedDateTime = DateTime.Now,
                        Status = "",
                        OrderId = "0"
                    };
                    MobinBroker m1 = new MobinBroker(token, order);
                    await m1.SendOrder();
                    tb_response.Text += $"ElapsedTime: {m1.SendingOrderElapsedTime}, Desc: {m1.SendingOrderMessageDesc}{Environment.NewLine}";
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
            MobinBroker mobin = new MobinBroker();
            mobin.Token = token;
            GetOpenOrder openOrder = await mobin.GetOpenOrders();
            tb_response.Text = mobin.SendingOrderMessageDesc + Environment.NewLine;
            foreach (var o in openOrder.Data)
            {
                tb_response.Text += o.symbol + Environment.NewLine;
            }
        }
    }
}
