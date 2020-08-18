using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BCore.DataModel
{
    public class ThreadParamObject
    {
        public int ID { get; set; }
        public string SYM { get; set; }
        public HttpRequestMessage REQ { get; set; }
        public int WhichOne { get; set; }
    }
}
