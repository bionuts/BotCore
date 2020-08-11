using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Data
{
    public class CookieItem
    {
        // Multiple attributes are also possible, for example:
        // Set-Cookie: <cookie-name>=<cookie-value>; Domain=<domain-value>; Secure; HttpOnly

        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Expires { get; set; }
        public string MaxAge { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
        public string SameSite { get; set; } // Strict , Lax , None
        public string Secure { get; set; }
        public string HttpOnly { get; set; }
    }
}
