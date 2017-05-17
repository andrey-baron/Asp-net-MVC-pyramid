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
                    foreach (var item in entity.ProductValues)
                    {
                        item.ProductId = entity.Id;
                        ProductValueDAL.AddOrUpdate(entity.Id, item);
                    }
                    dbContext.Entry(efProduct).CurrentValues.SetValues(EntityToDAL(entity));

                }
                if (entity.Categories!=null)
                {
                    var catCheak = entity.Categories.Where(i => i.Cheaked == true).ToList();
                    foreach (var item in catCheak)
                    {
                        AddOrUpdateCategoryRelated(entity.Id, item, dbContext);
                    }
                   
                }
                dbContext.SaveChanges();
            }
        }
        private static void AddOrUpdateCategoryRelated(int productId,Pyramid.Entity.Category category, PyramidFinalContext dbContext )
        {
            var efProdCat = dbContext.ProductCategories.FirstOrDefault(i => i.ProductId == productId&& i.CategoryId==category.Id);
            if (efProdCat!=null)
            {
                efProdCat.ProductId = productId;
                efProdCat.CategoryId = category.Id;
            }
            else
            {
                dbContext.ProductCategories.Add(new ProductCategories() {
                CategoryId=category.Id,
                ProductId=productId
                });
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
                        Id = product.Id,
                        Title = product.Title,
                        Price = product.Price,
                        TypePrice = (Pyramid.Entity.Enumerable.TypeProductPrice)product.TypePrice,
                        Alias = product.Alias,
                        DateChange = product.DateChange,
                        DateCreation = product.DateCreation,
                        MetaDescription = product.MetaDescription,
                        MetaKeywords = product.MetaKeywords,
                        MetaTitle = product.MetaTitle,
                        ProductValues = product.ProductValues.Select(i => new Pyramid.Entity.ProductValue()
                        {
                            Id = i.Id,
                            Key = i.Key,
                            ProductId = i.ProductId,
                            Value = i.Value
                        }).ToList(),
                        Categories = product.ProductCategories.Select(c => new Pyramid.Entity.Category() {
                            Id=c.CategoryId,
                            Title=c.Categories.Title,
                            Cheaked=true
                            
                        }).ToList(),
                        ThumbnailId=product.ThumbnailId,
                        ThumbnailImg=product.Images!=null? new Pyramid.Entity.Image() {
                            Id= product.Images.Id,
                            ServerPathImg= product.Images.ServerPathImg
                        } :null,
                        
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
            var obj= new Products() {
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
                ThumbnailId=product.ThumbnailId,
            };
           
            return obj;
        }

    }
}
