namespace BCore.Forms
{
    partial class SendOrderForm
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
            this.btn_sell = new System.Windows.Forms.Button();
            this.lbl_total = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_price = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_count = new System.Windows.Forms.TextBox();
            this.btn_buy = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.cb_symboles = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tb_response = new System.Windows.Forms.TextBox();
            this.btn_get_open_orders = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_sell
            // 
            this.btn_sell.BackColor = System.Drawing.Color.Red;
            this.btn_sell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_sell.Location = new System.Drawing.Point(672, 40);
            this.btn_sell.Name = "btn_sell";
            this.btn_sell.Size = new System.Drawing.Size(111, 36);
            this.btn_sell.TabIndex = 4;
            this.btn_sell.Text = "Sell";
            this.btn_sell.UseVisualStyleBackColor = false;
            this.btn_sell.Click += new System.EventHandler(this.btn_send_order_Click);
            // 
            // lbl_total
            // 
            this.lbl_total.AutoSize = true;
            this.lbl_total.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_total.Location = new System.Drawing.Point(12, 87);
            this.lbl_total.Name = "lbl_total";
            this.lbl_total.Size = new System.Drawing.Size(101, 35);
            this.lbl_total.TabIndex = 4;
            this.lbl_total.Text = "Total: 0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(400, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Price:";
            // 
            // tb_price
            // 
            this.tb_price.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tb_price.Location = new System.Drawing.Point(400, 41);
            this.tb_price.Name = "tb_price";
            this.tb_price.Size = new System.Drawing.Size(149, 34);
            this.tb_price.TabIndex = 2;
            this.tb_price.TextChanged += new System.EventHandler(this.tb_count_price_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(245, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Count:";
            // 
            // tb_count
            // 
            this.tb_count.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tb_count.Location = new System.Drawing.Point(245, 41);
            this.tb_count.Name = "tb_count";
            this.tb_count.Size = new System.Drawing.Size(149, 34);
            this.tb_count.TabIndex = 1;
            this.tb_count.TextChanged += new System.EventHandler(this.tb_count_price_TextChanged);
            // 
            // btn_buy
            // 
            this.btn_buy.BackColor = System.Drawing.Color.LightGreen;
            this.btn_buy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_buy.ForeColor = System.Drawing.Color.Green;
            this.btn_buy.Location = new System.Drawing.Point(555, 40);
            this.btn_buy.Name = "btn_buy";
            this.btn_buy.Size = new System.Drawing.Size(111, 36);
            this.btn_buy.TabIndex = 3;
            this.btn_buy.Text = "Buy";
            this.btn_buy.UseVisualStyleBackColor = false;
            this.btn_buy.Click += new System.EventHandler(this.btn_send_order_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label4.Location = new System.Drawing.Point(12, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Symbole:";
            // 
            // cb_symboles
            // 
            this.cb_symboles.DisplayMember = "SymName";
            this.cb_symboles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_symboles.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cb_symboles.FormattingEnabled = true;
            this.cb_symboles.Location = new System.Drawing.Point(12, 40);
            this.cb_symboles.Name = "cb_symboles";
            this.cb_symboles.Size = new System.Drawing.Size(227, 36);
            this.cb_symboles.TabIndex = 0;
            this.cb_symboles.ValueMember = "SymCode";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tb_response);
            this.groupBox1.Location = new System.Drawing.Point(12, 160);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1217, 374);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Response";
            // 
            // tb_response
            // 
            this.tb_response.Location = new System.Drawing.Point(6, 26);
            this.tb_response.Multiline = true;
            this.tb_response.Name = "tb_response";
            this.tb_response.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tb_response.Size = new System.Drawing.Size(1205, 342);
            this.tb_response.TabIndex = 0;
            this.tb_response.WordWrap = false;
            // 
            // btn_get_open_orders
            // 
            this.btn_get_open_orders.Location = new System.Drawing.Point(12, 125);
            this.btn_get_open_orders.Name = "btn_get_open_orders";
            this.btn_get_open_orders.Size = new System.Drawing.Size(197, 29);
            this.btn_get_open_orders.TabIndex = 6;
            this.btn_get_open_orders.Text = "Get Open Orders";
            this.btn_get_open_orders.UseVisualStyleBackColor = true;
            this.btn_get_open_orders.Click += new System.EventHandler(this.btn_get_open_orders_Click);
            // 
            // SendOrderForm
            // 
            this.ClientSize = new System.Drawing.Size(1241, 546);
            this.Controls.Add(this.btn_get_open_orders);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cb_symboles);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_buy);
            this.Controls.Add(this.tb_count);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_price);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_total);
            this.Controls.Add(this.btn_sell);
            this.Name = "SendOrderForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.SendOrderForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_sell;
        private System.Windows.Forms.Label lbl_total;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_price;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_count;
        private System.Windows.Forms.Button btn_buy;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cb_symboles;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tb_response;
        private System.Windows.Forms.Button btn_get_open_orders;
    }
}