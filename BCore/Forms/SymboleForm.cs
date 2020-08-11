using BCore.Data;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BCore.Forms
{
    public partial class SymboleForm : Form
    {
        private readonly ApplicationDbContext db;

        public SymboleForm()
        {
            InitializeComponent();
            db = new ApplicationDbContext();
        }

        private async void SymboleForm_Load(object sender, EventArgs e)
        {
            await ReLoadListView();
        }

        private async void btn_add_sym_Click(object sender, EventArgs e)
        {
            BSymbole sym = new BSymbole
            {
                SymName = tb_sym_name.Text.Trim(),
                SymCode = tb_sym_code.Text.Trim()
            };
            db.BSymboles.Add(sym);
            await db.SaveChangesAsync();
            await ReLoadListView();
        }

        private async void btn_delete_sym_Click(object sender, EventArgs e)
        {
            if (lv_symboles.SelectedItems.Count > 0)
            {
                var id = int.Parse(lv_symboles.SelectedItems[0].SubItems[0].Text);
                var item = await db.BSymboles.FindAsync(id);
                if (item != null)
                {
                    db.BSymboles.Remove(item);
                    await db.SaveChangesAsync();
                }
                await ReLoadListView();
            }
        }

        private async Task ReLoadListView()
        {
            lv_symboles.Items.Clear();
            var symboles = await db.BSymboles.ToListAsync();
            if (symboles.Count > 0)
            {
                foreach (var sym in symboles)
                {
                    var row = new string[]
                    {
                        sym.Id.ToString(),
                        sym.SymName,
                        sym.SymCode
                    };
                    var lvItem = new ListViewItem(row);
                    lv_symboles.Items.Add(lvItem);
                }
            }
        }
    }
}
