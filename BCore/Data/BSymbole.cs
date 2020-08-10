using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Data
{
    public class BSymbole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string SymboleName { get; set; }

        [Required]
        [MaxLength(100)]
        public string SymboleCode { get; set; }
    }
}
