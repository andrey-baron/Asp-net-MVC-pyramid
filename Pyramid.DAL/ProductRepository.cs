using Pyramid.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL
{
    public class ProductRepository:IRepository<Product>
    {
        private DataContext dbContext;

        public ProductRepository(DataContext context)
        {
            this.dbContext = context;
        }
        public void Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public void AddOrUpdateEntity(Product entity)
        {
            using (dbContext)
            {

                if (entity.Id == 0)
                {
                    dbContext.Products.Add(entity);
                }
                else
                {
                    var efProduct = dbContext.Products.Find(entity.Id);
                    dbContext.Entry(efProduct).CurrentValues.SetValues(entity);
                }
                dbContext.SaveChanges();
            }
        }

        public IEnumerable<Product> GetAll()
        {
            return dbContext.Products;
        }

        public Product Get(int id)
        {
            var t1 = dbContext.Products.FirstOrDefault(k => k.Id == id);
            var t2= dbContext.Products.Find(id);
            return dbContext.Products.Find(id);
        }
    }
}
