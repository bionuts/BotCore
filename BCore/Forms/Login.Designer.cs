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
            this.pb_captcha = new System.Windows.Forms.PictureBox();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_init_cookies = new System.Windows.Forms.Button();
            this.tb_captcha = new System.Windows.Forms.TextBox();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.lv_accounts = new System.Windows.Forms.ListView();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader7 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            ((System.ComponentModel.ISupportInitialize)(this.pb_captcha)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(310, 449);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(216, 64);
            this.btn_login.TabIndex = 2;
            this.btn_login.Text = "login";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // pb_captcha
            // 
            this.pb_captcha.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pb_captcha.Location = new System.Drawing.Point(12, 449);
            this.pb_captcha.Name = "pb_captcha";
            this.pb_captcha.Size = new System.Drawing.Size(212, 64);
            this.pb_captcha.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_captcha.TabIndex = 3;
            this.pb_captcha.TabStop = false;
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(230, 449);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(74, 64);
            this.btn_refresh.TabIndex = 2;
            this.btn_refresh.Text = "R";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_init_cookies
            // 
            this.btn_init_cookies.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_init_cookies.Location = new System.Drawing.Point(656, 449);
            this.btn_init_cookies.Name = "btn_init_cookies";
            this.btn_init_cookies.Size = new System.Drawing.Size(216, 64);
            this.btn_init_cookies.TabIndex = 5;
            this.btn_init_cookies.Text = "Init Cookies";
            this.btn_init_cookies.UseVisualStyleBackColor = true;
            this.btn_init_cookies.Click += new System.EventHandler(this.btn_init_cookies_Click);
            // 
            // tb_captcha
            // 
            this.tb_captcha.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tb_captcha.Location = new System.Drawing.Point(12, 519);
            this.tb_captcha.Name = "tb_captcha";
            this.tb_captcha.Size = new System.Drawing.Size(212, 41);
            this.tb_captcha.TabIndex = 1;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Username";
            this.columnHeader3.Width = 90;
            // 
            // lv_accounts
            // 
            this.lv_accounts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader6,
            this.columnHeader4,
            this.columnHeader7,
            this.columnHeader5});
            this.lv_accounts.FullRowSelect = true;
            this.lv_accounts.GridLines = true;
            this.lv_accounts.HideSelection = false;
            this.lv_accounts.Location = new System.Drawing.Point(12, 12);
            this.lv_accounts.Name = "lv_accounts";
            this.lv_accounts.Size = new System.Drawing.Size(860, 431);
            this.lv_accounts.TabIndex = 5;
            this.lv_accounts.UseCompatibleStateImageBehavior = false;
            this.lv_accounts.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Password";
            this.columnHeader6.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "BCode";
            this.columnHeader4.Width = 70;
            // 
            // columnHeader7
            // 
            this.columnHeader7.Text = "Token Date";
            this.columnHeader7.Width = 160;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Token";
            this.columnHeader5.Width = 400;
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(884, 572);
            this.Controls.Add(this.lv_accounts);
            this.Controls.Add(this.tb_captcha);
            this.Controls.Add(this.btn_init_cookies);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.pb_captcha);
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
        private System.Windows.Forms.PictureBox pb_captcha;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btn_init_cookies;
        private System.Windows.Forms.TextBox tb_captcha;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ListView lv_accounts;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ColumnHeader columnHeader7;
    }
}