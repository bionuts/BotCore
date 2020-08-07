using BotCore.Contracts;
using BotCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Repository
{
    public class BOrderRepository : IBOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public BOrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ClearAll()
        {
            throw new NotImplementedException();
        }

        public bool Create(BOrder entity)
        {
            _db.BOrders.Add(entity);
            return Save();
        }

        public bool Delete(BOrder entity)
        {
            _db.BOrders.Remove(entity);
            return Save();
        }

        public bool Delete(List<BOrder> entities)
        {
            throw new NotImplementedException();
        }

        public BOrder Find(int id)
        {
            return _db.BOrders.Find(id);
        }

        public ICollection<BOrder> FindAll()
        {
            return _db.BOrders.ToList();
        }

        public List<BOrder> FindAllToday()
        {
            return _db.BOrders.Where(t => t.CreatedDateTime.Date == DateTime.Today).ToList();
        }

        public BOrder FindByName(string name)
        {
            var border = _db.BOrders.Where(n => n.SymboleCode == name).FirstOrDefault();
            return border;
        }

        public List<BOrder> Finds(string name)
        {
            throw new NotImplementedException();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool Update(BOrder entity)
        {
            throw new NotImplementedException();
        }
    }
}
