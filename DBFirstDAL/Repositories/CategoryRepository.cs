using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBFirstDAL;
using DBFirstDAL.DataModels;

namespace DBFirstDAL.Repositories
{
    public class CategoryRepository:GenericRepository<PyramidFinalContext,Categories>
    {
        public IEnumerable<RootCategory> GetRootCategoriesWithSubs()
        {
            var rootCategories = Context.Categories.Where(i => i.ParentId == null);
            var d =new List<RootCategory>();
            foreach (var item in rootCategories)
            {
                d.Add(new RootCategory()
                {
                    Category = item,
                    SubCategories = Context.Categories.Where(i => i.ParentId == item.Id)
                });
            }
            return d;
           
        }

        public IEnumerable<Filters> GetFilters(int categoryId)
        {
            var efCategory = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efCategory!=null)
            {
                return efCategory.Filters;
            }

            return new List<Filters>();
        }

        public void DeleteFilter(int categoryId, int filterId)
        {
            var efCategory = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efCategory!=null)
            {
                var efFilter = Context.Filters.Find(filterId);
                efCategory.Filters.Remove(efFilter);
            }
        }

        public override void AddOrUpdate(Categories entity)
        {
            var prop = entity.GetType().GetProperty("Id");
            int id = (int)prop.GetValue(entity, null);

            var efEntity = FindBy(i=>i.Id==id).SingleOrDefault();
            if (efEntity == null)
            {
                Context.Categories.Add(entity);
            }
            else
            {
                if (entity.Filters!=null)
                {
                    efEntity.Filters.Clear();
                    foreach (var item in entity.Filters)
                    {
                        var efFilter = Context.Filters.Find(item.Id);
                        efEntity.Filters.Add(efFilter);
                    }
                }
                Context.Entry(efEntity).CurrentValues.SetValues(entity);
                Context.Entry(efEntity).State = System.Data.Entity.EntityState.Modified;
                
            }

        }

        public IEnumerable<Products> GetWithCheckedEnumValues(int categoryId,IEnumerable<EnumValues> enumValues)
        {
            var efCategory = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efCategory != null)
            {
                if (enumValues.Count() > 0)
                {


                    var idValues = enumValues.Select(s => s.Id).ToList();


                    var efEnumValuetemp = Context.EnumValues.ToList();
                    var tttest = efCategory.Products.Where(i => i.EnumValues.All(p => idValues.Contains(p.Id))).ToList();
                    var te = efEnumValuetemp.Where(i => idValues.Contains(i.Id)).ToList();
                    return tttest;
                }
                
                return efCategory.Products;
            }
            return new List<Products>();
           
        }
    }
}
