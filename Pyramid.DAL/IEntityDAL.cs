using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL
{
     interface  IEntityDAL<T>
    {
        int  AddOrUpdateEntity(T entity);
        void DeleteEntity(int entityId);
    }
}
