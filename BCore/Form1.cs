using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        private async void btn_http_req_Click(object sender, EventArgs e)
        {
            var httpClient = HttpClientFactory.Create();
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
            
            var url = "https://silver.mobinsb.com/login";
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);
            IEnumerable<string> cookies = httpResponseMessage.Headers.SingleOrDefault(header => header.Key == "Set-Cookie").Value;
            var content = httpResponseMessage.Content;
            var output = content.ReadAsStringAsync();
            txt_log.Text = await output;
        }
    }
}
