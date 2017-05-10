using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL
{
    public class FilterDAL
    {
        public static void AddOrDefault(Pyramid.Entity.Filter filter)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                if (filter.Id==0)
                {
                    dbContext.Filters.Add(new Filters() {
                    Title=filter.Title,
                    });

                }
                else
                {
                    var efFilter = dbContext.Filters.Find(filter.Id);
                        dbContext.Entry(efFilter).CurrentValues.SetValues(filter);
                }
                dbContext.SaveChanges();
            }
            
        }
        public static void Delete(int entityId)
        {
            throw new NotImplementedException();
        }
        public static Pyramid.Entity.Filter Get(int id)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                var entity = dbContext.Filters.Find(id);
                if (entity!=null)
                {
                    return new Pyramid.Entity.Filter()
                    {
                        Id = entity.Id,
                        Title = entity.Title
                    };
                }
                return null;
            }
        }
        public static ICollection<Pyramid.Entity.Filter> GetAll()
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                return dbContext.Filters.Select(f => new Pyramid.Entity.Filter() {
                    Title = f.Title,
                    Id= f.Id
                }).ToList();
            }
        }
    }
}
