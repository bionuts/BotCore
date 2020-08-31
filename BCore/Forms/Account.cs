using BCore.Data;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    public partial class Account : Form
    {
        public Account()
        {
            InitializeComponent();
        }

        private async void Account_Load(object sender, EventArgs e)
        {
            await ReLoadListView();
        }

        private async void btn_save_Click(object sender, EventArgs e)
        {
            if (tb_name.Text.Trim() != "" && tb_username.Text.Trim() != "" && tb_password.Text.Trim() != "")
            {
                using (var db = new ApplicationDbContext())
                {
                    BAccount acc = new BAccount
                    {
                        Name = tb_name.Text.Trim(),
                        Username = tb_username.Text.Trim(),
                        Password = tb_password.Text.Trim()
                    };
                    db.BAccounts.Add(acc);
                    await db.SaveChangesAsync();
                    await ReLoadListView();
                }
            }
        }

        private async Task ReLoadListView()
        {
            using (var dbt = new ApplicationDbContext())
            {
                lv_accounts.Items.Clear();
                var accounts = await dbt.BAccounts.ToListAsync();
                if (accounts.Count > 0)
                {
                    foreach (var acc in accounts)
                    {
                        var row = new string[]
                        {
                        acc.Id.ToString(),
                        acc.Name,
                        acc.Username,
                        acc.Password
                        };
                        var lvItem = new ListViewItem(row);
                        lv_accounts.Items.Add(lvItem);
                    }
                }
            }
        }

        private async void btn_delete_Click(object sender, EventArgs e)
        {
            using (var db = new ApplicationDbContext())
            {
                if (lv_accounts.SelectedItems.Count > 0)
                {
                    foreach (ListViewItem sitem in lv_accounts.SelectedItems)
                    {
                        var id = int.Parse(sitem.SubItems[0].Text);
                        var item = await db.BAccounts.FindAsync(id);
                        if (item != null)
                        {
                            db.BAccounts.Remove(item);
                        }
                    }
                    await db.SaveChangesAsync();
                    await ReLoadListView();
                }
            }
        }
    }
}
