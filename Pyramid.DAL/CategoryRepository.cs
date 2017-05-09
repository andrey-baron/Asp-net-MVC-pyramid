using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pyramid.DAL.Entity;

namespace Pyramid.DAL
{
    public class CategoryRepository : IRepository<Category>
    {
        private DataContext dbContext;
        public CategoryRepository(DataContext context)
        {
            this.dbContext = context;
        }
        public void AddOrUpdateEntity(Category entity)
        {
            using (dbContext)
            {

                if (entity.Id == 0)
                {
                    dbContext.Categories.Add(entity);
                }
                else
                {
                    var efCategory = dbContext.Categories.Find(entity.Id);
                    dbContext.Entry(efCategory).CurrentValues.SetValues(entity);
                }
                dbContext.SaveChanges();
            }
        }

        public void Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public Category Get(int id)
        {
            return dbContext.Categories.Find(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return dbContext.Categories;
        }
    }
}
