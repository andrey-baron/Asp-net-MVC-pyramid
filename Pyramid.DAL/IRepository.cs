using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL
{
     interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T Get(int id);
        void AddOrUpdateEntity(T entity);
        void Delete(int entityId);
    }
}
