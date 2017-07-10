using Common.SearchClasses;
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
    public class EnumValueRepository : GenericRepository<EnumValues, PyramidFinalContext, Pyramid.Entity.EnumValue, SearchParamsBase, int>
    {
        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, EnumValues dbEntity, EnumValue entity, bool exists)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<EnumValues> BuildDbObjectsList(PyramidFinalContext context, IQueryable<EnumValues> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        protected override EnumValue ConvertDbObjectToEntity(PyramidFinalContext context, EnumValues dbObject)
        {
            throw new NotImplementedException();
        }

        protected override EnumValues GetDbObjectByEntity(DbSet<EnumValues> objects, EnumValue entity)
        {
            throw new NotImplementedException();
        }

        protected override Expression<Func<EnumValues, int>> GetIdByDbObjectExpression()
        {
            throw new NotImplementedException();
        }
    }
}
