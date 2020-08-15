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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainBotForm));
            this.btn_start = new System.Windows.Forms.Button();
            this.lv_orders = new System.Windows.Forms.ListView();
            this.orderId = new System.Windows.Forms.ColumnHeader();
            this.symCode = new System.Windows.Forms.ColumnHeader();
            this.symName = new System.Windows.Forms.ColumnHeader();
            this.countprice = new System.Windows.Forms.ColumnHeader();
            this.price = new System.Windows.Forms.ColumnHeader();
            this.total = new System.Windows.Forms.ColumnHeader();
            this.orderType = new System.Windows.Forms.ColumnHeader();
            this.sendingtimes = new System.Windows.Forms.ColumnHeader();
            this.orderStatus = new System.Windows.Forms.ColumnHeader();
            this.tb_logs = new System.Windows.Forms.TextBox();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.btn_load = new System.Windows.Forms.Button();
            this.lbl_status = new System.Windows.Forms.Label();
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
            this.MainMenuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_delete_orders = new System.Windows.Forms.Button();
            this.lbl_path = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.MainMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_start
            // 
            this.btn_start.Location = new System.Drawing.Point(158, 270);
            this.btn_start.Name = "btn_start";
            this.btn_start.Size = new System.Drawing.Size(140, 29);
            this.btn_start.TabIndex = 0;
            this.btn_start.Text = "Start";
            this.btn_start.UseVisualStyleBackColor = true;
            this.btn_start.Click += new System.EventHandler(this.btn_start_Click);
            // 
            // lv_orders
            // 
            this.lv_orders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.orderId,
            this.symCode,
            this.symName,
            this.countprice,
            this.price,
            this.total,
            this.orderType,
            this.sendingtimes,
            this.orderStatus});
            this.lv_orders.FullRowSelect = true;
            this.lv_orders.GridLines = true;
            this.lv_orders.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lv_orders.HideSelection = false;
            this.lv_orders.Location = new System.Drawing.Point(12, 81);
            this.lv_orders.Name = "lv_orders";
            this.lv_orders.Size = new System.Drawing.Size(1478, 183);
            this.lv_orders.TabIndex = 2;
            this.lv_orders.UseCompatibleStateImageBehavior = false;
            this.lv_orders.View = System.Windows.Forms.View.Details;
            // 
            // orderId
            // 
            this.orderId.Name = "orderId";
            this.orderId.Text = "ID";
            this.orderId.Width = 50;
            // 
            // symCode
            // 
            this.symCode.Name = "symCode";
            this.symCode.Text = "SymCode[X]";
            this.symCode.Width = 130;
            // 
            // symName
            // 
            this.symName.Name = "symName";
            this.symName.Text = "Symbole";
            this.symName.Width = 120;
            // 
            // countprice
            // 
            this.countprice.Name = "countprice";
            this.countprice.Text = "Count/Price";
            this.countprice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.countprice.Width = 110;
            // 
            // price
            // 
            this.price.Name = "price";
            this.price.Text = "Price";
            this.price.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.price.Width = 100;
            // 
            // total
            // 
            this.total.Name = "total";
            this.total.Text = "Total";
            this.total.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.total.Width = 120;
            // 
            // orderType
            // 
            this.orderType.Name = "orderType";
            this.orderType.Text = "Type";
            // 
            // sendingtimes
            // 
            this.sendingtimes.Name = "sendingtimes";
            this.sendingtimes.Text = "Times / Hit";
            this.sendingtimes.Width = 100;
            // 
            // orderStatus
            // 
            this.orderStatus.Name = "orderStatus";
            this.orderStatus.Text = "Status";
            this.orderStatus.Width = 320;
            // 
            // tb_logs
            // 
            this.tb_logs.AcceptsReturn = true;
            this.tb_logs.Location = new System.Drawing.Point(12, 325);
            this.tb_logs.Multiline = true;
            this.tb_logs.Name = "tb_logs";
            this.tb_logs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_logs.Size = new System.Drawing.Size(1089, 323);
            this.tb_logs.TabIndex = 0;
            // 
            // btn_load
            // 
            this.btn_load.Location = new System.Drawing.Point(12, 270);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(140, 29);
            this.btn_load.TabIndex = 4;
            this.btn_load.Text = "Load Orders";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.btn_load_Click);
            // 
            // lbl_status
            // 
            this.lbl_status.AutoSize = true;
            this.lbl_status.Location = new System.Drawing.Point(12, 302);
            this.lbl_status.Name = "lbl_status";
            this.lbl_status.Size = new System.Drawing.Size(43, 20);
            this.lbl_status.TabIndex = 5;
            this.lbl_status.Text = "Logs:";
            // 
            // tb_duration
            // 
            this.tb_duration.Location = new System.Drawing.Point(104, 46);
            this.tb_duration.MaxLength = 4;
            this.tb_duration.Name = "tb_duration";
            this.tb_duration.Size = new System.Drawing.Size(47, 27);
            this.tb_duration.TabIndex = 1;
            this.tb_duration.Text = "2.65";
            this.tb_duration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Duration(s):";
            // 
            // tb_interval
            // 
            this.tb_interval.Location = new System.Drawing.Point(249, 46);
            this.tb_interval.MaxLength = 3;
            this.tb_interval.Name = "tb_interval";
            this.tb_interval.Size = new System.Drawing.Size(47, 27);
            this.tb_interval.TabIndex = 2;
            this.tb_interval.Text = "200";
            this.tb_interval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(157, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Interval(ms):";
            // 
            // lbl_starttime
            // 
            this.lbl_starttime.AutoSize = true;
            this.lbl_starttime.Location = new System.Drawing.Point(302, 49);
            this.lbl_starttime.Name = "lbl_starttime";
            this.lbl_starttime.Size = new System.Drawing.Size(80, 20);
            this.lbl_starttime.TabIndex = 5;
            this.lbl_starttime.Text = "Start Time:";
            this.lbl_starttime.Click += new System.EventHandler(this.lbl_starttime_Click);
            // 
            // tb_hh
            // 
            this.tb_hh.BackColor = System.Drawing.Color.LightYellow;
            this.tb_hh.Location = new System.Drawing.Point(391, 46);
            this.tb_hh.MaxLength = 2;
            this.tb_hh.Name = "tb_hh";
            this.tb_hh.Size = new System.Drawing.Size(36, 27);
            this.tb_hh.TabIndex = 3;
            this.tb_hh.Text = "08";
            this.tb_hh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(428, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = ":";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(475, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(12, 20);
            this.label5.TabIndex = 7;
            this.label5.Text = ":";
            // 
            // tb_mm
            // 
            this.tb_mm.BackColor = System.Drawing.Color.LightYellow;
            this.tb_mm.Location = new System.Drawing.Point(440, 46);
            this.tb_mm.MaxLength = 2;
            this.tb_mm.Name = "tb_mm";
            this.tb_mm.Size = new System.Drawing.Size(36, 27);
            this.tb_mm.TabIndex = 4;
            this.tb_mm.Text = "29";
            this.tb_mm.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(524, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 20);
            this.label6.TabIndex = 7;
            this.label6.Text = ":";
            // 
            // tb_ss
            // 
            this.tb_ss.BackColor = System.Drawing.Color.LightYellow;
            this.tb_ss.Location = new System.Drawing.Point(488, 46);
            this.tb_ss.MaxLength = 2;
            this.tb_ss.Name = "tb_ss";
            this.tb_ss.Size = new System.Drawing.Size(36, 27);
            this.tb_ss.TabIndex = 5;
            this.tb_ss.Text = "57";
            this.tb_ss.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tb_ms
            // 
            this.tb_ms.BackColor = System.Drawing.Color.LightYellow;
            this.tb_ms.Location = new System.Drawing.Point(538, 46);
            this.tb_ms.MaxLength = 3;
            this.tb_ms.Name = "tb_ms";
            this.tb_ms.Size = new System.Drawing.Size(42, 27);
            this.tb_ms.TabIndex = 6;
            this.tb_ms.Text = "350";
            this.tb_ms.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbl_endTime
            // 
            this.lbl_endTime.AutoSize = true;
            this.lbl_endTime.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_endTime.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbl_endTime.Location = new System.Drawing.Point(587, 49);
            this.lbl_endTime.Name = "lbl_endTime";
            this.lbl_endTime.Size = new System.Drawing.Size(15, 20);
            this.lbl_endTime.TabIndex = 8;
            this.lbl_endTime.Text = "-";
            // 
            // MainMenuStrip
            // 
            this.MainMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem1,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.MainMenuStrip.Location = new System.Drawing.Point(0, 0);
            this.MainMenuStrip.Name = "MainMenuStrip";
            this.MainMenuStrip.Size = new System.Drawing.Size(1502, 28);
            this.MainMenuStrip.TabIndex = 9;
            this.MainMenuStrip.Text = "menuStrip1";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(60, 24);
            this.toolStripMenuItem2.Text = "Login";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.LoginMenuItemClick);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(61, 24);
            this.toolStripMenuItem3.Text = "Order";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.OrderMenuItemClick);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(87, 24);
            this.toolStripMenuItem4.Text = "Symboles";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.SymboleMenuItemClick);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(77, 24);
            this.toolStripMenuItem1.Text = "Account";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.AccountMenuItemClick);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(49, 24);
            this.toolStripMenuItem5.Text = "Test";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.TestSendOrderOpenOrderMenuItemClick);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem7,
            this.toolStripMenuItem8,
            this.toolStripMenuItem9});
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(76, 24);
            this.toolStripMenuItem6.Text = "Settings";
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(156, 26);
            this.toolStripMenuItem7.Text = "Account";
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(156, 26);
            this.toolStripMenuItem8.Text = "Login";
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(156, 26);
            this.toolStripMenuItem9.Text = "Symboles";
            // 
            // btn_delete_orders
            // 
            this.btn_delete_orders.Location = new System.Drawing.Point(961, 270);
            this.btn_delete_orders.Name = "btn_delete_orders";
            this.btn_delete_orders.Size = new System.Drawing.Size(140, 29);
            this.btn_delete_orders.TabIndex = 10;
            this.btn_delete_orders.Text = "Delete Orders";
            this.btn_delete_orders.UseVisualStyleBackColor = true;
            this.btn_delete_orders.Click += new System.EventHandler(this.btn_delete_orders_Click);
            // 
            // lbl_path
            // 
            this.lbl_path.AutoSize = true;
            this.lbl_path.Location = new System.Drawing.Point(304, 274);
            this.lbl_path.Name = "lbl_path";
            this.lbl_path.Size = new System.Drawing.Size(0, 20);
            this.lbl_path.TabIndex = 11;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(861, 270);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(94, 29);
            this.button1.TabIndex = 12;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainBotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1502, 660);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbl_path);
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
            this.Controls.Add(this.lbl_status);
            this.Controls.Add(this.btn_load);
            this.Controls.Add(this.btn_start);
            this.Controls.Add(this.MainMenuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainBotForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WineoBot";
            this.Load += new System.EventHandler(this.MainBotForm_Load);
            this.MainMenuStrip.ResumeLayout(false);
            this.MainMenuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_start;
        private System.Windows.Forms.ListView lv_orders;
        private System.Windows.Forms.TextBox tb_logs;
        private System.Windows.Forms.ColumnHeader symCode;
        private System.Windows.Forms.ColumnHeader symName;
        private System.Windows.Forms.ColumnHeader countprice;
        private System.Windows.Forms.ColumnHeader price;
        private System.Windows.Forms.ColumnHeader total;
        private System.Windows.Forms.ColumnHeader orderType;
        private System.Windows.Forms.ColumnHeader orderStatus;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button btn_load;
        private System.Windows.Forms.Label lbl_status;
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
        // private System.Windows.Forms.MenuStrip MainMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.Button btn_delete_orders;
        private System.Windows.Forms.ColumnHeader orderId;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem5;
        private System.Windows.Forms.Label lbl_path;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem7;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem8;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem9;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColumnHeader sendingtimes;
    }
}