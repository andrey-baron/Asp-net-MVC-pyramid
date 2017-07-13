using Common.SearchClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace DBFirstDAL.Repositories
{
    public class SeoRepository : GenericRepository<Seo, PyramidFinalContext, Entity.Seo, SearchParamsBase, int>
    {
        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Seo dbEntity, Entity.Seo entity, bool exists)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<Seo> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Seo> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        public override Entity.Seo ConvertDbObjectToEntity(PyramidFinalContext context, Seo dbObject)
        {
            throw new NotImplementedException();
        }

        protected override Seo GetDbObjectByEntity(DbSet<Seo> objects, Entity.Seo entity)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Seo, int>> GetIdByDbObjectExpression()
        {
            throw new NotImplementedException();
        }
    }
}
