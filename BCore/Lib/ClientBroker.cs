using BCore.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCore.Lib
{
    class ClientBroker
    {
        readonly IMobinBroker mobinBroker;
        readonly IMofidBroker mofidBroker;

        public ClientBroker(IMobinBroker mobinBroker, IMofidBroker mofidBroker)
        {
            this.mobinBroker = mobinBroker;
            this.mofidBroker = mofidBroker;
        }
    }
}
