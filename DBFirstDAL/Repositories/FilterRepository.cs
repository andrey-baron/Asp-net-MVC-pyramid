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
    public class FilterRepository : GenericRepository<Filters, PyramidFinalContext, Pyramid.Entity.Filter, SearchParamsBase,int>
    {
        public IEnumerable<Pyramid.Entity.EnumValue> GetAllEnumValues(int filterId)
        {
            var filter = Get(filterId);
            if (filter!=null)
            {
                return filter.EnumValues;
            }
            return new List<Pyramid.Entity.EnumValue>();
        }
        public void DeleteEnumValue(int id, int enumValueId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var filter = dbContext.Filters.Find(id);
                if (filter != null)
                {
                    var enumvalue = dbContext.EnumValues.Find(enumValueId);
                    filter.EnumValues.Remove(enumvalue);
                    dbContext.SaveChanges();
                }
            }
        }
        //public override void AddOrUpdate(Pyramid.Entity.Filter entity)
        //{
        //    var prop = entity.GetType().GetProperty("Id");
        //    int id = (int)prop.GetValue(entity, null);

        //    var efEntity = Context.Filters.Find(id);
        //    if (efEntity == null)
        //    {
        //        var tmpEnumval = new List<EnumValues>(entity.EnumValues);
        //        entity.EnumValues.Clear();
        //        Context.Filters.Add(entity);

        //        Context.SaveChanges();
        //        foreach (var item in tmpEnumval)
        //        {
        //            var efitem = Context.EnumValues.Find(item.Id);
        //            if (efitem != null)
        //            {
        //                entity.EnumValues.Add(efitem);
        //            }
        //        }
        //        Context.SaveChanges();
        //    }
        //    else
        //    {
                
        //        efEntity.EnumValues.Clear();
        //        foreach (var item in entity.EnumValues)
        //        {
        //            var efEVal = Context.EnumValues.Find(item.Id);
        //            if (efEVal!=null)
        //            {
        //                efEntity.EnumValues.Add(efEVal);
        //            }
                    
        //        }
        //        Context.Entry(efEntity).CurrentValues.SetValues(entity);
        //        Context.Entry(efEntity).State = System.Data.Entity.EntityState.Modified;

        //    }

        //}

        protected override Filters GetDbObjectByEntity(DbSet<Filters> objects, Filter entity)
        {
            return objects.FirstOrDefault(i => i.Id == entity.Id);
        }

        protected override Expression<Func<Filters, int>> GetIdByDbObjectExpression()
        {
            return item => item.Id;
        }

        public override Filter ConvertDbObjectToEntity(PyramidFinalContext context, Filters dbObject)
        {
            var filter = new Filter() {
                Id=dbObject.Id,
                Title=dbObject.Title,
                Categories=dbObject.Categories.Select(s=>new Category() {Id=s.Id,
                Title=s.Title,}).ToList(),
               EnumValues=dbObject.EnumValues.Select(s=>new EnumValue() {
                   Id=s.Id,
                   Key=s.Key,
                   TypeValue=(Common.TypeFromEnumValue)s.TypeValue
               }) .ToList()
            };
            return filter;
        }

        protected override IQueryable<Filters> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Filters> dbObjects, SearchParamsBase searchParams)
        {
            dbObjects = dbObjects.OrderBy(item => item.Title).ThenBy(item => item.Id);
            return dbObjects;
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Filters dbEntity, Filter entity, bool exists)
        {
            dbEntity.Title = entity.Title;
        }
        public override void UpdateAfterSaving(PyramidFinalContext dbContext, Filters dbEntity, Filter entity, bool exists)
        {
            dbEntity.EnumValues.Clear();
            foreach (var item in entity.EnumValues)
            {
                var efEVal = dbContext.EnumValues.Find(item.Id);
                if (efEVal != null)
                {
                    dbEntity.EnumValues.Add(efEVal);
                }

            }
        }
    }
    
}
