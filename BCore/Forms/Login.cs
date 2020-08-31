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
        private readonly ApplicationDbContext db;
        private MobinBroker mobin;

        public Login()
        {
            db = new ApplicationDbContext();
            mobin = new MobinBroker();
            InitializeComponent();
        }

        private async void Login_Load(object sender, EventArgs e)
        {
            await ReLoadListView();
            if (await mobin.InitCookies())
                pb_captcha.Image = await mobin.GetCaptcha();
            else
                MessageBox.Show("init failed.Try again");
        }

        private async void btn_init_cookies_Click(object sender, EventArgs e)
        {
            if (await mobin.InitCookies())
                MessageBox.Show("init done.");
            else
                MessageBox.Show("init failed. try again");
        }

        private async void btn_refresh_Click(object sender, EventArgs e)
        {
            pb_captcha.Image = await mobin.GetCaptcha();
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
                        foreach (ListViewItem sitem in lv_accounts.SelectedItems)
                        {
                            var id = int.Parse(sitem.SubItems[0].Text);
                            var username = sitem.SubItems[2].Text;
                            var password = sitem.SubItems[3].Text;
                            string output = await mobin.Login(username, password, tb_captcha.Text.Trim());
                            if (output != "")
                            {
                                var acc = await db.BAccounts.FindAsync(id);
                                acc.BCode = output.Substring(output.IndexOf("@@@") + 3);
                                acc.Token = output.Substring(0, output.IndexOf("@@@"));
                                acc.TokenDate = DateTime.Now;
                                db.Update(acc);
                            }

                        }
                        await db.SaveChangesAsync();
                        await ReLoadListView();
                    }
                }
            }
            btn_login.Text = "Login";
            btn_login.Enabled = Enabled;
            /*string output = await mobin.Login(tb_username.Text, tb_password.Text, tb_captcha.Text.Trim());
            if (output != "")
            {
                lbl_api_token.Text = "ApiToken: " + output.Substring(0, output.IndexOf("@@@"));
                lbl_bourse_code.Text = "BourseCode: " + output.Substring(output.IndexOf("@@@") + 3);
                var mainForm = Application.OpenForms.OfType<MainBotForm>().FirstOrDefault();
                mainForm.UpdateToken(output.Substring(0, output.IndexOf("@@@")));
            }*/
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
                        acc.Id.ToString(),
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
    }
}
