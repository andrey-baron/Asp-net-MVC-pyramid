using DBFirstDAL.DataModels.HomeModels;
using DBFirstDAL.DataModels.ProductModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using DBFirstDAL.DataModels._1C;
using Common.SearchClasses;
using Pyramid.Entity;
using System.Linq.Expressions;
using Common.Models;

namespace DBFirstDAL.Repositories
{
    public class ProductRepository: GenericRepository<Products,PyramidFinalContext,Pyramid.Entity.Product, SearchParamsProduct,int>
    {
       const string defaulProductLink = "/Product/index/";
        const string defaulCateggorytLink = "/Category/index/";
        public IEnumerable<EnumValue> GetAllEnumValues(int productId)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                var product = dbContext.Products.Find(productId);
                if (product != null)
                {
                    return product.EnumValues.Select(s=>new EnumValue {
                        Id=s.Id,
                        Key=s.Key,
                        TypeValue=(Common.TypeFromEnumValue)s.TypeValue
                    }). ToList();
                }
                return new List<EnumValue>();
            }
            
        }

        public void DeleteEnumValue(int id, int enumValueId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var product = dbContext.Products.Find(id);
                if (product != null)
                {
                    var enumvalue = dbContext.EnumValues.Find(enumValueId);
                    if (enumvalue!=null)
                    {
                        product.EnumValues.Remove(enumvalue);
                        dbContext.SaveChanges();
                    }
                }
            }
        }

        //public void DeleteRecomendation(int id, int recomendationId)
        //{
        //    using (PyramidFinalContext dbContext = new PyramidFinalContext())
        //    {
        //        var product = dbContext.Products.Find(id);
        //        if (product != null)
        //        {
        //            var recomend = dbContext.Recommendations.Find(recomendationId);
        //            if (recomend != null)
        //            {
        //                product.Recommendations.Remove(recomend);
        //                dbContext.SaveChanges();
        //            }
        //        }
        //    }
        //}

        //public override void AddOrUpdate(Products entity)
        //{
        //    var prop = entity.GetType().GetProperty("Id");
        //    int id = (int)prop.GetValue(entity, null);

        //    var efEntity = Context.Products.Find(id);
        //    if (efEntity == null)
        //    {
        //        var tmpEnumval = new List<EnumValues>(entity.EnumValues);
        //        var tmpPrVal = new List<ProductValues>(entity.ProductValues);
        //        var tmpCat = new List<Categories>(entity.Categories);
        //        entity.EnumValues.Clear();
        //        entity.ProductValues.Clear();
        //        entity.Categories.Clear();
        //        Context.Products.Add(entity);

        //        Context.SaveChanges();
        //        foreach (var item in tmpEnumval)
        //        {
        //            var efitem = Context.EnumValues.Find(item.Id);
        //            if (efitem != null)
        //            {
        //                entity.EnumValues.Add(efitem);
        //            }
        //        }
        //        foreach (var item in tmpPrVal)
        //        {
        //            entity.ProductValues.Add(new ProductValues()
        //            {
        //                Key = item.Key,
        //                Value = item.Value,
        //                ProductId = item.ProductId
        //            });
        //        }
        //        foreach (var item in tmpCat)
        //        {
        //            var efCategory = Context.Categories.Find(item.Id);
        //            entity.Categories.Add(efCategory);
        //        }
        //        Context.SaveChanges();
        //    }
        //    else
        //    {

        //        Context.Entry(efEntity).CurrentValues.SetValues(entity);
        //        Context.Entry(efEntity).State = System.Data.Entity.EntityState.Modified;

        //        Context.SaveChanges();


        //        efEntity.Categories.Clear();
        //        foreach (var item in entity.Categories)
        //        {
        //            var efCategory = Context.Categories.Find(item.Id);
        //            efEntity.Categories.Add(efCategory);
        //        }
        //        Context.SaveChanges();
        //        efEntity.EnumValues.Clear();

        //        foreach (var item in entity.EnumValues)
        //        {
        //            var efEnumValues = Context.EnumValues.Find(item.Id);
        //            if (efEnumValues != null)
        //            {
        //                efEntity.EnumValues.Add(efEnumValues);
        //            }

        //        }
        //        Context.SaveChanges();

        //        foreach (var item in entity.ProductValues)
        //        {
        //            var efprval = Context.ProductValues.FirstOrDefault(i => i.Id == item.Id);
        //            if (efprval == null)
        //            {
        //                efprval = new ProductValues()
        //                {
        //                    Key = item.Key,
        //                    Value = item.Value,
        //                    ProductId = efEntity.Id
        //                };
        //                Context.ProductValues.Add(efprval);
        //            }
        //            else
        //            {
        //                efprval.Key = item.Key;
        //                efprval.Value = item.Value;
        //            }



        //        }
        //        Context.SaveChanges();

        //    }
        //    if (entity.ProductImages != null)
        //    {
        //        var thumbnail = entity.ProductImages.FirstOrDefault(i => i.TypeImage == (int)Pyramid.Entity.Enumerable.TypeImage.Thumbnail);
        //        if (thumbnail != null)
        //        {
        //            var efThumbnail = Context.ProductImages.FirstOrDefault(i => i.ImageId == thumbnail.ImageId);
        //            if (efThumbnail != null)
        //            {
        //                Context.ProductImages.Remove(efThumbnail);
        //            }

        //            Context.ProductImages.Add(new ProductImages()
        //            {
        //                ImageId = thumbnail.ImageId,
        //                ProductId = thumbnail.ProductId,
        //                TypeImage = thumbnail.TypeImage
        //            });
        //        }

        //    }

        //}

        public Images GetThumbnail(int productId, int typeThumbnail )
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                return dbContext.Images.FirstOrDefault(i => i.ProductImages.Any(f => f.ProductId == productId && f.TypeImage == typeThumbnail));
            }
        }

        public IEnumerable<Pyramid.Entity.Image> GetGalleryImage(int productId, int typeGallery)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                return dbContext.Images
                    .Where(i => i.ProductImages.Any(f => f.ProductId == productId && f.TypeImage == typeGallery))
                    .ToList()
                    .Select(s=>Convert.ConvertImageToEntity.Convert(s));

            }
        }
        public void AddToGallry(int productId,int imageId,int typeImage)
        {
            using (PyramidFinalContext dbContext=new PyramidFinalContext())
            {
                var efProduct = dbContext.Products.Find(productId);
                if (efProduct != null)
                {
                    var efImage = efProduct.ProductImages.FirstOrDefault(i => i.ImageId == imageId && i.TypeImage == typeImage);
                    if (efImage == null)
                    {
                        efProduct.ProductImages.Add(new ProductImages()
                        {
                            ProductId = productId,
                            ImageId = imageId,
                            TypeImage = typeImage
                        });
                        dbContext.SaveChanges();
                    }
                }
            }
           
            
        }

        public void RemoveToGallry(int productId, int imageId, int typeImage)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efProduct = dbContext.Products.Find(productId);
                if (efProduct != null)
                {
                    var efImage = dbContext.ProductImages.FirstOrDefault(i => i.ImageId == imageId 
                    && i.TypeImage == typeImage
                    && i.ProductId==efProduct.Id);
                    if (efImage != null)
                    {
                        dbContext.ProductImages.Remove(efImage);
                        dbContext.SaveChanges();
                    }
                }
            }
           

        }

        //public IEnumerable<Review> GetReview(int id)
        //{
        //    var efProduct = FindBy(i => i.Id == id).SingleOrDefault();
        //    return efProduct.Review;
        //}
        
        public IEnumerable<Product> GetSeasonOffers(int typeThumbnail)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                return dbContext.Products.Where(i => i.SeasonOffer == true && i.TypeStatusProduct!=(int)Common.TypeStatusProduct.Hide ).ToList().Select(i => ConvertDbObjectToEntityShort(dbContext, i)).ToList();
            }
           
        }

        public IQueryable<Products> GetAllWithThumbnail(int typeThumbnail)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                return dbContext.Products.Include(i => i.ProductImages.Select(s => s.Images));
            }
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
        //public  void InsertsOrNot(IEnumerable<Products> entities)
        //{
        //    // Настройки контекста
        //    using (PyramidFinalContext context = new PyramidFinalContext())
        //    {
        //        // Отключаем отслеживание и проверку изменений для оптимизации вставки множества полей
        //        context.Configuration.AutoDetectChangesEnabled = false;
        //        context.Configuration.ValidateOnSaveEnabled = false;

        //        context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));


        //        foreach (var entity in entities)
        //        {
        //            var efPr = context.Products.FirstOrDefault(f => f.OneCId == entity.OneCId);
        //            if (efPr == null)
        //            {
        //                context.Entry(entity).State = EntityState.Added;
        //            }
        //        }

        //        context.SaveChanges();

        //        context.Configuration.AutoDetectChangesEnabled = true;
        //        context.Configuration.ValidateOnSaveEnabled = true;
        //    } 

           
        //}

        public void AddOrUpdateFromOneC(Products product)
        {
            using (PyramidFinalContext dbContext=new PyramidFinalContext())
            {
                var dbProduct = dbContext.Products.FirstOrDefault(i => i.OneCId == product.OneCId);
               
                var exist = dbProduct != null;
                if (!exist)
                {
                    dbProduct = new Products();
                }
                UpdateBeforeSavingOceC(dbContext, dbProduct, product, exist);
                dbContext.SaveChanges();

                UpdateAfterSavingOneC(dbContext, dbProduct, product, exist);
                dbContext.SaveChanges();
            }

        }

        public void UpdateBeforeSavingOceC(PyramidFinalContext dbContext,Products dbObjext, Products model,bool exist)
        {
            dbObjext.DateChange = DateTime.Now;
            dbObjext.DateCreation = DateTime.Now;
            dbObjext.IsFilled = model.IsFilled;
            dbObjext.IsPriority = model.IsPriority;
            dbObjext.Title = model.Title;
            dbObjext.TypePrice = model.TypePrice;
            dbObjext.OneCId = model.OneCId;
            dbObjext.Price = model.Price;
            dbObjext.TypeStatusProduct = model.TypeStatusProduct;
            dbObjext.IsNotUnloading1C = model.IsNotUnloading1C;
            if (!exist)
            {
                dbContext.Products.Add(dbObjext);
            }
           
        }
        public void UpdateAfterSavingOneC(PyramidFinalContext dbContext, Products dbObjext, Products model, bool exist)
        {
            if (model.Categories != null)
            {
                foreach (var item in model.Categories)
                {
                    var efCat = dbContext.Categories.FirstOrDefault(f => f.OneCId == item.OneCId);
                    if (efCat != null)
                    {
                        if (!dbObjext.Categories.Contains(efCat))
                        {
                            dbObjext.Categories.Add(efCat);
                        }

                    }

                }
            }
            //переписать в allowable
            if (model.EnumValues != null)
            {
                var firstEnumValue = model.EnumValues.FirstOrDefault();
                if (firstEnumValue != null&& firstEnumValue.Key!=null)
                {
                    var enumValueFromProduct = dbObjext.EnumValues.FirstOrDefault(f => f.Key == firstEnumValue.Key);
                    if (enumValueFromProduct == null)
                    {
                        var listBrandsFromProduct = new List<EnumValues>( dbObjext.EnumValues.Where(i => i.TypeValue == (int)Common.TypeFromEnumValue.Brand));
                        if (listBrandsFromProduct.Count>0)
                        {
                            foreach (var item in listBrandsFromProduct)
                            {
                                dbObjext.EnumValues.Remove(item);
                            }
                        }
                        dbContext.SaveChanges();
                        var dbEnumValue = dbContext.EnumValues.Where(i => i.Key == firstEnumValue.Key).Distinct().SingleOrDefault();
                        if (dbEnumValue != null)
                        {
                            dbObjext.EnumValues.Add(dbEnumValue);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            var newEnumValue = new EnumValues() {
                                Key = firstEnumValue.Key,
                                TypeValue =(int)Common.TypeFromEnumValue.Brand
                            };
                            dbContext.EnumValues.Add(newEnumValue);
                            dbContext.SaveChanges();
                            dbObjext.EnumValues.Add(newEnumValue);
                        }
                    }
                    
                }
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

        protected override Products GetDbObjectByEntity(DbSet<Products> objects, Product entity)
        {
           return objects.FirstOrDefault(f => f.Id == entity.Id);
        }

        protected override Expression<Func<Products, int>> GetIdByDbObjectExpression()
        {
            return item => item.Id;
        }

        public override Product ConvertDbObjectToEntity(PyramidFinalContext context, Products dbObject)
        {
            var product = new Product()
            {
                Alias=dbObject.Alias,
                TypeStatusProduct=(Common.TypeStatusProduct) dbObject.TypeStatusProduct,
                Id=dbObject.Id,
                DateChange=dbObject.DateChange,
                DateCreation=dbObject.DateCreation,
                IsFilled=dbObject.IsFilled,
                IsPriority=dbObject.IsPriority,
                IsSEOReady= dbObject.IsSEOReady,
                MetaDescription=dbObject.MetaDescription,
                MetaKeywords=dbObject.MetaKeywords,
                MetaTitle=dbObject.MetaTitle,
                PopularCount=dbObject.PopularCount.HasValue ? dbObject.PopularCount.Value : 0,
                OneCId=dbObject.OneCId,
                Price=dbObject.Price,
                SeasonOffer= dbObject.SeasonOffer.HasValue ? dbObject.SeasonOffer.Value:false,
                Title=dbObject.Title,
                TypePrice=(Common.TypeProductPrice) dbObject.TypePrice,
                Content = dbObject.Content,
                IsNotUnloading1C=dbObject.IsNotUnloading1C,
                Categories =dbObject.Categories.Select(s=>new Category() {
                    Title=s.Title,
                    Id=s.Id
                }).ToList(),
                EnumValues=dbObject.EnumValues.Select(s => new EnumValue()
                {
                    Key = s.Key,
                    TypeValue=(Common.TypeFromEnumValue) s.TypeValue,
                    Id = s.Id,
                    
                }).ToList(),
               Images=dbObject.ProductImages.Where(w=>w.TypeImage==(int)Common.TypeImage.GalleryItem&&w.ProductId==dbObject.Id).Select(i=>i.Images).Select(i=>new Image {
                   Id=i.Id,
                   ImgAlt=i.ImgAlt,
                   PathInFileSystem=i.PathInFileSystem,
                   ServerPathImg=i.ServerPathImg,
                   Title=i.Title
               }).ToList() ,
               ProductValues=dbObject.ProductValues.Select(i=>new ProductValue
               {
                   Id=i.Id,
                   Key=i.Key,
                   ProductId=i.ProductId,
                   Value=i.Value
               }).ToList(),
               Reviews= dbObject.Reviews.Where(w=>w.IsApproved).Select(i=>new Pyramid.Entity.Review
               {
                   Content=i.Content,
                   DateCreation=i.DateCreation,
                   Name=i.Name,
                   Rating=i.Rating
               }).ToList(),
               //Recommendations=dbObject.Recommendations.Select(i=>new Recommendation
               //{
               //    Id=i.Id,
               //    Title=i.Title
               //}).ToList(),
               ThumbnailImg=dbObject.ProductImages.FirstOrDefault(f=>f.ProductId==dbObject.Id&&f.TypeImage==(int)Common.TypeImage.Thumbnail)!=null?
               Convert.ConvertImageToEntity.Convert(dbObject.ProductImages.FirstOrDefault(f => f.ProductId == dbObject.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images):new Image()
               
            };
            return product;

        }

        protected override Product ConvertDbObjectToEntityShort(PyramidFinalContext context, Products dbObject)
        {
            var product = new Product()
            {
                TypeStatusProduct = (Common.TypeStatusProduct)dbObject.TypeStatusProduct,
                Id = dbObject.Id,
                DateChange = dbObject.DateChange,
                DateCreation = dbObject.DateCreation,
                IsFilled = dbObject.IsFilled,
                IsPriority = dbObject.IsPriority,
                IsSEOReady = dbObject.IsSEOReady,
                Price = dbObject.Price,
                SeasonOffer = dbObject.SeasonOffer.HasValue ? dbObject.SeasonOffer.Value : false,
                Title = dbObject.Title,
                TypePrice = (Common.TypeProductPrice)dbObject.TypePrice,
                ThumbnailImg = dbObject.ProductImages.FirstOrDefault(f => f.ProductId == dbObject.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
               Convert.ConvertImageToEntity.Convert(dbObject.ProductImages.FirstOrDefault(f => f.ProductId == dbObject.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images) : new Image()
            };
            return product;
        }
        protected  Product ConvertDbObjectToEntityShortWithCategory(PyramidFinalContext context, Products dbObject)
        {
            var product = new Product()
            {
                TypeStatusProduct = (Common.TypeStatusProduct)dbObject.TypeStatusProduct,
                Id = dbObject.Id,
                DateChange = dbObject.DateChange,
                DateCreation = dbObject.DateCreation,
                IsFilled = dbObject.IsFilled,
                IsPriority = dbObject.IsPriority,
                IsSEOReady = dbObject.IsSEOReady,
                Price = dbObject.Price,
                SeasonOffer = dbObject.SeasonOffer.HasValue ? dbObject.SeasonOffer.Value : false,
                Title = dbObject.Title,
                TypePrice = (Common.TypeProductPrice)dbObject.TypePrice,
                ThumbnailImg = dbObject.ProductImages.FirstOrDefault(f => f.ProductId == dbObject.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
               Convert.ConvertImageToEntity.Convert(dbObject.ProductImages.FirstOrDefault(f => f.ProductId == dbObject.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images) : new Image(),
                Categories = dbObject.Categories.Select(s => new Category()
                {
                    Title = s.Title,
                    Id = s.Id
                }).ToList(),
            };
            return product;
        }

        protected override IQueryable<Products> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Products> dbObjects, SearchParamsProduct searchParams)
        {
            if (searchParams.IsSearchOnlyPublicProduct!=null&& searchParams.IsSearchOnlyPublicProduct.Value)
            {
                dbObjects = dbObjects.Where(i => i.TypeStatusProduct!=(int)Common.TypeStatusProduct.Hide);
            }
            if (searchParams.CategoryId!=null)
            {
                dbObjects= dbObjects.Where(i => i.Categories.Any(a => a.Id == searchParams.CategoryId.Value));
            }
            if (searchParams.SearchString!=null)
            {
                dbObjects = dbObjects.Where(i=>i.Title.Contains(searchParams.SearchString));
            }
            if (searchParams.Priority.HasValue&& searchParams.Priority.Value)
            {
                dbObjects = dbObjects.Where(i=>i.IsPriority==true && searchParams.Priority.Value);
            }
            if (searchParams.Filled.HasValue && searchParams.Filled.Value!=0)
            {
                if (searchParams.Filled.Value==(int)Common.TypeFilledProduct.IsFilled)
                {
                    dbObjects = dbObjects.Where(i => i.IsFilled == true);
                }
                else
                {
                    dbObjects = dbObjects.Where(i => i.IsFilled == false);

                }
            }
            if (searchParams.IsNotUnloading1C.HasValue&& searchParams.IsNotUnloading1C.Value)
            {
                dbObjects = dbObjects.Where(s => s.IsNotUnloading1C == searchParams.IsNotUnloading1C.Value);
            }
            dbObjects = dbObjects.OrderByDescending(item => item.Id);
            return dbObjects;
        }

       
        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Products dbEntity, Product entity, bool exists)
        {
            dbEntity.Alias = entity.Alias;
            dbEntity.DateChange = entity.DateChange;
            dbEntity.DateCreation = entity.DateCreation;
            dbEntity.IsFilled = entity.IsFilled;
            dbEntity.IsPriority = entity.IsPriority;
            dbEntity.IsSEOReady = entity.IsSEOReady;
            dbEntity.MetaDescription = entity.MetaDescription;
            dbEntity.MetaKeywords = entity.MetaKeywords;
            dbEntity.MetaTitle = entity.MetaTitle;
            dbEntity.Price = entity.Price;
            dbEntity.SeasonOffer = entity.SeasonOffer;
            dbEntity.Title = entity.Title;
            dbEntity.TypePrice = (int)entity.TypePrice;
            dbEntity.TypeStatusProduct = (int)entity.TypeStatusProduct;
            dbEntity.Content = entity.Content;
            dbEntity.IsNotUnloading1C = entity.IsNotUnloading1C;
        }
        public override void UpdateAfterSaving(PyramidFinalContext dbContext, Products dbEntity, Product entity, bool exists)
        {
            dbEntity.Categories.Clear();
            foreach (var item in entity.Categories)
            {
                var efCategory = dbContext.Categories.Find(item.Id);
                dbEntity.Categories.Add(efCategory);
            }
            //dbEntity.Recommendations.Clear();
            //foreach (var item in entity.Recommendations)
            //{
            //    var efRecommendation = dbContext.Recommendations.Find(item.Id);
            //    dbEntity.Recommendations.Add(efRecommendation);
            //}
            //Context.SaveChanges();
            dbEntity.EnumValues.Clear();

            foreach (var item in entity.EnumValues)
            {
                var efEnumValues = dbContext.EnumValues.Find(item.Id);
                if (efEnumValues != null)
                {
                    dbEntity.EnumValues.Add(efEnumValues);
                }

            }
           // Context.SaveChanges();

            foreach (var item in entity.ProductValues)
            {
                var efprval = dbContext.ProductValues.FirstOrDefault(i => i.Id == item.Id);
                if (efprval == null)
                {
                    efprval = new ProductValues()
                    {
                        Key = item.Key,
                        Value = item.Value,
                        ProductId = dbEntity.Id
                    };
                    dbContext.ProductValues.Add(efprval);
                }
                else
                {
                    efprval.Key = item.Key;
                    efprval.Value = item.Value;
                }



            }
           // dbContext.SaveChanges();

        
            if (entity.ThumbnailId != 0)
            {
                var curThumbnail = dbEntity.ProductImages.FirstOrDefault(i => i.ProductId == dbEntity.Id && i.TypeImage == (int)Pyramid.Entity.Enumerable.TypeImage.Thumbnail);
                if (curThumbnail != null)
                {
                    var efThumbnail = dbContext.Images.FirstOrDefault(i => i.Id == entity.ThumbnailId);
                    if (efThumbnail != null)
                    {
                        if (efThumbnail.Id!=curThumbnail.Id)
                        {
                            dbContext.ProductImages.Remove(curThumbnail);
                            dbContext.ProductImages.Add(new ProductImages()
                            {
                                ImageId = entity.ThumbnailId,
                                ProductId = dbEntity.Id,
                                TypeImage = (int)Common.TypeImage.Thumbnail,
                            });
                        }
                    }
                }
                else
                {
                    dbContext.ProductImages.Add(new ProductImages()
                    {
                        ImageId = entity.ThumbnailId,
                        ProductId = dbEntity.Id,
                        TypeImage = (int)Common.TypeImage.Thumbnail,
                    });

                }

            }
        }

        public IEnumerable<Common.Models.BreadCrumbViewModel> GetBreadCrumbs(int productId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efProduct = dbContext.Products.Find(productId);
                List<BreadCrumbViewModel> breadcrumbs = new List<BreadCrumbViewModel>();
                if (efProduct != null)
                {
                    breadcrumbs.Add(new BreadCrumbViewModel()
                    {
                        Title = efProduct.Title
                    ,
                        //Link = defaulCateggorytLink + EfModel.Id.ToString()
                    });
                    var flagstop = true;
                    var cat = efProduct.Categories.FirstOrDefault();
                    var listIds = new List<int>();
                    while (flagstop)
                    {

                        if (cat != null)
                        {
                            if (listIds.Contains(cat.Id))
                            {
                                break;
                            }
                            breadcrumbs.Add(new BreadCrumbViewModel()
                            {
                                Title = cat.Title,
                                Link = defaulCateggorytLink + cat.Id.ToString()
                            });
                            
                            listIds.Add(cat.Id);
                            if (cat.ParentId == null)
                            {

                                flagstop = false;
                            }
                            else
                            {
                                cat = cat.Categories2;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    breadcrumbs.Reverse();
                }
                return breadcrumbs;
            }
        }

        public IEnumerable<Product> RelatedProducts(int productId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efProduct = dbContext.Products.Find(productId);
                var cat = efProduct.Categories.FirstOrDefault();
                if (cat!=null)
                {
                    return cat.Products.Where(i => i.Id != productId&& i.TypeStatusProduct!=(int)Common.TypeStatusProduct.Hide).Select(s => ConvertDbObjectToEntityShort(dbContext, s)).ToList();
                }
                else
                {
                    return new List<Product>();
                }
            }
        }

        public IEnumerable<Product> GetAllWithCategory()
        {
            using (PyramidFinalContext data = new PyramidFinalContext())
            {
                IQueryable<Products> query = data.Set<Products>().AsNoTracking();
                var entityObjectList = query.ToList().Select(s => ConvertDbObjectToEntityShortWithCategory(data, s)).ToList();
                return entityObjectList;
            }
        }

        public void SetIsNotUnloading(ICollection<Products> xmlproducts)
        {
            using (PyramidFinalContext dbContext =new PyramidFinalContext())
            {
                var xmlIds1c = xmlproducts.Select(s => s.OneCId);
                var objects = dbContext.Products;
                foreach (var item in objects)
                {
                    if (!xmlIds1c.Contains(item.OneCId))
                    {
                        item.IsNotUnloading1C = true;
                       
                    }
                }
                dbContext.SaveChanges();

            }
        }
    }
}
