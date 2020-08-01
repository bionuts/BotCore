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

namespace BCore
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_http_req_Click(object sender, EventArgs e)
        {
            var httpClient = HttpClientFactory.Create();
            var url = "https://silver.mobinsb.com/login";
            var output = 
        }
    }
}
