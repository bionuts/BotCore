using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Data
{
    public class BSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
