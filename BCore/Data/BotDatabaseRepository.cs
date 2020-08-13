using BCore.Contracts;
using BotCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCore.Data
{
    class BotDatabaseRepository : IBotDatabaseRepository
    {
        private readonly ApplicationDbContext db;
        private int callCount = 0;

        public BotDatabaseRepository()
        {
            db = new ApplicationDbContext();
        }

        public List<BOrder> OrdersFindAll()
        {
            return db.BOrders.ToList();
        }

        public bool Save()
        {
            return db.SaveChanges() > 0;
        }

        public string Hello()
        {
            callCount++;
            return $"Hello {callCount}";
        }
    }
}
