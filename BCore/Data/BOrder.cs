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
        [MaxLength(10)]
        public string TypeOrder { get; set; }

        [Required]
        [MaxLength(100)]
        public string SymboleName { get; set; }

        [Required]
        [MaxLength(100)]
        public string SymboleCode { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public int Price { get; set; }

        [Required]
        public DateTime CreatedDateTime { get; set; }

        [MaxLength(200)]
        public string Stat { get; set; }

        [Required]
        [MaxLength(100)]
        public string OrderId { get; set; }

        [Required]
        public int TrySendCount { get; set; }
        
        public DateTime LastTrySendTime { get; set; }

        [Required]
        public bool Done { get; set; }
    }
}
