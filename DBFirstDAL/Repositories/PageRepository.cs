using Common.SearchClasses;
using DBFirstDAL.Intercaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pyramid.Entity;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DBFirstDAL.Repositories
{
    public class PageRepository : GenericRepository<Pages, PyramidFinalContext, Pyramid.Entity.Page, SearchParamsBase, int>
    {
        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Pages dbEntity, Page entity, bool exists)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<Pages> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Pages> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        protected override Page ConvertDbObjectToEntity(PyramidFinalContext context, Pages dbObject)
        {
            throw new NotImplementedException();
        }

        protected override Pages GetDbObjectByEntity(DbSet<Pages> objects, Page entity)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<Pages, int>> GetIdByDbObjectExpression()
        {
            throw new NotImplementedException();
        }
    }
}
