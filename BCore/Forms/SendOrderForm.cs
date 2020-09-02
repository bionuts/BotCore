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
        private readonly MobinBroker mobinBroker;
        private string token;

        public SendOrderForm()
        {
            mobinBroker = new MobinBroker();
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
                        OrderType = (button.Name == "BUY" ? "BUY" : "SELL"),
                        Count = int.Parse(tb_count.Text.Trim()),
                        Price = decimal.Parse(tb_price.Text.Trim()),
                        CreatedDateTime = DateTime.Now,
                        Status = "",
                        OrderId = "0"
                    };
                    tb_response.AppendText((await mobinBroker.SendOrder(order)).Replace("\n", Environment.NewLine));
                }
                catch (Exception ex)
                {
                    tb_response.AppendText("Error: " + ex.Message);
                }
            }
        }

        private void tb_count_price_TextChanged(object sender, EventArgs e)
        {
            if (tb_count.Text != "" && tb_price.Text != "")
            {
                var tot = long.Parse(tb_count.Text.Trim()) * long.Parse(tb_price.Text.Trim());
                lbl_total.Text = $"{tot:N0}";
            }
        }

        private async Task ReloadSymboles()
        {
            using (var db = new ApplicationDbContext())
            {
                var testform = Application.OpenForms.OfType<SendOrderForm>().FirstOrDefault();
                testform.Text = "Testing By Master User => " + (await db.BSettings.Where(s => s.Key == "username").FirstOrDefaultAsync()).Value;
                token = (await db.BSettings.Where(s => s.Key == "apitoken").FirstOrDefaultAsync()).Value;
                mobinBroker.Token = token;
                var symboles = await db.BSymboles.ToListAsync();
                if (symboles.Count > 0)
                {
                    cb_symboles.DataSource = symboles;
                }
            }
        }

        private async void btn_get_open_orders_Click(object sender, EventArgs e)
        {
            tb_response.AppendText((await mobinBroker.GetOpenOrders()).Replace("\n", Environment.NewLine));
        }
    }
}
