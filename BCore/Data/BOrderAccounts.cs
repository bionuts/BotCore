using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Data
{
    public class BOrderAccounts
    {
        public int OrderID { get; set; }
        public BOrder BOrder { get; set; }

        public int AccountId { get; set; }
        public BAccount BAccount { get; set; }
    }
}
