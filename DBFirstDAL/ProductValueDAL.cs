using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL
{
    public class ProductValueDAL
    {
        public  static int GetCountByProductId(int productId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
               
                int amount = dbContext.ProductValues.Where(i => i.ProductId == productId).Count();
                return amount;
            }
        }
        public static ICollection<Pyramid.Entity.ProductValue> GetAll(int productId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                return dbContext.ProductValues.Where(p=>p.ProductId == productId).Select(i => new Pyramid.Entity.ProductValue() {
                Id=i.Id,
                Key=i.Key,
                ProductId= i.ProductId,
                Value=i.Value
                }).ToList();
            }
        }
        public static void AddOrUpdate(int productId, Pyramid.Entity.ProductValue prValue)
        {
            if (productId!=0&& prValue!=null)
            {
                using (PyramidFinalContext dbContext = new PyramidFinalContext())
                {
                    if (prValue.Id==0)
                    {
                        dbContext.ProductValues.Add(new ProductValues()
                        {
                            Id = prValue.Id,
                            Key = prValue.Key,
                            ProductId = productId,
                            Value = prValue.Value
                        });
                    }
                    else
                    {
                        var efEntiry = dbContext.ProductValues.Find(prValue.Id);
                        if (efEntiry!=null)
                        {
                            dbContext.Entry(efEntiry).CurrentValues.SetValues(prValue);
                        }
                    }
                    dbContext.SaveChanges();
                }
            }
        }

        public static void Delete(int id)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                var efEntity = dbContext.ProductValues.Find(id);
                if (efEntity!=null)
                {
                    dbContext.ProductValues.Remove(efEntity);
                    dbContext.SaveChanges();
                }
            }
            
        }
    }
}
