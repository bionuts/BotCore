using BCore.Contracts;
using BotCore.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCore.Lib
{
    class MofidBroker : IMofidBroker
    {
        public void CheckOrderList()
        {
            throw new NotImplementedException();
        }

        public void FirstHint()
        {
            throw new NotImplementedException();
        }

        public List<BOrder> GetOpenOrders()
        {
            Console.WriteLine("Mofid GetOpenOrders Called");
            return null;
        }

        public bool Login()
        {
            Console.WriteLine("Mofid Login Called");
            return true;
        }

        public void SendOrder()
        {
            Console.WriteLine("Mofid SendOrder Called");
        }
    }
}
