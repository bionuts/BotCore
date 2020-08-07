using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Data
{
    public class BCookie
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Value { get; set; }

        [MaxLength(200)]
        public string Expires { get; set; }

        [MaxLength(50)]
        public string MaxAge { get; set; }

        [MaxLength(200)]
        public string Domain { get; set; }

        [MaxLength(200)]
        public string Path { get; set; }

        [MaxLength(15)]
        public string SameSite { get; set; } // Strict , Lax , None

        [MaxLength(15)]
        public string Secure { get; set; }

        [MaxLength(15)]
        public string HttpOnly { get; set; }

        // Multiple attributes are also possible, for example:
        // Set-Cookie: <cookie-name>=<cookie-value>; Domain=<domain-value>; Secure; HttpOnly
    }
}
