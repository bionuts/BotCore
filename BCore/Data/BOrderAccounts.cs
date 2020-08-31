using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Data
{
    public class BOrderAccounts
    {
        [Key]
        public int OrderID { get; set; }
        public BOrder BOrder { get; set; }

        [Key]
        public int UserId { get; set; }
        public BAccount BAccount { get; set; }
    }
}
