using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Data
{
    public class BOrder
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string OrderType { get; set; }

        [Required]
        public string SymboleName { get; set; }

        [Required]
        public string SymboleCode { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        public string Status { get; set; }

        [Required]
        public string OrderId { get; set; }
    }
}
