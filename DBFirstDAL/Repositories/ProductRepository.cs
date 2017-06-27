using DBFirstDAL.DataModels.HomeModels;
using DBFirstDAL.DataModels.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DBFirstDAL.DataModels._1C;

namespace DBFirstDAL.Repositories
{
    public class ProductRepository: GenericRepository<PyramidFinalContext, Products>
    {
        public IEnumerable<EnumValues> GetAllEnumValues(int productId)
        {
            var filter = FindBy(i => i.Id == productId).SingleOrDefault();
            if (filter != null)
            {
                return filter.EnumValues.ToList();
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

        public override void AddOrUpdate(Products entity)
        {
            var prop = entity.GetType().GetProperty("Id");
            int id = (int)prop.GetValue(entity, null);

            var efEntity = Context.Products.Find(id);
            if (efEntity == null)
            {
                var tmpEnumval = new List<EnumValues>(entity.EnumValues);
                var tmpPrVal = new List<ProductValues>(entity.ProductValues);
                var tmpCat = new List<Categories>(entity.Categories);
                entity.EnumValues.Clear();
                entity.ProductValues.Clear();
                entity.Categories.Clear();
                Context.Products.Add(entity);
                
                Context.SaveChanges();
                foreach (var item in tmpEnumval)
                {
                    var efitem = Context.EnumValues.Find(item.Id);
                    if (efitem!=null)
                    {
                        entity.EnumValues.Add(efitem);
                    }
                }
                foreach (var item in tmpPrVal)
                {
                    entity.ProductValues.Add(new ProductValues()
                    {
                        Key = item.Key,
                        Value = item.Value,
                        ProductId = item.ProductId
                    });
                }
                foreach (var item in tmpCat)
                {
                    var efCategory = Context.Categories.Find(item.Id);
                    entity.Categories.Add(efCategory);
                }
                Context.SaveChanges();
            }
            else
            {

                Context.Entry(efEntity).CurrentValues.SetValues(entity);
                Context.Entry(efEntity).State = System.Data.Entity.EntityState.Modified;

                Context.SaveChanges();
                
                
                efEntity.Categories.Clear();
                foreach (var item in entity.Categories)
                {
                    var efCategory = Context.Categories.Find(item.Id);
                    efEntity.Categories.Add(efCategory);
                }
                Context.SaveChanges();
                efEntity.EnumValues.Clear();
                
                foreach (var item in entity.EnumValues)
                {
                    var efEnumValues = Context.EnumValues.Find(item.Id);
                    if (efEnumValues!=null)
                    {
                        efEntity.EnumValues.Add(efEnumValues);
                    }
                    
                }
                Context.SaveChanges();
              
                foreach (var item in entity.ProductValues)
                {
                    var efprval = Context.ProductValues.FirstOrDefault(i => i.Id == item.Id);
                    if (efprval == null)
                    {
                        efprval = new ProductValues()
                        {
                            Key = item.Key,
                            Value = item.Value,
                            ProductId = efEntity.Id
                        };
                        Context.ProductValues.Add(efprval);
                    }
                    else
                    {
                        efprval.Key = item.Key;
                        efprval.Value = item.Value;
                    }


                    
                }
                Context.SaveChanges();

            }
            if (entity.ProductImages!=null)
            {
                var thumbnail = entity.ProductImages.FirstOrDefault(i => i.TypeImage == (int)Pyramid.Entity.Enumerable.TypeImage.Thumbnail);
                if (thumbnail != null)
                {
                   var efThumbnail= Context.ProductImages.FirstOrDefault(i => i.ImageId == thumbnail.ImageId);
                    if (efThumbnail!=null)
                    {
                        Context.ProductImages.Remove(efThumbnail);
                    }
                   
                    Context.ProductImages.Add(new ProductImages() {
                        ImageId =thumbnail.ImageId,
                        ProductId=thumbnail.ProductId,
                        TypeImage=thumbnail.TypeImage
                    });
                }
                
            }

        }

        public Images GetThumbnail(int productId, int typeThumbnail )
        {
            return Context.Images.FirstOrDefault(i => i.ProductImages.Any(f => f.ProductId == productId && f.TypeImage == typeThumbnail));
        }

        public IEnumerable<Images> GetGalleryImage(int productId, int typeGallery)
        {
            return Context.Images.Where(i => i.ProductImages.Any(f => f.ProductId == productId && f.TypeImage == typeGallery));
        }
        public void AddToGallry(int productId,int imageId,int typeImage)
        {
            var efProduct = FindBy(i => i.Id == productId).SingleOrDefault();
            if (efProduct!=null)
            {
                var efImage=efProduct.ProductImages.FirstOrDefault(i => i.ImageId == imageId && i.TypeImage == typeImage);
                if (efImage == null)
                {
                    efProduct.ProductImages.Add(new ProductImages()
                    {
                        ProductId = productId,
                        ImageId = imageId,
                        TypeImage = typeImage
                    });
                }
            }
            
        }

        public void RemoveToGallry(int productId, int imageId, int typeImage)
        {
            var efProduct = FindBy(i => i.Id == productId).SingleOrDefault();
            if (efProduct != null)
            {
                var efImage = efProduct.ProductImages.FirstOrDefault(i => i.ImageId == imageId && i.TypeImage == typeImage);
                if (efImage != null)
                {
                    efProduct.ProductImages.Remove(efImage);
                }
            }

        }

        public IEnumerable<Review> GetReview(int id)
        {
            var efProduct = FindBy(i => i.Id == id).SingleOrDefault();
            return efProduct.Review;
        }
        
        public IEnumerable<ProductHomeModel> GetSeasonOffers(int typeThumbnail)
        {
            return FindBy(i => i.SeasonOffer == true).Select(i=>new ProductHomeModel() {
                Id=i.Id,
                ThumbnailImg=i.ProductImages.FirstOrDefault(f=>f.TypeImage==typeThumbnail&&f.ProductId==i.Id)!=null? i.ProductImages.FirstOrDefault(f => f.TypeImage == typeThumbnail && f.ProductId == i.Id).Images:null,
                InStock=(bool)i.InStock,
                Price=i.Price,
                SeasonOffer=(bool)i.SeasonOffer,
                Title=i.Title,
                TypePrice=i.TypePrice
            }).ToList() ;
        }

        public IQueryable<Products> GetAllWithThumbnail(int typeThumbnail)
        {
            return Context.Products.Include(i => i.ProductImages.Select(s => s.Images));
        }

        public void EnhancementPopularField(int productId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efProduct=dbContext.Products.Find(productId);
                if (efProduct!= null)
                {
                    efProduct.PopularCount = efProduct.PopularCount.HasValue ? efProduct.PopularCount + 1 : 1;
                    dbContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Запись нескольких полей в БД
        /// </summary>
        public  void InsertsOrNot(IEnumerable<Products> entities)
        {
            // Настройки контекста
            using (PyramidFinalContext context = new PyramidFinalContext())
            {
                // Отключаем отслеживание и проверку изменений для оптимизации вставки множества полей
                context.Configuration.AutoDetectChangesEnabled = false;
                context.Configuration.ValidateOnSaveEnabled = false;

                context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));


                foreach (var entity in entities)
                {
                    var efPr = context.Products.FirstOrDefault(f => f.OneCId == entity.OneCId);
                    if (efPr == null)
                    {
                        context.Entry(entity).State = EntityState.Added;
                    }
                }

                context.SaveChanges();

                context.Configuration.AutoDetectChangesEnabled = true;
                context.Configuration.ValidateOnSaveEnabled = true;
            } 

           
        }

        public void UpdateReletedCategoriesFromProducts(IEnumerable<Product1cIdWith1cCategoryIds> productsWithCategories)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                foreach (var item in productsWithCategories)
                {
                    var efProduct=dbContext.Products.FirstOrDefault(f => f.OneCId == item.OneCId);
                    if (efProduct!=null)
                    {
                        foreach (var cat in item.CategoryIds)
                        {
                            var efCat = dbContext.Categories.FirstOrDefault(f => f.OneCId == cat);
                            if (efCat!=null)
                            {
                                efProduct.Categories.Add(efCat);
                            }
                        }
                    }
                }
                dbContext.SaveChanges();
               
            }
        }

       
    }
}
