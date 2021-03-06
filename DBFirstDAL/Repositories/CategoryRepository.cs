﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBFirstDAL;
using DBFirstDAL.DataModels;
using System.Data.Entity;
using DBFirstDAL.DataModels._1C;
using Common.SearchClasses;
using System.Linq.Expressions;
using Pyramid.Entity;
using DBFirstDAL.Convert;
using Common.Models;

namespace DBFirstDAL.Repositories
{
    public class CategoryRepository:GenericRepository<Categories, PyramidFinalContext,Pyramid.Entity.Category, SearchParamsCategory,int>
    {
        const string defaulCateggorytLink = "/Category/index/";

        public CategoryRepository(PyramidFinalContext context) : base(context)
        {
        }

        public CategoryRepository() 
        {
        }
        public string GetChainFrendlyUrlByAlias(int categoryId)
        {
            var data = _entities ?? new PyramidFinalContext();
            try
            {
                List<string> categoriesAliass = new List<string>();
                bool isExistCategoryAliasFlag = true;
                List<int> usedIds = new List<int>();
                var cat = data.Categories.Find(categoryId);
                if (cat != null)
                {
                    if (cat.Seo == null || string.IsNullOrEmpty(cat.Seo.Alias))
                    {
                        isExistCategoryAliasFlag = false;
                        return null;
                    }
                    else
                    {
                        categoriesAliass.Add(cat.Seo.Alias);
                    }
                    usedIds.Add(cat.Id);
                    while (cat.Categories2 != null && isExistCategoryAliasFlag)
                    {
                        cat = cat.Categories2;
                        if (cat.Seo == null || string.IsNullOrEmpty(cat.Seo.Alias)||usedIds.Contains(cat.Id))
                        {
                            isExistCategoryAliasFlag = false;
                            break;
                        }
                        else
                        {
                            categoriesAliass.Add(cat.Seo.Alias);
                        }
                        usedIds.Add(cat.Id);
                    }
                    if (isExistCategoryAliasFlag)
                    {

                        categoriesAliass.Reverse();
                    }



                    var sw = new StringBuilder();
                    foreach (var item in categoriesAliass)
                    {
                        sw.Append("/");
                        sw.Append(item.ToLower());
                    }

                    return sw.ToString();
                }
                return null;
            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
        }


        public IEnumerable<RootCategory> GetRootCategoriesWithSubs()
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                
                var rootCategories = dbContext.Categories.Where(i => i.ParentId == null&&i.ShowCategoryOnSite && (i.Products.Count > 0 || i.Categories1.Count > 0)).ToList();
                var d = new List<RootCategory>();
                foreach (var item in rootCategories)
                {
                    d.Add(new RootCategory()
                    {
                        Category = ConvertDbObjectToEntityShort(dbContext, item)
                        //new Pyramid.Entity.Category()
                        //{
                        //    Id = item.Id,
                        //    Thumbnail = item.CategoryImages.FirstOrDefault(f => f.CategoryId == item.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                        //    ConvertImageToEntity.Convert( item.CategoryImages.FirstOrDefault(f => f.CategoryId == item.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images) : null,
                        //    Title = item.Title,
                        //}
                        ,
                        SubCategories = dbContext.Categories.Where(i => i.ParentId == item.Id && i.ShowCategoryOnSite).ToList().Select(s => ConvertDbObjectToEntityShort(dbContext, s)).ToList(),
                        //{
                        //    Id = s.Id,
                        //    Thumbnail = s.CategoryImages.FirstOrDefault(f => f.CategoryId == s.Id && f.TypeImage == 1) != null ?
                        //    ConvertImageToEntity.Convert(s.CategoryImages.FirstOrDefault(f => f.CategoryId == s.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images) : null,
                        //    Title = s.Title,
                        //}).ToList()
                    });
                }
                return d;
            }
        }

