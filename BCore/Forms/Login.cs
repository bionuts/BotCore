using BCore.Data;
using BCore.Lib;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
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
            tb_username.Text = (await db.BSettings.Where(s => s.Key == "username").FirstOrDefaultAsync()).Value;
            tb_password.Text = (await db.BSettings.Where(s => s.Key == "password").FirstOrDefaultAsync()).Value;
        }

        private async void btn_init_cookies_Click(object sender, EventArgs e)
        {
            if (await mobin.InitCookies())
                MessageBox.Show("init done.");
            else
                MessageBox.Show("init failed.");
        }

        private async void btn_refresh_Click(object sender, EventArgs e)
        {
            pb_captcha.Image = await mobin.GetCaptcha();
        }

        private async void btn_login_Click(object sender, EventArgs e)
        {
            string output = await mobin.Login(tb_username.Text, tb_password.Text, tb_captcha.Text.Trim());
            if (output != "")
            {
                lbl_api_token.Text = "ApiToken: " + output.Substring(0, output.IndexOf("@@@"));
                lbl_bourse_code.Text = "BourseCode: " + output.Substring(output.IndexOf("@@@") + 3);
                var mainForm = Application.OpenForms.OfType<MainBotForm>().FirstOrDefault();
                mainForm.UpdateToken(output.Substring(0, output.IndexOf("@@@")));
            }
        }
    }
}
