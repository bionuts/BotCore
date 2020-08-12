using BCore.Data;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCore.Forms
{
    public partial class OrderForm : Form
    {
        private readonly ApplicationDbContext db;

        public OrderForm()
        {
            InitializeComponent();
            db = new ApplicationDbContext();
        }

        private async void OrderForm_Load(object sender, EventArgs e)
        {
            await ReloadSymboles();
        }

        private async Task ReloadSymboles()
        {
            var symboles = await db.BSymboles.ToListAsync();
            if (symboles.Count > 0)
            {
                cb_symboles.DataSource = symboles;
            }
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
                    db.BOrders.Add(order);
                    if (await db.SaveChangesAsync() > 0)
                    {
                        tb_count.Text = "";
                        tb_price.Text = "";
                        lbl_total.Text = "Total: 0";
                    }
                    var mainForm = Application.OpenForms.OfType<MainBotForm>().FirstOrDefault();
                    await mainForm.LoadOrdersToListView();
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
    }
}
