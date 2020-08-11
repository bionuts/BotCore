namespace BCore.Forms
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_login = new System.Windows.Forms.Button();
            this.tb_password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_username = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pb_captcha = new System.Windows.Forms.PictureBox();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.lbl_bourse_code = new System.Windows.Forms.Label();
            this.lbl_api_token = new System.Windows.Forms.Label();
            this.btn_init_cookies = new System.Windows.Forms.Button();
            this.tb_captcha = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pb_captcha)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(262, 149);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(216, 46);
            this.btn_login.TabIndex = 2;
            this.btn_login.Text = "login";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // tb_password
            // 
            this.tb_password.Location = new System.Drawing.Point(264, 43);
            this.tb_password.Name = "tb_password";
            this.tb_password.PasswordChar = '*';
            this.tb_password.ReadOnly = true;
            this.tb_password.Size = new System.Drawing.Size(214, 27);
            this.tb_password.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(264, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Password:";
            // 
            // tb_username
            // 
            this.tb_username.Location = new System.Drawing.Point(44, 43);
            this.tb_username.Name = "tb_username";
            this.tb_username.ReadOnly = true;
            this.tb_username.Size = new System.Drawing.Size(214, 27);
            this.tb_username.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Username:";
            // 
            // pb_captcha
            // 
            this.pb_captcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_captcha.Location = new System.Drawing.Point(44, 98);
            this.pb_captcha.Name = "pb_captcha";
            this.pb_captcha.Size = new System.Drawing.Size(212, 64);
            this.pb_captcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_captcha.TabIndex = 3;
            this.pb_captcha.TabStop = false;
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(217, 168);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(39, 28);
            this.btn_refresh.TabIndex = 2;
            this.btn_refresh.Text = "R";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // lbl_bourse_code
            // 
            this.lbl_bourse_code.AutoSize = true;
            this.lbl_bourse_code.Location = new System.Drawing.Point(44, 210);
            this.lbl_bourse_code.Name = "lbl_bourse_code";
            this.lbl_bourse_code.Size = new System.Drawing.Size(92, 20);
            this.lbl_bourse_code.TabIndex = 4;
            this.lbl_bourse_code.Text = "BourseCode:";
            // 
            // lbl_api_token
            // 
            this.lbl_api_token.AutoSize = true;
            this.lbl_api_token.Location = new System.Drawing.Point(44, 230);
            this.lbl_api_token.Name = "lbl_api_token";
            this.lbl_api_token.Size = new System.Drawing.Size(74, 20);
            this.lbl_api_token.TabIndex = 4;
            this.lbl_api_token.Text = "ApiToken:";
            // 
            // btn_init_cookies
            // 
            this.btn_init_cookies.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_init_cookies.Location = new System.Drawing.Point(262, 98);
            this.btn_init_cookies.Name = "btn_init_cookies";
            this.btn_init_cookies.Size = new System.Drawing.Size(216, 46);
            this.btn_init_cookies.TabIndex = 5;
            this.btn_init_cookies.Text = "Init Cookies";
            this.btn_init_cookies.UseVisualStyleBackColor = true;
            this.btn_init_cookies.Click += new System.EventHandler(this.btn_init_cookies_Click);
            // 
            // tb_captcha
            // 
            this.tb_captcha.Location = new System.Drawing.Point(44, 168);
            this.tb_captcha.Name = "tb_captcha";
            this.tb_captcha.Size = new System.Drawing.Size(167, 27);
            this.tb_captcha.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(44, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 20);
            this.label6.TabIndex = 0;
            this.label6.Text = "Capcha:";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(523, 270);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tb_captcha);
            this.Controls.Add(this.btn_init_cookies);
            this.Controls.Add(this.lbl_api_token);
            this.Controls.Add(this.lbl_bourse_code);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.pb_captcha);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_username);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_password);
            this.Controls.Add(this.btn_login);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Login";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_captcha)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.TextBox tb_password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_username;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pb_captcha;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Label lbl_bourse_code;
        private System.Windows.Forms.Label lbl_api_token;
        private System.Windows.Forms.Button btn_init_cookies;
        private System.Windows.Forms.TextBox tb_captcha;
        private System.Windows.Forms.Label label6;
    }
}