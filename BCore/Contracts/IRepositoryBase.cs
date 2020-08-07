using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotCore.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        ICollection<T> FindAll();
        T FindByName(string name);
        List<T> Finds(string name);
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool Delete(List<T> entities);
        bool ClearAll();
        bool Save();
    }
}
