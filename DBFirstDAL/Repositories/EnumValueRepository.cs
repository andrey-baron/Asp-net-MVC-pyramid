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
            dbEntity.Key = entity.Key;
            dbEntity.TypeValue = (int)entity.TypeValue;

            
        }

        protected override IQueryable<EnumValues> BuildDbObjectsList(PyramidFinalContext context, IQueryable<EnumValues> dbObjects, SearchParamsBase searchParams)
        {
            dbObjects = dbObjects.OrderBy(item => item.Id);
            return dbObjects;
        }

        public override EnumValue ConvertDbObjectToEntity(PyramidFinalContext context, EnumValues dbObject)
        {
            var entity = new EnumValue() {
                Id=dbObject.Id,
                Key=dbObject.Key,
               TypeValue=(Common.TypeFromEnumValue)dbObject.TypeValue 
            };
            return entity;
        }

        protected override EnumValues GetDbObjectByEntity(DbSet<EnumValues> objects, EnumValue entity)
        {
            return objects.FirstOrDefault(f => f.Id == entity.Id);
        }

        protected override Expression<Func<EnumValues, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }
    }
}
