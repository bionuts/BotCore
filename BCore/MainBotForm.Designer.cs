namespace BCore
{
    partial class MainBotForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainBotForm));
            this.btn_start = new System.Windows.Forms.Button();
            this.lv_orders = new System.Windows.Forms.ListView();
            this.orderId = new System.Windows.Forms.ColumnHeader();
            this.symName = new System.Windows.Forms.ColumnHeader();
            this.count = new System.Windows.Forms.ColumnHeader();
            this.price = new System.Windows.Forms.ColumnHeader();
            this.total = new System.Windows.Forms.ColumnHeader();
            this.orderType = new System.Windows.Forms.ColumnHeader();
            this.times = new System.Windows.Forms.ColumnHeader();
            this.hit = new System.Windows.Forms.ColumnHeader();
            this.tb_logs = new System.Windows.Forms.TextBox();
            this.btn_load = new System.Windows.Forms.Button();
            this.tb_duration = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_interval = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_starttime = new System.Windows.Forms.Label();
            this.tb_hh = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tb_mm = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_ss = new System.Windows.Forms.TextBox();
            this.tb_ms = new System.Windows.Forms.TextBox();
            this.lbl_endTime = new System.Windows.Forms.Label();
            this.btn_delete_orders = new System.Windows.Forms.Button();
            this.MainConsoleMenu = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.lbl_ws_time = new System.Windows.Forms.Label();
            this.timer_cando = new System.Windows.Forms.Timer(this.components);
            this.lbl_done = new System.Windows.Forms.Label();
            this.timer_real_time = new System.Windows.Forms.Timer(this.components);
            this.lbl_pc_time = new System.Windows.Forms.Label();
            this.tb_ws_logs = new System.Windows.Forms.TextBox();
            this.timer_stay_tune = new System.Windows.Forms.Timer(this.components);
            this.lbl_login = new System.Windows.Forms.Label();
            this.lbl_option_time = new System.Windows.Forms.Label();
            this.lbl_openorders_time = new System.Windows.Forms.Label();
            this.timer_option = new System.Windows.Forms.Timer(this.components);
            this.timer_login = new System.Windows.Forms.Timer(this.components);
            this.MainConsoleMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(101, 228);
            this.btn_start.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(85, 22);
            this.btn_start.TabIndex = 0;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // lv_orders
            // 
            this.lv_orders.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.lv_orders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lv_orders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.orderId,
            this.symName,
            this.count,
            this.price,
            this.total,
            this.orderType,
            this.times,
            this.hit});
            this.lv_orders.FullRowSelect = true;
            this.lv_orders.GridLines = true;
            this.lv_orders.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_orders.HideSelection = false;
            this.lv_orders.Location = new System.Drawing.Point(10, 61);
            this.lv_orders.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lv_orders.Name = "lv_orders";
            this.lv_orders.Size = new System.Drawing.Size(659, 163);
            this.lv_orders.TabIndex = 2;
            this.lv_orders.UseCompatibleStateImageBehavior = false;
            this.lv_orders.View = System.Windows.Forms.View.Details;
            // 
            // orderId
            // 
            this.orderId.Name = "orderId";
            this.orderId.Text = "ID";
            this.orderId.Width = 65;
            // 
            // symName
            // 
            this.symName.Name = "symName";
            this.symName.Text = "Symbole";
            this.symName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.symName.Width = 110;
            // 
            // count
            // 
            this.count.Name = "count";
            this.count.Text = "Count";
            this.count.Width = 70;
            // 
            // price
            // 
            this.price.Name = "price";
            this.price.Text = "Price";
            this.price.Width = 95;
            // 
            // total
            // 
            this.total.Name = "total";
            this.total.Text = "Total";
            this.total.Width = 130;
            // 
            // orderType
            // 
            this.orderType.Name = "orderType";
            this.orderType.Text = "Type";
            this.orderType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // times
            // 
            this.times.Name = "times";
            this.times.Text = "Times";
            this.times.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // hit
            // 
            this.hit.Name = "hit";
            this.hit.Text = "Hits";
            this.hit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_logs
            // 
            this.tb_logs.AcceptsReturn = true;
            this.tb_logs.Location = new System.Drawing.Point(10, 254);
            this.tb_logs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_logs.Multiline = true;
            this.tb_logs.Name = "tb_logs";
            this.tb_logs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_logs.Size = new System.Drawing.Size(659, 234);
            this.tb_logs.TabIndex = 0;
            this.tb_logs.WordWrap = false;
            // 
            // btn_load
            // 
            this.btn_load.Location = new System.Drawing.Point(10, 228);
            this.btn_load.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(85, 22);
            this.btn_load.TabIndex = 4;
            this.btn_load.Text = "Load";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.btn_load_Click);
            // 
            // tb_duration
            // 
            this.tb_duration.Location = new System.Drawing.Point(83, 34);
            this.tb_duration.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_duration.MaxLength = 4;
            this.tb_duration.Name = "tb_duration";
            this.tb_duration.Size = new System.Drawing.Size(42, 23);
            this.tb_duration.TabIndex = 1;
            this.tb_duration.Text = "3";
            this.tb_duration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Duration(s):";
            // 
            // tb_interval
            // 
            this.tb_interval.Location = new System.Drawing.Point(204, 34);
            this.tb_interval.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_interval.MaxLength = 3;
            this.tb_interval.Name = "tb_interval";
            this.tb_interval.Size = new System.Drawing.Size(42, 23);
            this.tb_interval.TabIndex = 2;
            this.tb_interval.Text = "300";
            this.tb_interval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 15);
            this.label2.TabIndex = 5;
            this.label2.Text = "Interval(ms):";
            // 
            // lbl_starttime
            // 
            this.lbl_starttime.AutoSize = true;
            this.lbl_starttime.Location = new System.Drawing.Point(250, 37);
            this.lbl_starttime.Name = "lbl_starttime";
            this.lbl_starttime.Size = new System.Drawing.Size(63, 15);
            this.lbl_starttime.TabIndex = 5;
            this.lbl_starttime.Text = "Start Time:";
            this.lbl_starttime.Click += new System.EventHandler(this.lbl_starttime_Click);
            // 
            // tb_hh
            // 
            this.tb_hh.BackColor = System.Drawing.Color.LightYellow;
            this.tb_hh.Location = new System.Drawing.Point(317, 34);
            this.tb_hh.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_hh.MaxLength = 2;
            this.tb_hh.Name = "tb_hh";
            this.tb_hh.Size = new System.Drawing.Size(32, 23);
            this.tb_hh.TabIndex = 3;
            this.tb_hh.Text = "08";
            this.tb_hh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(349, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = ":";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(391, 37);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 15);
            this.label5.TabIndex = 7;
            this.label5.Text = ":";
            // 
            // tb_mm
            // 
            this.tb_mm.BackColor = System.Drawing.Color.LightYellow;
            this.tb_mm.Location = new System.Drawing.Point(360, 34);
            this.tb_mm.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_mm.MaxLength = 2;
            this.tb_mm.Name = "tb_mm";
            this.tb_mm.Size = new System.Drawing.Size(32, 23);
            this.tb_mm.TabIndex = 4;
            this.tb_mm.Text = "29";
            this.tb_mm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(433, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 15);
            this.label6.TabIndex = 7;
            this.label6.Text = ":";
            // 
            // tb_ss
            // 
            this.tb_ss.BackColor = System.Drawing.Color.LightYellow;
            this.tb_ss.Location = new System.Drawing.Point(402, 34);
            this.tb_ss.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_ss.MaxLength = 2;
            this.tb_ss.Name = "tb_ss";
            this.tb_ss.Size = new System.Drawing.Size(32, 23);
            this.tb_ss.TabIndex = 5;
            this.tb_ss.Text = "57";
            this.tb_ss.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_ms
            // 
            this.tb_ms.BackColor = System.Drawing.Color.LightYellow;
            this.tb_ms.Location = new System.Drawing.Point(446, 34);
            this.tb_ms.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_ms.MaxLength = 3;
            this.tb_ms.Name = "tb_ms";
            this.tb_ms.Size = new System.Drawing.Size(37, 23);
            this.tb_ms.TabIndex = 6;
            this.tb_ms.Text = "000";
            this.tb_ms.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbl_endTime
            // 
            this.lbl_endTime.AutoSize = true;
            this.lbl_endTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_endTime.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbl_endTime.Location = new System.Drawing.Point(489, 37);
            this.lbl_endTime.Name = "lbl_endTime";
            this.lbl_endTime.Size = new System.Drawing.Size(38, 15);
            this.lbl_endTime.TabIndex = 8;
            this.lbl_endTime.Text = "End: -";
            // 
            // btn_delete_orders
            // 
            this.btn_delete_orders.Location = new System.Drawing.Point(581, 228);
            this.btn_delete_orders.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_delete_orders.Name = "btn_delete_orders";
            this.btn_delete_orders.Size = new System.Drawing.Size(88, 22);
            this.btn_delete_orders.TabIndex = 10;
            this.btn_delete_orders.Text = "Delete";
            this.btn_delete_orders.UseVisualStyleBackColor = true;
            this.btn_delete_orders.Click += new System.EventHandler(this.btn_delete_orders_Click);
            // 
            // MainConsoleMenu
            // 
            this.MainConsoleMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainConsoleMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4});
            this.MainConsoleMenu.Location = new System.Drawing.Point(0, 0);
            this.MainConsoleMenu.Name = "MainConsoleMenu";
            this.MainConsoleMenu.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.MainConsoleMenu.Size = new System.Drawing.Size(1042, 24);
            this.MainConsoleMenu.TabIndex = 12;
            this.MainConsoleMenu.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(49, 20);
            this.toolStripMenuItem1.Text = "Order";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.OrderMenuItemClick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(70, 20);
            this.toolStripMenuItem2.Text = "Symboles";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.SymboleMenuItemClick);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(39, 20);
            this.toolStripMenuItem3.Text = "Test";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.TestSendOrderOpenOrderMenuItemClick);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem6,
            this.toolStripMenuItem7});
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(42, 20);
            this.toolStripMenuItem4.Text = "User";
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(119, 22);
            this.toolStripMenuItem6.Text = "Account";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.AccountMenuItemClick);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(119, 22);
            this.toolStripMenuItem7.Text = "Login";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.LoginMenuItemClick);
            // 
            // lbl_ws_time
            // 
            this.lbl_ws_time.AutoSize = true;
            this.lbl_ws_time.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_ws_time.ForeColor = System.Drawing.Color.Red;
            this.lbl_ws_time.Location = new System.Drawing.Point(707, 87);
            this.lbl_ws_time.Name = "lbl_ws_time";
            this.lbl_ws_time.Size = new System.Drawing.Size(80, 15);
            this.lbl_ws_time.TabIndex = 14;
            this.lbl_ws_time.Text = "WS: 00:00:00";
            this.lbl_ws_time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer_cando
            // 
            this.timer_cando.Enabled = true;
            this.timer_cando.Interval = 1800000;
            this.timer_cando.Tick += new System.EventHandler(this.timer_cando_Tick);
            // 
            // lbl_done
            // 
            this.lbl_done.AutoSize = true;
            this.lbl_done.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lbl_done.ForeColor = System.Drawing.Color.White;
            this.lbl_done.Location = new System.Drawing.Point(191, 230);
            this.lbl_done.Name = "lbl_done";
            this.lbl_done.Size = new System.Drawing.Size(21, 19);
            this.lbl_done.TabIndex = 15;
            this.lbl_done.Text = "--";
            // 
            // timer_real_time
            // 
            this.timer_real_time.Enabled = true;
            this.timer_real_time.Interval = 1000;
            this.timer_real_time.Tick += new System.EventHandler(this.timer_real_time_Tick);
            // 
            // lbl_pc_time
            // 
            this.lbl_pc_time.AutoSize = true;
            this.lbl_pc_time.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_pc_time.ForeColor = System.Drawing.Color.CornflowerBlue;
            this.lbl_pc_time.Location = new System.Drawing.Point(712, 70);
            this.lbl_pc_time.Name = "lbl_pc_time";
            this.lbl_pc_time.Size = new System.Drawing.Size(75, 15);
            this.lbl_pc_time.TabIndex = 14;
            this.lbl_pc_time.Text = "PC: 00:00:00";
            this.lbl_pc_time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_ws_logs
            // 
            this.tb_ws_logs.AcceptsReturn = true;
            this.tb_ws_logs.Location = new System.Drawing.Point(675, 159);
            this.tb_ws_logs.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tb_ws_logs.Multiline = true;
            this.tb_ws_logs.Name = "tb_ws_logs";
            this.tb_ws_logs.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_ws_logs.Size = new System.Drawing.Size(351, 330);
            this.tb_ws_logs.TabIndex = 0;
            this.tb_ws_logs.WordWrap = false;
            // 
            // timer_stay_tune
            // 
            this.timer_stay_tune.Enabled = true;
            this.timer_stay_tune.Interval = 8000;
            this.timer_stay_tune.Tick += new System.EventHandler(this.timer_stay_tune_Tick);
            // 
            // lbl_login
            // 
            this.lbl_login.AutoSize = true;
            this.lbl_login.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_login.ForeColor = System.Drawing.Color.Red;
            this.lbl_login.Location = new System.Drawing.Point(696, 139);
            this.lbl_login.Name = "lbl_login";
            this.lbl_login.Size = new System.Drawing.Size(91, 15);
            this.lbl_login.TabIndex = 14;
            this.lbl_login.Text = "Login: 00:00:00";
            this.lbl_login.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_option_time
            // 
            this.lbl_option_time.AutoSize = true;
            this.lbl_option_time.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_option_time.ForeColor = System.Drawing.Color.Red;
            this.lbl_option_time.Location = new System.Drawing.Point(688, 121);
            this.lbl_option_time.Name = "lbl_option_time";
            this.lbl_option_time.Size = new System.Drawing.Size(99, 15);
            this.lbl_option_time.TabIndex = 14;
            this.lbl_option_time.Text = "Option: 00:00:00";
            this.lbl_option_time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_openorders_time
            // 
            this.lbl_openorders_time.AutoSize = true;
            this.lbl_openorders_time.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_openorders_time.ForeColor = System.Drawing.Color.Red;
            this.lbl_openorders_time.Location = new System.Drawing.Point(688, 104);
            this.lbl_openorders_time.Name = "lbl_openorders_time";
            this.lbl_openorders_time.Size = new System.Drawing.Size(99, 15);
            this.lbl_openorders_time.TabIndex = 14;
            this.lbl_openorders_time.Text = "Orders: 00:00:00";
            this.lbl_openorders_time.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer_option
            // 
            this.timer_option.Enabled = true;
            this.timer_option.Interval = 5000;
            this.timer_option.Tick += new System.EventHandler(this.timer_option_Tick);
            // 
            // timer_login
            // 
            this.timer_login.Enabled = true;
            this.timer_login.Interval = 4000;
            this.timer_login.Tick += new System.EventHandler(this.timer_login_Tick);
            // 
            // MainBotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1042, 496);
            this.Controls.Add(this.lbl_login);
            this.Controls.Add(this.lbl_option_time);
            this.Controls.Add(this.lbl_openorders_time);
            this.Controls.Add(this.tb_ws_logs);
            this.Controls.Add(this.lbl_pc_time);
            this.Controls.Add(this.lbl_done);
            this.Controls.Add(this.lbl_ws_time);
            this.Controls.Add(this.btn_delete_orders);
            this.Controls.Add(this.lbl_endTime);
            this.Controls.Add(this.tb_ss);
            this.Controls.Add(this.tb_mm);
            this.Controls.Add(this.tb_hh);
            this.Controls.Add(this.tb_ms);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_logs);
            this.Controls.Add(this.lv_orders);
            this.Controls.Add(this.lbl_starttime);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_interval);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_duration);
            this.Controls.Add(this.btn_load);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.MainConsoleMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.Name = "MainBotForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sniper Bot";
            this.Load += new System.EventHandler(this.MainBotForm_Load);
            this.MainConsoleMenu.ResumeLayout(false);
            this.MainConsoleMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.ListView lv_orders;
        private System.Windows.Forms.TextBox tb_logs;
        private System.Windows.Forms.ColumnHeader symName;
        private System.Windows.Forms.ColumnHeader count;
        private System.Windows.Forms.ColumnHeader total;
        private System.Windows.Forms.ColumnHeader orderType;
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.TextBox tb_duration;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_interval;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_starttime;
        private System.Windows.Forms.TextBox tb_hh;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tb_mm;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_ss;
        private System.Windows.Forms.TextBox tb_ms;
        private System.Windows.Forms.Label lbl_endTime;
        //private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.Button btn_delete_orders;
        private System.Windows.Forms.ColumnHeader orderId;
        private System.Windows.Forms.ColumnHeader times;
        private System.Windows.Forms.MenuStrip MainConsoleMenu;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ColumnHeader price;
        private System.Windows.Forms.ColumnHeader hit;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.Label lbl_ws_time;
        private System.Windows.Forms.Timer timer_cando;
        private System.Windows.Forms.Label lbl_done;
        private System.Windows.Forms.Timer timer_real_time;
        private System.Windows.Forms.Label lbl_pc_time;
        private System.Windows.Forms.TextBox tb_ws_logs;
        private System.Windows.Forms.Timer timer_stay_tune;
        private System.Windows.Forms.Label lbl_login;
        private System.Windows.Forms.Label lbl_option_time;
        private System.Windows.Forms.Label lbl_openorders_time;
        private System.Windows.Forms.Timer timer_option;
        private System.Windows.Forms.Timer timer_login;
    }
}