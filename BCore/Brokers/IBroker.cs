using System;
using System.Collections.Generic;
using System.Text;

namespace BCore.Brokers
{
    interface IBroker
    {
        public void Login();

        public void SendOrder();
    }
}
