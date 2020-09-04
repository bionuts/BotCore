using BCore.Data;
using BCore.Lib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCore.Forms
{
    public partial class Login : Form
    {
        private MobinBroker mobin;

        public Login()
        {
            mobin = new MobinBroker();
            InitializeComponent();
        }

        private async void Login_Load(object sender, EventArgs e)
        {
            try
            {
                await ReLoadListView();
                if (await mobin.InitCookies())
                    pb_captcha.Image = await mobin.GetCaptcha();
                else
                    MessageBox.Show("init failed.Try again");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Login_Load(): " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_init_cookies_Click(object sender, EventArgs e)
        {
            try
            {
                if (await mobin.InitCookies())
                    MessageBox.Show("init done.");
                else
                    MessageBox.Show("init failed. try again");
            }
            catch (Exception ex)
            {
                MessageBox.Show("btn_init_cookies_Click(): " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_refresh_Click(object sender, EventArgs e)
        {
            tb_captcha.Text = "";
            try
            {
                pb_captcha.Image = await mobin.GetCaptcha();
            }
            catch (Exception ex)
            {
                MessageBox.Show("btn_refresh_Click(): " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_login_Click(object sender, EventArgs e)
        {
            btn_login.Text = "Wait...";
            btn_login.Enabled = false;
            if (tb_captcha.Text.Trim() != "")
            {
                using (var db = new ApplicationDbContext())
                {
                    if (lv_accounts.SelectedItems.Count > 0)
                    {
                        var id = int.Parse(lv_accounts.SelectedItems[0].SubItems[0].Text);
                        var username = lv_accounts.SelectedItems[0].SubItems[2].Text;
                        var password = lv_accounts.SelectedItems[0].SubItems[3].Text;
                        string output = await mobin.Login(username, password, tb_captcha.Text.Trim());
                        if (output != "")
                        {
                            var acc = await db.BAccounts.FindAsync(id);
                            acc.BCode = output.Substring(output.IndexOf("@@@") + 3);
                            acc.Token = output.Substring(0, output.IndexOf("@@@"));
                            acc.TokenDate = DateTime.Now;
                            db.Update(acc);
                        }
                        await db.SaveChangesAsync();
                        await ReLoadListView();
                    }
                }
            }
            btn_login.Text = "Login";
            btn_login.Enabled = Enabled;
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
                        acc.Username,
                        acc.Password,
                        acc.BCode,
                        acc.TokenDate.ToString(),
                        acc.Token
                        };
                        var lvItem = new ListViewItem(row);
                        lv_accounts.Items.Add(lvItem);
                    }
                }
            }
        }

        private async void btn_master_Click(object sender, EventArgs e)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    if (lv_accounts.SelectedItems.Count == 1)
                    {
                        var username = lv_accounts.SelectedItems[0].SubItems[2].Text;
                        var password = lv_accounts.SelectedItems[0].SubItems[3].Text;
                        var token = lv_accounts.SelectedItems[0].SubItems[6].Text;

                        var u = await db.BSettings.Where(s => s.Key == "username").FirstOrDefaultAsync();
                        u.Value = username.Trim();
                        db.BSettings.Update(u);

                        var p = await db.BSettings.Where(s => s.Key == "password").FirstOrDefaultAsync();
                        p.Value = password.Trim();
                        db.BSettings.Update(p);

                        var t = await db.BSettings.Where(s => s.Key == "apitoken").FirstOrDefaultAsync();
                        t.Value = token.Trim();
                        db.BSettings.Update(t);

                        if (await db.SaveChangesAsync() > 0)
                            MessageBox.Show("Master User Selected.", "Master User", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else
                            MessageBox.Show("Error");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("btn_master_Click(): " + ex.Message, "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
