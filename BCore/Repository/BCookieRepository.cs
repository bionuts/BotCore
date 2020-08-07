using BotCore.Contracts;
using BotCore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Repository
{
    public class BCookieRepository : IBCookieRepository
    {
        private readonly ApplicationDbContext _db;

        public BCookieRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool ClearAll()
        {
            var all = _db.BCookies;
            foreach (var c in all)
                _db.Remove(c);
            return this.Save();
        }

        public bool Create(BCookie entity)
        {
            _db.BCookies.Add(entity);
            return Save();
        }

        public bool Delete(BCookie entity)
        {
            _db.BCookies.Remove(entity);
            return Save();
        }

        public bool Delete(List<BCookie> entities)
        {
            foreach (var c in entities)
                _db.BCookies.Remove(c);
            return Save();
        }

        public ICollection<BCookie> FindAll()
        {
            return _db.BCookies.ToList();
        }

        public BCookie FindByName(string name)
        {
            var bcookie = _db.BCookies.Where(n => n.Name == name).FirstOrDefault();
            return bcookie;
        }

        public BCookie FindByNameAuth()
        {
            var bcookie = _db.BCookies.Where(n => n.Name == ".ASPXAUTH" && n.Expires != null).FirstOrDefault();
            return bcookie;
        }

        public List<BCookie> Finds(string name)
        {
            return _db.BCookies.Where(n => n.Name == name).ToList();
        }

        public bool Save()
        {
            return _db.SaveChanges() > 0;
        }

        public bool Update(BCookie entity)
        {
            var rec = this.FindByName(entity.Name);
            rec.Value = entity.Value;
            rec.Expires = entity.Expires;
            rec.Path = entity.Path;
            rec.Domain = entity.Domain;
            rec.MaxAge = entity.MaxAge;
            rec.SameSite = entity.SameSite;
            rec.Secure = entity.Secure;
            rec.HttpOnly = entity.HttpOnly;
            _db.BCookies.Update(rec);
            return Save();
        }
    }
}
