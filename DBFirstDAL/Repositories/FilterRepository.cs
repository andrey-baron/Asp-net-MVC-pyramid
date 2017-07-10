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
        public IEnumerable<EnumValues> GetAllEnumValues(int filterId)
        {
            var filter = FindBy(i => i.Id == filterId).SingleOrDefault();
            if (filter!=null)
            {
                return filter.EnumValues;
            }
            return new List<EnumValues>();
        }
        public void DeleteEnumValue(int id, int enumValueId)
        {
            var filter = FindBy(i => i.Id == id).SingleOrDefault();
            if (filter != null)
            {
                var enumvalue = Context.EnumValues.Find(enumValueId);
                filter.EnumValues.Remove(enumvalue);

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
            throw new NotImplementedException();
        }

        protected override Filter ConvertDbObjectToEntity(PyramidFinalContext context, Filters dbObject)
        {
            throw new NotImplementedException();
        }

        protected override IQueryable<Filters> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Filters> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Filters dbEntity, Filter entity, bool exists)
        {
            throw new NotImplementedException();
        }
    }
    
}
