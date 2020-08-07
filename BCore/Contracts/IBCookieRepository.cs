using BotCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Contracts
{
    public interface IBCookieRepository : IRepositoryBase<BCookie>
    {
        BCookie FindByNameAuth();
    }
}
