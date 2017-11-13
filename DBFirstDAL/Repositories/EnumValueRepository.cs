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
    public class EnumValueRepository : GenericRepository<EnumValues, PyramidFinalContext, Pyramid.Entity.EnumValue, SearchParamsEnumValue, int>
    {
        public EnumValueRepository(PyramidFinalContext context) : base(context) { }
        public EnumValueRepository(){ }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, EnumValues dbEntity, EnumValue entity, bool exists)
        {
            dbEntity.Key = entity.Key;
            dbEntity.TypeValue = (int)entity.TypeValue;

            
        }

        protected override IQueryable<EnumValues> BuildDbObjectsList(PyramidFinalContext context, IQueryable<EnumValues> dbObjects, SearchParamsEnumValue searchParams)
        {
            if (searchParams.ProductId.HasValue)
            {
                dbObjects = dbObjects.Where(w => w.Products.Any(a => a.Id == searchParams.ProductId.Value));
            }
            dbObjects = dbObjects.OrderBy(item => item.Key);
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

        public override IEnumerable<EnumValue> GetAll()
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                var entityObjectList = dbContext.EnumValues
                    .AsNoTracking()
                    .OrderBy(i=>i.Key)
                    .ToList()
                    .Select(s => ConvertDbObjectToEntityShort(dbContext, s))
                    .ToList();
                return entityObjectList;
            }
        }
    }
}
