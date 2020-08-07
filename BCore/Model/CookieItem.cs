using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.ViewModel
{
    public class CookieItem
    {
        public CookieItem(string k,string v)
        {
            Key = k;
            Value = v;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
