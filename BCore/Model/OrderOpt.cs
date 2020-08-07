using BotCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Models
{
    public class OrderOpt : BOrder
    {
        public string OrderId { get; set; }
        public int TrySend { get; set; }
        public DateTime WhenSent { get; set; }
        public bool Done { get; set; }
    }
}
