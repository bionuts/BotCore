namespace BCore.Forms
{
    partial class OrderForm
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
            this.cb_symboles = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_buy = new System.Windows.Forms.Button();
            this.tb_count = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tb_price = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_total = new System.Windows.Forms.Label();
            this.btn_sell = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cb_symboles
            // 
            this.cb_symboles.DisplayMember = "SymName";
            this.cb_symboles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_symboles.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.cb_symboles.FormattingEnabled = true;
            this.cb_symboles.Location = new System.Drawing.Point(21, 58);
            this.cb_symboles.Name = "cb_symboles";
            this.cb_symboles.Size = new System.Drawing.Size(227, 36);
            this.cb_symboles.TabIndex = 0;
            this.cb_symboles.ValueMember = "SymCode";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(21, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Symbole:";
            // 
            // btn_buy
            // 
            this.btn_buy.BackColor = System.Drawing.Color.LightGreen;
            this.btn_buy.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_buy.ForeColor = System.Drawing.Color.Green;
            this.btn_buy.Location = new System.Drawing.Point(564, 58);
            this.btn_buy.Name = "btn_buy";
            this.btn_buy.Size = new System.Drawing.Size(111, 36);
            this.btn_buy.TabIndex = 3;
            this.btn_buy.Text = "Buy";
            this.btn_buy.UseVisualStyleBackColor = false;
            this.btn_buy.Click += new System.EventHandler(this.btn_send_order_Click);
            // 
            // tb_count
            // 
            this.tb_count.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tb_count.Location = new System.Drawing.Point(254, 59);
            this.tb_count.Name = "tb_count";
            this.tb_count.Size = new System.Drawing.Size(149, 34);
            this.tb_count.TabIndex = 1;
            this.tb_count.TextChanged += new System.EventHandler(this.tb_count_price_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(254, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Count:";
            // 
            // tb_price
            // 
            this.tb_price.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tb_price.Location = new System.Drawing.Point(409, 59);
            this.tb_price.Name = "tb_price";
            this.tb_price.Size = new System.Drawing.Size(149, 34);
            this.tb_price.TabIndex = 2;
            this.tb_price.TextChanged += new System.EventHandler(this.tb_count_price_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label3.Location = new System.Drawing.Point(409, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Price:";
            // 
            // lbl_total
            // 
            this.lbl_total.AutoSize = true;
            this.lbl_total.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lbl_total.Location = new System.Drawing.Point(21, 105);
            this.lbl_total.Name = "lbl_total";
            this.lbl_total.Size = new System.Drawing.Size(101, 35);
            this.lbl_total.TabIndex = 4;
            this.lbl_total.Text = "Total: 0";
            // 
            // btn_sell
            // 
            this.btn_sell.BackColor = System.Drawing.Color.Red;
            this.btn_sell.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btn_sell.Location = new System.Drawing.Point(681, 58);
            this.btn_sell.Name = "btn_sell";
            this.btn_sell.Size = new System.Drawing.Size(111, 36);
            this.btn_sell.TabIndex = 4;
            this.btn_sell.Text = "Sell";
            this.btn_sell.UseVisualStyleBackColor = false;
            this.btn_sell.Click += new System.EventHandler(this.btn_send_order_Click);
            // 
            // OrderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(810, 169);
            this.Controls.Add(this.btn_sell);
            this.Controls.Add(this.lbl_total);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tb_price);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_count);
            this.Controls.Add(this.btn_buy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cb_symboles);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OrderForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Orders";
            this.Load += new System.EventHandler(this.OrderForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cb_symboles;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_buy;
        private System.Windows.Forms.TextBox tb_count;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_price;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_total;
        private System.Windows.Forms.Button btn_sell;
    }
}