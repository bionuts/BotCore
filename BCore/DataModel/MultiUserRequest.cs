using BotCore.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCore.DataModel
{
    class MultiUserRequest
    {
        public BAccount BAccount { get; set; }
        public List<BOrder> Orders { get; set; }
        public int QPTR { get; set; }
    }
}