        public RootCategory GetRootCategoryWithSubs(int id)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efCat = dbContext.Categories.FirstOrDefault(i => i.Id == id);
                if (efCat != null)
                {
                    var rootCategory = new RootCategory()
                    {
                        Category = new Pyramid.Entity.Category()
                        {
                            Id = efCat.Id,
                            Thumbnail = efCat.CategoryImages.FirstOrDefault(f => f.CategoryId == efCat.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                                 ConvertImageToEntity.Convert(efCat.CategoryImages.FirstOrDefault(f => f.CategoryId == efCat.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images) : null,
                            Title = efCat.Title,
                            //Products = efCat.Products
                        },
                        SubCategories = dbContext.Categories.Where(i => i.ParentId == efCat.Id).Select(s => new Pyramid.Entity.Category()
                        {
                            Id = s.Id,
                            Thumbnail = s.CategoryImages.FirstOrDefault(f => f.CategoryId == s.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                            ConvertImageToEntity.Convert(s.CategoryImages.FirstOrDefault(f => f.CategoryId == s.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images) : null,
                            Title = s.Title,
                            // Products = s.Products
                        })

                    };
                    return rootCategory;
                }
                return new RootCategory();

            }
        }

        public IEnumerable<Pyramid.Entity.Category> GetNestedCategories(int categoryId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efCat = dbContext.Categories.FirstOrDefault(i => i.Id == categoryId);
                return efCat.Categories1.Select(s => ConvertDbObjectToEntityShort(dbContext, s)).ToList();
            }

        }

        public IEnumerable<Category> GetRootCategoriesWithThumbnail(int typeThumbnail)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var rootCategories = dbContext.Categories
                    .Where(i => i.ParentId == null && i.ShowCategoryOnSite && (i.Products.Count > 0 || i.Categories1.Count > 0))
                    .ToList() 
                    .Select(i => ConvertDbObjectToEntityShort(dbContext,i))
                    .ToList();

                return rootCategories;
            }
        }

        public IEnumerable<Pyramid.Entity.Filter> GetFilters(int categoryId)
        {

            var category = Get(categoryId);
            if (category != null)
            {
                return category.Filters.ToList();
            }
            return new List<Pyramid.Entity.Filter>();
        }


        public IEnumerable<Pyramid.Entity.Recommendation> GetRecommendations(int categoryId)
        {
            var category = Get(categoryId);
            if (category != null)
            {
                return category.Recommendations.ToList();
            }
            return new List<Pyramid.Entity.Recommendation>();
        }

        public void DeleteFilter(int categoryId, int filterId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efCategory = dbContext.Categories.Find(categoryId);
                if (efCategory != null)
                {

                    var efFilter = dbContext.Filters.Find(filterId);
                    efCategory.Filters.Remove(efFilter);
                    dbContext.SaveChanges();
                }
            }

        }
        public void DeleteRecommendation(int categoryId, int recommendationid)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efCategory = dbContext.Categories.Find(categoryId);
                if (efCategory != null)
                {

                    var efRecommendation = dbContext.Recommendations.Find(recommendationid);
                    efCategory.Recommendations.Remove(efRecommendation);
                    dbContext.SaveChanges();
                }
            }

        }

        public override void AddOrUpdate(Category entity)
        {
            var data = _entities ?? new PyramidFinalContext();
            try
            {
                var objects = data.Categories;
                var dbObject = GetDbObjectByEntity(objects, entity);
                bool exists = dbObject != null;
                if (!exists)
                {
                    dbObject = new Categories();
                }
                UpdateBeforeSaving(data, dbObject, entity, exists);
                if (!exists)
                {
                    objects.Add(dbObject);
                }
                data.SaveChanges();
                UpdateAfterSaving(data, dbObject, entity, exists);
                data.SaveChanges();

            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
            
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Categories dbEntity, Category entity, bool exists)
        {
            dbEntity.FlagRoot = entity.FlagRoot;
            dbEntity.ParentId = entity.ParentId;
            dbEntity.Title = entity.Title;
            dbEntity.Content = entity.Content;
            dbEntity.ShowCategoryOnSite = entity.ShowCategoryOnSite;
            dbEntity.Id = entity.Id;
            if (entity.OneCId!=null)
            {
                dbEntity.OneCId = entity.OneCId;
            }

            //dbContext.SaveChanges();
        }
        public override void UpdateAfterSaving(PyramidFinalContext dbContext, Categories dbEntity, Category entity, bool exists)
        {
            if (entity.Thumbnail != null && entity.Thumbnail.Id != 0)
            {
                var typeImage = (int)Common.TypeImage.Thumbnail;
                var efImage = dbContext.CategoryImages.FirstOrDefault(i => i.TypeImage == typeImage && i.CategoryId==dbEntity.Id);

                if (efImage != null)
                {
                    if (efImage.ImageId == entity.Thumbnail.Id)
                    {

                    }
                    else
                    {
                        dbEntity.CategoryImages.Remove(efImage);

                        dbEntity.CategoryImages.Add(new CategoryImages()
                        {
                            CategoryId = dbEntity.Id,
                            ImageId = entity.Thumbnail.Id,
                            TypeImage = typeImage
                        });

                    }

                }
                else
                {
                    dbEntity.CategoryImages.Add(new CategoryImages()
                    {
                        CategoryId = dbEntity.Id,
                        ImageId = entity.Thumbnail.Id,
                        TypeImage = typeImage
                    });
                }

              
               // SetThumbnail(dbEntity.Id, entity.Thumbnail.Id, (int)Common.TypeImage.Thumbnail);

            }
            if (entity.Filters != null)
            {
                dbEntity.Filters.Clear();
                foreach (var item in entity.Filters)
                {
                    var efFilter = dbContext.Filters.Find(item.Id);
                    dbEntity.Filters.Add(efFilter);
                }
            }
            if (dbEntity.Seo == null)
            {
                dbEntity.Seo = new Seo();
            }

            if (entity.Seo!=null)
            {
                dbEntity.Seo.MetaDescription = entity.Seo.MetaDescription;
                dbEntity.Seo.MetaKeywords = entity.Seo.MetaKeywords;
                dbEntity.Seo.MetaTitle = entity.Seo.MetaTitle;
                dbEntity.Seo.Alias = entity.Seo.Alias;
            }

            //dbContext.SaveChanges();
            //dbEntity.SeoId = dbEntity.Seo.Id;

            if (entity.Recommendations!=null)
            {
                dbEntity.Recommendations.Clear();
                foreach (var item in entity.Recommendations)
                {
                    var recomend = dbContext.Recommendations.Find(item.Id);
                    if (recomend!=null)
                    {
                        dbEntity.Recommendations.Add(recomend);
                    }
                }
            }
        }
        public IEnumerable<Products> GetWithCheckedEnumValues(int categoryId,IEnumerable<int> enumValueIds)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                var efCategory = dbContext.Categories.Find(categoryId);
                if (efCategory != null)
                {
                    if (enumValueIds.Count() > 0)
                    {

                        var efEnumValuetemp = dbContext.EnumValues.ToList();
                        var tttest = efCategory.Products.Where(i => i.EnumValues.Any(p => enumValueIds.Contains(p.Id))).ToList();
                        var te = efEnumValuetemp.Where(i => enumValueIds.Contains(i.Id)).ToList();
                        return tttest;
                    }

                    return efCategory.Products;
                }
                return new List<Products>();

            }
           
           
        }

        public void SetThumbnail(int categoryId, int imageId, int typeImage)
        {
            var efCategory = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efCategory != null)
            {
                var efImage = efCategory.CategoryImages.FirstOrDefault(i => i.TypeImage == typeImage);
                
                if (efImage!=null)
                {
                    if (efImage.ImageId == imageId)
                    {
                        return;
                    }
                    efCategory.CategoryImages.Remove(efImage);
                }
               
                efCategory.CategoryImages.Add(new CategoryImages()
                {
                    CategoryId = categoryId,
                    ImageId = imageId,
                    TypeImage = typeImage
                });
               
            }

        }

        public void RemoveImageToCategory(int categoryId, int imageId, int typeImage)
        {
            var efCategory = FindBy(i => i.Id == categoryId).SingleOrDefault();
            if (efCategory != null)
            {
                var efImage = efCategory.CategoryImages.FirstOrDefault(i => i.ImageId == imageId && i.TypeImage == typeImage);
                if (efImage != null)
                {
                    efCategory.CategoryImages.Remove(efImage);
                }
            }

        }

        public Images GetThumbnail(int categoryId, int typeThumbnail)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                return dbContext.Images.FirstOrDefault(i => i.CategoryImages.Any(f => f.CategoryId == categoryId && f.TypeImage == typeThumbnail));

            }
        }

        public int GetMaxPriceFromCategory(int categoryId)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                var efcat = dbContext.Categories.Find(categoryId);
                if (efcat != null)
                {
                    if (efcat.Products != null && efcat.Products.Count > 0)
                    {
                        int max = (int)efcat.Products.Max(i => i.Price);
                        int min = (int)efcat.Products.Min(i => i.Price);
                        return max;
                    }

                }
                return 0;
            }
           
        }
        public int GetMinPriceFromCategory(int categoryId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efcat = dbContext.Categories.Find(categoryId);
                if (efcat != null)
                {
                    if (efcat.Products != null && efcat.Products.Count > 0)
                    {
                        int min = (int)efcat.Products.Min(i => i.Price);
                        return min;
                    }
                }
                return 0;
            }
        }

        public IEnumerable< Products> GetProductsByCategoryId(int id)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                var efCat = dbContext.Categories.Include(i => i.Products).FirstOrDefault(f => f.Id == id);

                if (efCat != null)
                {
                    return efCat.Products.ToList();
                }
                return new List<Products>();
            }
          
        }

        public void InsertsOrNot(IEnumerable<Categories> entities)
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
                    var efCat = context.Categories.FirstOrDefault(f => f.OneCId == entity.OneCId);
                    if (efCat == null)
                    {
                        context.Entry(entity).State = EntityState.Added;
                        
                    }
                }

                context.SaveChanges();

                context.Configuration.AutoDetectChangesEnabled = true;
                context.Configuration.ValidateOnSaveEnabled = true;
            }


        }

        public void UpdateParentCategory(IEnumerable<Category1CIdWithParent1CId> entities)
        {
            // Настройки контекста
            using (PyramidFinalContext context = new PyramidFinalContext())
            {
                // Отключаем отслеживание и проверку изменений для оптимизации вставки множества полей
                //context.Configuration.ValidateOnSaveEnabled = false;

                context.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));


                foreach (var entity in entities)
                {
                    var efCat = context.Categories.FirstOrDefault(f => !string.IsNullOrEmpty(entity.Id)&& !string.IsNullOrEmpty(f.OneCId) && f.OneCId == entity.Id);
                    if (efCat != null)
                    {
                        var efParent = context.Categories.FirstOrDefault(f => !string.IsNullOrEmpty(entity.Id) && !string.IsNullOrEmpty(f.OneCId) && f.OneCId == entity.ParentId);
                        if (efParent!=null)
                        {
                            if (efCat.OneCId != efParent.OneCId)
                            {
                                efCat.ParentId = efParent.Id;
                            }
                            else
                            {
                                efCat.ParentId = null;

                            }
                           
                            context.SaveChanges();
                        }
                       
                    }
                }

                context.SaveChanges();

                context.Configuration.ValidateOnSaveEnabled = true;
            }

        }

        public bool IsCategoryNested(int id)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                var t = dbContext.Categories.Where(w => w.ParentId == id).ToList();
                return t != null&&t.Count>0;
            }
        }

        public void AddOrUpdateFromOneC(Categories category)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var dbCategory = dbContext.Categories.FirstOrDefault(i => i.OneCId == category.OneCId);
                var exist = dbCategory != null;
                if (!exist)
                {
                    dbCategory = new Categories();
                }
                UpdateBeforeSavingOceC(dbContext, dbCategory, category, exist);
                dbContext.SaveChanges();

                UpdateAfterSavingOneC(dbContext, dbCategory, category, exist);
                dbContext.SaveChanges();
            }

        }

        public void UpdateBeforeSavingOceC(PyramidFinalContext dbContext, Categories dbObjext, Categories model, bool exist)
        {
            dbObjext.Title = model.Title;
            dbObjext.FlagRoot = false;
            dbObjext.OneCId = model.OneCId;
           
            if (!exist)
            {
                dbContext.Categories.Add(dbObjext);
            }
           
        }

        public void UpdateAfterSavingOneC(PyramidFinalContext dbContext, Categories dbObjext, Categories model, bool exist)
        {
            if (dbObjext.Seo==null)
            {
                var seo= new Seo()
                {
                    MetaTitle = dbObjext.Title,
                };
                dbContext.Seo.Add(seo);
                dbContext.SaveChanges();
                dbObjext.Seo = seo;
            }
        }

        public void DeleteAllFilterBrands()
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var dbFilterBrand = dbContext.Filters.Where(i => i.Title == "Бренд");
                dbContext.Filters.RemoveRange(dbFilterBrand);
                var enumBrands = dbContext.EnumValues.Where(i => i.TypeValue == (int)Common.TypeFromEnumValue.Brand);
                dbContext.EnumValues.RemoveRange(enumBrands);
                dbContext.SaveChanges();
            }
        }
        public void AddOrUpdateFilterBrand()
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
               // переписать
                var brandFilter= dbContext.Filters.FirstOrDefault(i => i.Title == "Бренд");
                if (brandFilter==null)
                {
                    brandFilter = new Filters()
                    {
                        Title = "Бренд",
                    };
                    dbContext.Filters.Add(brandFilter);
                    dbContext.SaveChanges();
                }
                foreach (var category in dbContext.Categories)
                {
                    var dbFilterBrand = category.Filters.FirstOrDefault(i => i.Title == "Бренд");
                    if (dbFilterBrand == null)
                    {
                        category.Filters.Add(brandFilter);
                    }
                        //dbFilterBrand = new Filters()
                        //{
                        //    Title = "Бренд",

                        //};
                        //var listEnumValuesBrand = new List<EnumValues>();
                        //foreach (var item in category.Products)
                        //{
                        //    var valueBrand = item.EnumValues.Where(w => w.TypeValue == (int)Common.TypeFromEnumValue.Brand);
                        //    if (valueBrand != null && valueBrand.Count() > 0)
                        //    {
                        //        foreach (var value in valueBrand)
                        //        {
                        //            listEnumValuesBrand.Add(value);
                        //        }
                        //    }
                        //}

                        //if (listEnumValuesBrand.Count>0)
                        //{
                        //    dbFilterBrand.EnumValues = listEnumValuesBrand.Distinct().ToList();
                        //    //dbContext.Filters.Add(dbFilterBrand);
                        //    category.Filters.Add(dbFilterBrand);
                        //}

                        foreach (var item in category.Products)
                        {
                            var valueBrand = item.EnumValues.Where(w => w.TypeValue == (int)Common.TypeFromEnumValue.Brand);
                            if (valueBrand != null && valueBrand.Count() > 0)
                            {
                                foreach (var value in valueBrand)
                                {
                                    if (!brandFilter.EnumValues.Contains(value))
                                    {
                                        brandFilter.EnumValues.Add(value);
                                    }
                                   
                                }
                            }
                        }
                        
                    //}
                    //else
                    //{
                    //    var listEnumValuesBrand = new List<EnumValues>();
                    //    foreach (var item in category.Products)
                    //    {
                    //        var valueBrand = item.EnumValues.Where(w => w.TypeValue == (int)Common.TypeFromEnumValue.Brand);
                    //        if (valueBrand != null && valueBrand.Count() > 0)
                    //        {
                    //            foreach (var value in valueBrand)
                    //            {
                    //                listEnumValuesBrand.Add(value);
                    //            }
                    //        }
                    //    }
                    //  var enumvalues= listEnumValuesBrand.Distinct().ToList();
                    //    foreach (var item in enumvalues )
                    //    {
                    //        if (!dbFilterBrand.EnumValues.Contains(item))
                    //        {
                    //            dbFilterBrand.EnumValues.Add(item);
                    //        }
                    //    }
                    //    //dbFilterBrand.EnumValues = 
                    //}
                    
                }
                //var enumBrands = dbContext.EnumValues.Where(i => i.TypeValue == (int)Common.TypeFromEnumValue.Brand).ToList();
                //foreach (var enumVal in enumBrands)
                //{
                //    if (enumVal.Filters.Count==0)
                //    {
                //        dbContext.EnumValues.Remove(enumVal);
                //    }
                //}
                dbContext.SaveChanges();
                //dbContext.SaveChanges();
            }
        }


        protected override Categories GetDbObjectByEntity(DbSet<Categories> objects, Category entity)
        {
          return  objects.FirstOrDefault(item => item.Id == entity.Id);
        }

        protected override Expression<Func<Categories, int>> GetIdByDbObjectExpression()
        {
            return item => item.Id;
        }

        public override Category ConvertDbObjectToEntity(PyramidFinalContext context, Categories dbObject)
        {
            var seo = new SeoRepository(context).Get(dbObject.SeoId.HasValue ? dbObject.SeoId.Value : 0);
            var products = new ProductRepository(context).Get(new SearchParamsProduct() { CategoryId = dbObject.Id, IsSearchOnlyPublicProduct = true });
            var friendlyUrl = new RouteItemRepository(context).GetFriendlyUrl(dbObject.Id, Common.TypeEntityFromRouteEnum.CategoryType);
            var cat = new Category();
            cat.FriendlyUrl = friendlyUrl;
            cat.Filters = dbObject.Filters.Select(s => new Pyramid.Entity.Filter() {
                Id = s.Id,
                Title = s.Title,
                EnumValues = s.EnumValues.Select(e => new Pyramid.Entity.EnumValue()
                {
                    Id = e.Id,
                    Key=e.Key,
                    TypeValue=(Common.TypeFromEnumValue)e.TypeValue
                }).ToList(),
              }).ToList();


            cat.Products = products.Objects.ToList();

            cat.Id = dbObject.Id;
            cat.Title = dbObject.Title;
           

            cat.Thumbnail = new ImageRepository(context).GetFromCategory(dbObject.Id, (int)Common.TypeImage.Thumbnail);

            cat.OneCId = dbObject.OneCId;
            cat.ParentId = dbObject.ParentId;
            cat.Content = dbObject.Content;
            cat.ShowCategoryOnSite = dbObject.ShowCategoryOnSite;
            if (dbObject.Seo!=null)
            {
                cat.Seo = new Entity.Seo() {
                    Alias= dbObject.Seo.Alias,
                    Id= dbObject.Seo.Id,
                    MetaDescription= dbObject.Seo.MetaDescription,
                    MetaKeywords= dbObject.Seo.MetaKeywords,
                    MetaTitle= dbObject.Seo.MetaTitle
                };
            }

            if (dbObject.Recommendations!=null)
            {
                cat.Recommendations = dbObject.Recommendations.Select(s => new Recommendation() {
                    ShortContent=s.ShortContent,
                    Title=s.Title,
                    Id=s.Id,
                    Image=ConvertImageToEntity.Convert( s.Images.FirstOrDefault()),
                    FriendlyUrl=new RouteItemRepository(context).GetFriendlyUrl(s.Id,Common.TypeEntityFromRouteEnum.RecommendationType)
                }).ToList();
            }
            return cat;
            
        }

        protected override Category ConvertDbObjectToEntityShort(PyramidFinalContext context, Categories dbObject)
        {
            var cat = new Category();
            cat.Id = dbObject.Id;
            cat.Title = dbObject.Title;
            cat.ShowCategoryOnSite = dbObject.ShowCategoryOnSite;
            cat.Thumbnail = new ImageRepository(context).GetFromCategory(dbObject.Id, (int)Common.TypeImage.Thumbnail);
            var friendlyUrl = new RouteItemRepository(context).GetFriendlyUrl(dbObject.Id, Common.TypeEntityFromRouteEnum.CategoryType);
            cat.FriendlyUrl = friendlyUrl;

            return cat;
        }

        protected override IQueryable<Categories> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Categories> dbObjects, SearchParamsCategory searchParams)
        {
            if (!string.IsNullOrEmpty( searchParams.SearchString))
            {
                dbObjects = dbObjects.Where(i => i.Title.Contains(searchParams.SearchString));

            }
            if (searchParams.ProductId.HasValue)
            {
                dbObjects = dbObjects.Where(w =>  w.Products.Any(a => a.Id == searchParams.ProductId.Value));

            }
            if (searchParams.IsShowCategoryOnSite.HasValue && searchParams.IsShowCategoryOnSite.Value)
            {
                dbObjects.Where(w => w.ShowCategoryOnSite == searchParams.IsShowCategoryOnSite.Value);
            }
            return dbObjects;
        }

       

        //public Categories GetCategoryWithProductsAndImages(int id)
        //{
        //    using (PyramidFinalContext data =new PyramidFinalContext())
        //    {
        //        data.Configuration.LazyLoadingEnabled = false;
        //        data.Configuration.ProxyCreationEnabled = false;

        //        var efcat = data.Categories
        //            .Include(i=>i.Products)
        //            .Include(i=>i.CategoryImages)
        //            .FirstOrDefault(f => f.Id == id);


        //    }
        //}


        public IEnumerable<Common.Models.BreadCrumbViewModel> GetBreadCrumbs(int categoryId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
               var EfModel= dbContext.Categories.Find(categoryId);
                List<BreadCrumbViewModel> breadcrumbs = new List<BreadCrumbViewModel>();
                if (EfModel != null)
                {
                    breadcrumbs.Add(new BreadCrumbViewModel()
                    {
                        Title = EfModel.Title
                    ,
                        Link = defaulCateggorytLink + EfModel.Id.ToString(),
                        FriendlyUrl= new RouteItemRepository(dbContext).GetFriendlyUrl(EfModel.Id,Common.TypeEntityFromRouteEnum.CategoryType)
                    });
                    var flagstop = true;
                    var cat = EfModel.Categories2;
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
                                Link = defaulCateggorytLink + cat.Id.ToString(),
                                FriendlyUrl = new RouteItemRepository(dbContext).GetFriendlyUrl(EfModel.Id, Common.TypeEntityFromRouteEnum.CategoryType)
                            });

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

        public Category GetBySearchParams(SearchParamsCategory searchParams)
        {
            using (PyramidFinalContext dbContext=new PyramidFinalContext())
            {
                var dbCategory = dbContext.Categories.Find(searchParams.Id);
                if (dbCategory!=null)
                {
                    searchParams.ExistProductsInBd = dbCategory.Products.Count > 0;
                    foreach (var item in dbCategory.Products)
                    {
                        foreach (var eVal in item.EnumValues)
                        {
                            if (!searchParams.OutEnumValueIds.Contains(eVal.Id))
                            {
                                searchParams.OutEnumValueIds.Add(eVal.Id);
                            }
                        }
                        
                    }
                    IEnumerable<Products> temp= dbCategory.Products.Where(i => i.TypeStatusProduct != (int)Common.TypeStatusProduct.Hide/*&& i.Price>0*/).ToList();
                    if (searchParams.MaxPrice.HasValue)
                    {
                        temp = temp.Where(i => i.Price<= searchParams.MaxPrice.Value);
                    }
                    if (searchParams.MinPrice.HasValue)
                    {
                        temp = temp.Where(i => i.Price >= searchParams.MinPrice.Value);
                    }
                    if (searchParams.FiltersSearch!=null&& searchParams.FiltersSearch.Count()>0&& searchParams.FiltersSearch.Where(w=>w.EventValueIds.Count()>0).Count()>0)
                    {
                        List<IEnumerable<Products>> listProductsFromFilters = new List<IEnumerable<Products>>();
                        foreach (var item in searchParams.FiltersSearch)
                        {
                            if (item.EventValueIds.Count()>0)
                            {
                                listProductsFromFilters.Add(temp.Where(i => i.EnumValues.Any(a => item.EventValueIds.Contains(a.Id))));
                            }
                        }
                        foreach (var item in listProductsFromFilters)
                        {
                            temp = temp.Intersect(item);
                        }
                    }
                    dbCategory.Products = temp.ToList();
                }
                 

                return ConvertDbObjectToEntity(dbContext, dbCategory);
            }
        }

        public override bool Delete(int id)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                var objects = dbContext.Categories;
                var dbObject = GetDbObjectById(objects, id);
                if (dbObject == null) { 
                    return false;
                }
                else
                {
                    dbObject.ParentId = null;
                    foreach (var item in dbContext.Categories.Where(i=>i.ParentId==dbObject.Id))
                    {
                        item.ParentId = null;
                    }
                    dbContext.SaveChanges();
                    var seo=dbContext.Seo.Find(dbObject.SeoId);
                    if (seo!=null)
                    {
                        dbContext.Seo.Remove(seo);
                    }
                    dbContext.Categories.Remove(dbObject);
                }

                dbContext.SaveChanges();
                return true;
            }
        }

        public bool IsChildFromParent(int categoryChildId,int categoryParentId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                List<int> visitedList = new List<int>();
                var curCategory = dbContext.Categories.Find(categoryChildId);
                if (curCategory!=null)
                {
                   
                    while (curCategory!=null&&!visitedList.Contains(curCategory.Id))
                    {
                        visitedList.Add(curCategory.Id);
                        if (visitedList.Contains(categoryParentId))
                        {
                            return true;
                        }
                        curCategory = curCategory.Categories2;
                    }
                    //return BFSFromCategory(dbContext, curCategory, visitedList, categoryParentId);
                }
                return false;
            }
        }

        public void HideCategoryIfNotExistInCurrentUpdateFrom1C(IEnumerable<string> categoryIdStr)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                foreach (var item in dbContext.Categories)
                {
                    if (!categoryIdStr.Contains(item.OneCId))
                    {
                        item.ShowCategoryOnSite = false;
                    }
                }
                dbContext.SaveChanges();
            }
            
        }
        //private bool BFSFromCategory(PyramidFinalContext dbContext,Categories currentCategory, List<int> visitedIds, int categoryParentId)
        //{
        //    if (visitedIds.Contains(categoryParentId))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        if (currentCategory.Categories1!=null)
        //        {
        //            foreach (var item in currentCategory.Categories1)
        //            {
        //                if (!visitedIds.Contains(item.Id))
        //                {
        //                    visitedIds.Add(item.Id);
        //                    BFSFromCategory(dbContext, item, visitedIds, categoryParentId);
        //                }
        //            }
        //        }
        //        return false;
        //    }
        //}
        public static IEnumerable<Pyramid.Entity.Category> GetAllWithoutCategoryId(int catId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                return dbContext.Categories.Where(c => c.Id != catId).Select(c => new Pyramid.Entity.Category()
                {
                    Id = c.Id,
                    Title = c.Title,
                }).ToList(); ;
            }
        }

        public void InitSeo(int categoryId)
        {
            using (PyramidFinalContext context = new PyramidFinalContext())
            {
                var categoryDb = context.Categories.Find(categoryId);
                if (categoryDb != null)
                {
                    if (categoryDb.Seo == null)
                    {
                        categoryDb.Seo = new Seo();

                    }
                    categoryDb.Seo.Alias = Tools.Transliteration.Translit(categoryDb.Title);
                    categoryDb.Seo.MetaTitle = categoryDb.Title;
                    context.SaveChanges();

                }
            }

        }

    }
}
