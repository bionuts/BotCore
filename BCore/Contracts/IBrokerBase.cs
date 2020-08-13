using BotCore.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCore.Contracts
{
    public interface IBrokerBase // <T> where T : class
    {
        bool Login();
        List<BOrder> GetOpenOrders();
        void SendOrder();
    }
}
