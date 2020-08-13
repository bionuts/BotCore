using BCore.Contracts;
using BotCore.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCore.Lib
{
    class MobinBroker : IMobinBroker
    {
        private string baseAddress = "https://www.silver.mobinsb.com";
        private int intervalPat = 20; // ms

        public List<BOrder> GetOpenOrders()
        {
            Console.WriteLine("Mobin GetOpenOrders Called");
            return null;
        }

        public bool Login()
        {
            Console.WriteLine("Mobin Login Called");
            return true;
        }

        public void LogOut()
        {
            throw new NotImplementedException();
        }

        public void SendOrder()
        {
            Console.WriteLine("Mobin SendOrder Called");
        }
    }
}
