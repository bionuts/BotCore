using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BCore.DataModel
{
    public class RequestsVector
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public int OrderID { get; set; }
        public string SYM { get; set; }
        public int Count { get; set; }
        public HttpRequestMessage REQ { get; set; }
        public KeyValuePair<int, int> DicKey { get; set; }
    }
}
