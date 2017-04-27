using Pyramid.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.DAL
{
    public class ProductDAL 
    {
       // private static readonly DataContext dbContext = new DataContext();


        public static void DeleteEntity(int entityId)
        {
            throw new NotImplementedException();
        }

        public static int AddOrUpdateEntity(Product entity)
        {
            using (DataContext dbContext = new DataContext())
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
                return entity.Id;

            }
        }
    }
}
