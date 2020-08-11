namespace BCore.Forms
{
    partial class SymboleForm
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
            this.btn_add_sym = new System.Windows.Forms.Button();
            this.tb_sym_code = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_sym_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lv_symboles = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.btn_delete_sym = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_add_sym
            // 
            this.btn_add_sym.Location = new System.Drawing.Point(341, 33);
            this.btn_add_sym.Name = "btn_add_sym";
            this.btn_add_sym.Size = new System.Drawing.Size(102, 29);
            this.btn_add_sym.TabIndex = 0;
            this.btn_add_sym.Text = "Add";
            this.btn_add_sym.UseVisualStyleBackColor = true;
            this.btn_add_sym.Click += new System.EventHandler(this.btn_add_sym_Click);
            // 
            // tb_sym_code
            // 
            this.tb_sym_code.Location = new System.Drawing.Point(178, 34);
            this.tb_sym_code.Name = "tb_sym_code";
            this.tb_sym_code.Size = new System.Drawing.Size(157, 27);
            this.tb_sym_code.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(178, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Code:";
            // 
            // tb_sym_name
            // 
            this.tb_sym_name.Location = new System.Drawing.Point(15, 34);
            this.tb_sym_name.Name = "tb_sym_name";
            this.tb_sym_name.Size = new System.Drawing.Size(157, 27);
            this.tb_sym_name.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name:";
            // 
            // lv_symboles
            // 
            this.lv_symboles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lv_symboles.FullRowSelect = true;
            this.lv_symboles.GridLines = true;
            this.lv_symboles.HideSelection = false;
            this.lv_symboles.Location = new System.Drawing.Point(15, 68);
            this.lv_symboles.MultiSelect = false;
            this.lv_symboles.Name = "lv_symboles";
            this.lv_symboles.Size = new System.Drawing.Size(428, 261);
            this.lv_symboles.TabIndex = 3;
            this.lv_symboles.UseCompatibleStateImageBehavior = false;
            this.lv_symboles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Code";
            this.columnHeader3.Width = 130;
            // 
            // btn_delete_sym
            // 
            this.btn_delete_sym.Location = new System.Drawing.Point(15, 335);
            this.btn_delete_sym.Name = "btn_delete_sym";
            this.btn_delete_sym.Size = new System.Drawing.Size(428, 42);
            this.btn_delete_sym.TabIndex = 0;
            this.btn_delete_sym.Text = "Delete";
            this.btn_delete_sym.UseVisualStyleBackColor = true;
            this.btn_delete_sym.Click += new System.EventHandler(this.btn_delete_sym_Click);
            // 
            // SymboleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(458, 387);
            this.Controls.Add(this.btn_delete_sym);
            this.Controls.Add(this.lv_symboles);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tb_sym_name);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tb_sym_code);
            this.Controls.Add(this.btn_add_sym);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SymboleForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Symboles";
            this.Load += new System.EventHandler(this.SymboleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_add_sym;
        private System.Windows.Forms.TextBox tb_sym_code;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_sym_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView lv_symboles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Button btn_delete_sym;
    }
}