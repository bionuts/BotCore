using System;
using System.Collections.Generic;
using System.Text;

namespace BCore.DataModel
{
    public class OrderRespond
    {
        public OrderRespondData Data { get; set; }
        public string MessageDesc { get; set; } = "";
        public bool IsSuccessfull { get; set; }
        public string MessageCode { get; set; } = "";
        public int Version { get; set; }
    }

    public class OrderRespondData
    {
        public int OrderId { get; set; }
    }
}
