using Pyramid.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL
{
    public class FilterRepository : IRepository<Filter>
    {
        private DataContext dbContext;
        
        public FilterRepository(DataContext context)
        {
            this.dbContext = context;
        }
        public void AddOrUpdateEntity(Filter entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public Filter Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Filter> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
