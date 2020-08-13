using BotCore.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BCore.Contracts
{
    public interface IBotDatabaseRepository
    {
        List<BOrder> OrdersFindAll();
        
        bool Save();

        string Hello();
    }
}
