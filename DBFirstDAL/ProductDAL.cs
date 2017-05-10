using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL
{
    public class ProductDAL
    {
        public static void AddOrUpdateEntity(Pyramid.Entity.Product entity)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {

                if (entity.Id == 0)
                {
                    dbContext.Products.Add(new Products() {
                        Title=entity.Title,
                        DateChange= entity.DateChange,
                        DateCreation= entity.DateCreation,
                        Price=entity.Price,
                        TypePrice= (int) entity.TypePrice,
                        
                    });
                }
                else
                {
                    var efProduct = dbContext.Products.Find(entity.Id);
                    dbContext.Entry(efProduct).CurrentValues.SetValues(EntityToDAL(entity));
                }
                dbContext.SaveChanges();
            }
        }

        public static void Delete(int entityId)
        {
            throw new NotImplementedException();
        }

        public static Pyramid.Entity.Product Get(int id)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {

                var product = dbContext.Products.Find(id);
                if (product!=null)
                {
                    return new Pyramid.Entity.Product()
                    {
                        Id=product.Id,
                        Title=product.Title,
                        Price= product.Price,
                        TypePrice=(Pyramid.Entity.Enumerable.TypeProductPrice)product.TypePrice,
                        Alias = product.Alias,
                        DateChange = product.DateChange,
                        DateCreation = product.DateCreation,
                        MetaDescription = product.MetaDescription,
                        MetaKeywords = product.MetaKeywords,
                        MetaTitle = product.MetaTitle,
                    };
                }
                return null;
            }
        }

        public static IEnumerable<Pyramid.Entity.Product> GetAll()
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                return dbContext.Products.Select(product => new Pyramid.Entity.Product() {
                    Id = product.Id,
                    Title = product.Title,
                    Price = product.Price,
                    TypePrice = (Pyramid.Entity.Enumerable.TypeProductPrice)product.TypePrice,
                    Alias=product.Alias,
                    DateChange= product.DateChange,
                    DateCreation= product.DateCreation,
                    MetaDescription=product.MetaDescription,
                    MetaKeywords=product.MetaKeywords,
                    MetaTitle=product.MetaTitle,
                }).ToList();
                
            }
        }

        private static Products EntityToDAL(Pyramid.Entity.Product product)
        {
            return new Products() {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                TypePrice = (int)product.TypePrice,
                Alias = product.Alias,
                DateChange = product.DateChange,
                DateCreation = product.DateCreation,
                MetaDescription = product.MetaDescription,
                MetaKeywords = product.MetaKeywords,
                MetaTitle = product.MetaTitle,
            };
        }

    }
}
