using BCore.Data;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCore.Forms
{
    public partial class OrderForm : Form
    {
        public OrderForm()
        {
            InitializeComponent();
        }

        private async void OrderForm_Load(object sender, EventArgs e)
        {
            await ReloadSymboles();
            await ReLoadListView();
        }

        private async Task ReloadSymboles()
        {
            using (var db = new ApplicationDbContext())
            {

                var symboles = await db.BSymboles.ToListAsync();
                if (symboles.Count > 0)
                {
                    cb_symboles.DataSource = symboles;
                }
            }
        }

        private async void btn_send_order_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (tb_count.Text.Trim() != "" && tb_price.Text.Trim() != "" && lv_accounts.SelectedItems.Count > 0)
            {
                try
                {
                    using (var db = new ApplicationDbContext())
                    {
                        BOrder order = new BOrder
                        {
                            SymboleName = cb_symboles.GetItemText(cb_symboles.SelectedItem),
                            SymboleCode = cb_symboles.SelectedValue.ToString(),
                            OrderType = (button.Name == "btn_buy" ? "BUY" : "SELL"),
                            Count = int.Parse(tb_count.Text.Trim()),
                            Price = decimal.Parse(tb_price.Text.Trim()),
                            CreatedDateTime = DateTime.Now,
                            OrderAccounts = new List<BOrderAccounts>()
                        };

                        foreach (ListViewItem sitem in lv_accounts.SelectedItems)
                        {
                            var userid = int.Parse(sitem.SubItems[0].Text);
                            BOrderAccounts tmp = new BOrderAccounts 
                            { 
                                AccountId = userid
                            };
                            order.OrderAccounts.Add(tmp);
                        }
                        db.BOrders.Add(order);

                        if (await db.SaveChangesAsync() > 0)
                        {
                            tb_count.Text = "";
                            tb_price.Text = "";
                            lbl_total.Text = "0";

                            var mainForm = Application.OpenForms.OfType<MainBotForm>().FirstOrDefault();
                            await mainForm.LoadOrdersToListView();
                        }
                    }
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
            {
                var tot = long.Parse(tb_count.Text.Trim()) * long.Parse(tb_price.Text.Trim());
                lbl_total.Text = $"{tot:N0}";
            }
        }

        private async Task ReLoadListView()
        {
            using (var dbt = new ApplicationDbContext())
            {
                lv_accounts.Items.Clear();
                var accounts = await dbt.BAccounts.OrderByDescending(d => d.TokenDate).ToListAsync();
                if (accounts.Count > 0)
                {
                    foreach (var acc in accounts)
                    {
                        var row = new string[]
                        {
                        acc.AccountId.ToString(),
                        acc.Name,
                        acc.Username
                        };
                        var lvItem = new ListViewItem(row);
                        lv_accounts.Items.Add(lvItem);
                    }
                }
            }
        }
    }
}
