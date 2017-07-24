using DBFirstDAL.DataModels.HomeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Validation;
using Common.SearchClasses;
using Pyramid.Entity;
using System.Linq.Expressions;
using DBFirstDAL.Convert;

namespace DBFirstDAL.Repositories
{
    public class HomeEntityRepository : GenericRepository<HomeEntity,PyramidFinalContext,Pyramid.Entity.HomeEntity, SearchParamsBase,int>
    {


        public HomeEntity GetManageModel(int id)
        {
            var efHome = FindBy(i => i.Id == id).SingleOrDefault();
            //ManageHomeEntityDataModel dataModel = new DataModels.HomeModels.ManageHomeEntityDataModel();
            if (efHome != null)
            {


                // CategoryHomeModel catModel = new CategoryHomeModel();
                //// IEnumerable<ProductHomeModel> prModel=new List<ProductHomeModel>();
                // if (efHome.Products!=null)
                // {


                // var prModel = efHome.Categories.Products.Select(i => new ProductHomeModel()
                // {
                //     Id = i.Id,
                //     InStock = (bool)i.InStock,
                //     Price = i.Price,
                //     ThumbnailImg = (i.ProductImages.FirstOrDefault(f => f.ProductId == i.Id)) != null ?
                //      i.ProductImages.FirstOrDefault(f => f.ProductId == i.Id).Images
                //      :
                //      null,
                //     Title = i.Title,
                //     TypePrice = i.TypePrice
                // });
                //     catModel.Title = efHome.Categories.Title;
                //     catModel.Id = efHome.Categories.Id;
                //     catModel.Products = prModel;
                // }


                // dataModel.Category = catModel;
                // dataModel.Faq = efHome.Faq;
                // dataModel.BannerWithPoints = efHome.BannerWithPoints;
                // dataModel.VideoGuide = efHome.VideoGuide;
                // dataModel.Id = efHome.Id;
                // dataModel.Title = efHome.Title;

                return efHome;
            }
            return new HomeEntity();




        }

        public HomeEntity GetModel(int id)
        {
            var efHome = FindBy(i => i.Id == id).SingleOrDefault();
            HomeEntityModel dataModel = new DataModels.HomeModels.HomeEntityModel();
            if (efHome != null)
            {


                //CategoryHomeModel catModel = new CategoryHomeModel();

                //var prModel = efHome.Categories.Products.Select(i => new ProductHomeModel()
                //{
                //    Id = i.Id,
                //    InStock = (bool)i.InStock,
                //    Price = i.Price,
                //    ThumbnailImg = (i.ProductImages.FirstOrDefault(f => f.ProductId == i.Id)) != null ?
                //     i.ProductImages.FirstOrDefault(f => f.ProductId == i.Id).Images
                //     :
                //     null,
                //    Title = i.Title,
                //    TypePrice = i.TypePrice
                //});
                //catModel.Title = efHome.Categories.Title;
                //catModel.Id = efHome.Categories.Id;
                //catModel.Products = prModel;

                //dataModel.Category = catModel;
                //dataModel.Faq = efHome.Faq;
                //dataModel.BannerWithPoints = new BannerWithPointsHomeDataModel()
                //{
                //    BannerId = efHome.BannerWithPoints.BannerId,
                //    Images = efHome.BannerWithPoints.Images,
                //    PointOnImgs = efHome.BannerWithPoints.PointOnImgs.Select(i => new PointOnImgsDataModel()
                //    {
                //        BannerId = i.BannerId,
                //        CoordX = i.CoordX,
                //        CoordY = i.CoordY,
                //        Products = ToModelProduct(i.Products),
                //        Id = i.Id
                //    }).ToList()
                //};
                //dataModel.VideoGuide = efHome.VideoGuide;
                //dataModel.Id = efHome.Id;
                //dataModel.Title = efHome.Title;

                //return dataModel;
                return efHome;
            }
            return new HomeEntity();




        }

        public IEnumerable<Pyramid.Entity.HomeEntity> GetModels(bool adminManage)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                if (adminManage)
                {
                    return GetAll();
                }
                else
                {
                    var entity= dbContext.HomeEntity
                        .ToList()
                        .Select(s => ConvertDbObjectToEntity(dbContext, s))
                        .ToList();
                    return entity;
                }
                
            }

        }

        private ProductHomeModel ToModelProduct(Products product)
        {
            var model = new ProductHomeModel();
            if (product != null)
            {
                model.Id = product.Id;
                //model.InStock = product.InStock.HasValue ? product.InStock.Value : false;
                model.Price = product.Price;
                model.SeasonOffer = product.SeasonOffer.HasValue ? product.SeasonOffer.Value : false;
                model.ThumbnailImg = product.ProductImages.FirstOrDefault(i => i.TypeImage == 1 && i.ProductId == product.Id) != null ? product.ProductImages.FirstOrDefault(i => i.TypeImage == 1 && i.ProductId == product.Id).Images : null;
                model.Title = product.Title;
                model.TypePrice = product.TypePrice;

            }
            return model;
        }

        private CategoryHomeModel ToModelCategory(Categories cat)
        {
            CategoryHomeModel catModel = new CategoryHomeModel();
            if (cat != null)
            {
                catModel.Title = cat.Title;
                catModel.Id = cat.Id;
            }

            return catModel;
        }

        //public void AddOrUpdateModel(HomeEntity model)
        //{
        //    var efHomeModel = FindBy(i => i.Id == model.Id).SingleOrDefault();
        //    if (efHomeModel == null)
        //    {
        //        efHomeModel = new HomeEntity();

        //        efHomeModel.Title = model.Title;
        //        if (model.Content != null)
        //        {
        //            efHomeModel.Content = model.Content;
        //        }
        //        efHomeModel.LinkYouTobe = model.LinkYouTobe;
        //        if (model.ThumbnailId!=0)
        //        {
        //            efHomeModel.ThumbnailId = model.ThumbnailId;

        //        }
                
        //        efHomeModel.CallToAction = model.CallToAction;
        //        Context.HomeEntity.Add(efHomeModel);
        //        //Save();
        //        foreach (var item in model.Categories)
        //        {
        //            var efCat = Context.Categories.Find(item.Id);
        //            if (efCat != null)
        //            {
        //                efHomeModel.Categories.Add(efCat);
        //            }

        //        }
        //        foreach (var item in model.Products)
        //        {
        //            var efPr = Context.Products.Find(item.Id);
        //            if (efPr != null)
        //            {
        //                efHomeModel.Products.Add(efPr);
        //            }

        //        }
        //        var efFaq = Context.Faq.Find(model.Faq.Id);
        //        if (efFaq != null)
        //        {
        //            efHomeModel.Faq = efFaq;
        //        }
        //        //Save();
        //        efHomeModel.BannerWithPoints = new BannerWithPoints()
        //        {
        //            BannerId = efHomeModel.Id,
        //            ImageId = model.BannerWithPoints.Images.Id != 0 ? (int?)model.BannerWithPoints.Images.Id : null
        //        };
        //        foreach (var item in model.BannerWithPoints.PointOnImgs)
        //        {
        //            efHomeModel.BannerWithPoints.PointOnImgs.Add(new PointOnImgs()
        //            {
        //                BannerId = efHomeModel.BannerWithPoints.BannerId,
        //                CoordX = item.CoordX,
        //                CoordY = item.CoordY,
        //                Id = item.Id,
        //                ReferenceProductId = item.Products != null ? item.Products.Id : (int?)null
        //            });
        //        }
        //        //Save();


        //    }
        //    else
        //    {
        //        efHomeModel.Title = model.Title;
        //        if (model.Content != null)
        //        {
        //            efHomeModel.Content = model.Content;
        //           // Save();
        //        }
        //        efHomeModel.LinkYouTobe = model.LinkYouTobe;
        //        efHomeModel.TitleVideoGuide = model.TitleVideoGuide;
        //        if (model.ThumbnailId!=0)
        //        {
        //            efHomeModel.ThumbnailId = model.ThumbnailId;

        //        }
                
        //        efHomeModel.CallToAction = model.CallToAction;


        //        try
        //        {
        //            //Save();
        //        }
        //        catch (DbEntityValidationException ex)
        //        {
        //            StringBuilder s = new StringBuilder();
        //            foreach (var entityValidationErrors in ex.EntityValidationErrors)
        //            {
        //                foreach (var validationError in entityValidationErrors.ValidationErrors)
        //                {
        //                    s.AppendLine("Property: "+ validationError.PropertyName + " Error: "+ validationError.ErrorMessage);
        //                }
        //            }

        //        }

        //        efHomeModel.Categories.Clear();
        //        foreach (var item in model.Categories)
        //        {
        //            var efCat = Context.Categories.Find(item.Id);
        //            if (efCat != null)
        //            {
        //                efHomeModel.Categories.Add(efCat);
        //            }

        //        }

        //        efHomeModel.Products.Clear();
        //        //Save();
        //        foreach (var item in model.Products)
        //        {
        //            var efPr = Context.Products.Find(item.Id);
        //            if (efPr != null)
        //            {
        //                efHomeModel.Products.Add(efPr);
        //            }

        //        }
        //        var efFaq = Context.Faq.Find(model.Faq.Id);
        //        if (efFaq != null)
        //        {
        //            efHomeModel.Faq = efFaq;
        //        }
        //        //Save();
        //        if (efHomeModel.BannerWithPoints == null)
        //        {
        //            efHomeModel.BannerWithPoints = new BannerWithPoints()
        //            {
        //                BannerId = model.Id,
        //                ImageId = model.BannerWithPoints.Images.Id != 0 ? (int?)model.BannerWithPoints.Images.Id : null
        //            };
        //        }
        //        else
        //        {

        //            efHomeModel.BannerWithPoints.ImageId = model.BannerWithPoints.Images.Id != 0 ? (int?)model.BannerWithPoints.Images.Id : null;
        //        }
        //        //efHomeModel.BannerWithPoints.PointOnImgs.Clear();

        //        //Save();

        //        if (efHomeModel.BannerWithPoints.PointOnImgs != null && efHomeModel.BannerWithPoints.PointOnImgs.Count > 0)
        //        {
        //            List<PointOnImgs> oldImgs = new List<PointOnImgs>(efHomeModel.BannerWithPoints.PointOnImgs);

        //            foreach (var item in oldImgs)
        //            {
        //                var efpoint = efHomeModel.BannerWithPoints.PointOnImgs.FirstOrDefault(i => i.Id == item.Id);
        //                if (efpoint != null)
        //                {
        //                    //efHomeModel.BannerWithPoints.PointOnImgs.Remove(efpoint);
        //                    Context.PointOnImgs.Remove(efpoint);
        //                }

        //            }
        //        }
        //        //Save();
        //        foreach (var item in model.BannerWithPoints.PointOnImgs)
        //        {
        //            efHomeModel.BannerWithPoints.PointOnImgs.Add(new PointOnImgs()
        //            {
        //                BannerId = efHomeModel.BannerWithPoints.BannerId,
        //                CoordX = item.CoordX,
        //                CoordY = item.CoordY,
        //                Id = item.Id,
        //                ReferenceProductId = item.Products != null ? item.Products.Id : (int?)null
        //            });
        //        }
        //        //Save();

        //        //var efVideo = Context.VideoGuide.Find(efHomeModel.Id);
        //        //if(efVideo!=null)
        //        //{
        //        //    efVideo.LinkYouTobe = model.VideoGuide.LinkYouTobe;
        //        //    efVideo.ThumbnailId = model.VideoGuide.Images.Id;
        //        //}
        //        //else
        //        //{
        //        //    efVideo = new VideoGuide()
        //        //    {
        //        //        HomeEntityId = model.Id,
        //        //        LinkYouTobe = model.VideoGuide.LinkYouTobe,
        //        //        ThumbnailId = model.VideoGuide.Images.Id,
        //        //        HomeEntity=efHomeModel
        //        //    };
        //        //    Context.VideoGuide.Add(efVideo);

        //        //}

        //        //Save();
        //    }
        //}
 

        //public IQueryable<HomeEntity> GetAllWithNavigation()
        //{
        //    //// говорим, что не надо создавать динамически генерируемые прокси-классы
        //    //// (которые System.Data.Entity.DynamicProxies...)
        //    //Context.Configuration.ProxyCreationEnabled = false;
        //    //// отключаем ленивую загрузку
        //    //Context.Configuration.LazyLoadingEnabled = false;

        //    using (Context)
        //    {
        //        Context.HomeEntity.Include()
        //    }
        //    IQueryable<T> query = _entities.Set<T>().AsNoTracking();
        //    return query;
        //}

        public void DeletePoint(int pointId)
        {
            using (PyramidFinalContext dbContext =new PyramidFinalContext())
            {
                var efPoint = dbContext.PointOnImgs.Find(pointId);
                if (efPoint != null)
                {
                    dbContext.PointOnImgs.Remove(efPoint);
                    dbContext.SaveChanges();
                }
            }
           
        }

        public void DeleteProduct(int homeEntityId,int productId)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efEntity = dbContext.HomeEntity.Find(homeEntityId);
                var efProd = dbContext.Products.Find(productId);
                if (efProd != null)
                {
                    efEntity.Products.Remove(efProd);
                    dbContext.SaveChanges();
                }
            }
        }
        public void DeleteCategory(int homeEntityId, int categoryId )
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                var efEntity = dbContext.HomeEntity.Find(homeEntityId);

                var cat = dbContext.Categories.Find(categoryId);
                if (cat != null)
                {
                    efEntity.Categories.Remove(cat);
                    dbContext.SaveChanges();
                }
            }
            //Save();
        }

        protected override HomeEntity GetDbObjectByEntity(DbSet<HomeEntity> objects, Pyramid.Entity.HomeEntity entity)
        {
            return objects.FirstOrDefault(item => item.Id == entity.Id);    
        }

        protected override Expression<Func<HomeEntity, int>> GetIdByDbObjectExpression()
        {
            return item => item.Id;
        }

        public override Pyramid.Entity.HomeEntity ConvertDbObjectToEntity(PyramidFinalContext context, HomeEntity dbObject)
        {
            var hEntity = new Pyramid.Entity.HomeEntity();
            if (dbObject.BannerWithPoints != null)
            {
                hEntity.BannerWithPoints = new Pyramid.Entity.BannerWithPoints()
                {
                    BannerId = dbObject.BannerWithPoints.BannerId,
                    Images = dbObject.BannerWithPoints.Images != null ? new Image()
                    {
                        Id = dbObject.BannerWithPoints.Images.Id,
                        ImgAlt = dbObject.BannerWithPoints.Images.ImgAlt,
                        PathInFileSystem = dbObject.BannerWithPoints.Images.PathInFileSystem,
                        ServerPathImg = dbObject.BannerWithPoints.Images.ServerPathImg,
                        Title = dbObject.BannerWithPoints.Images.Title
                    } : new Image(),
                    PointOnImgs = dbObject.BannerWithPoints.PointOnImgs.Select(p => new Pyramid.Entity.PointOnImg()
                    {
                        CoordX = p.CoordX,
                        CoordY = p.CoordY,
                        Id = p.Id,
                        Products = new Product()
                        {
                            ThumbnailImg = p.Products.ProductImages.FirstOrDefault(f => f.ProductId == p.Products.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                               ConvertImageToEntity.Convert(p.Products.ProductImages.FirstOrDefault(f => f.ProductId == p.Products.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images) : new Pyramid.Entity.Image(),
                            Id = p.Products.Id,
                            Title = p.Products.Title,
                            Price = p.Products.Price,
                            TypePrice = (Common.TypeProductPrice)p.Products.TypePrice
                        }
                    }).ToList()
                };
            }
            hEntity.CallToAction = dbObject.CallToAction;
            hEntity.Content = dbObject.Content;
            hEntity.Id = dbObject.Id;
            hEntity.LinkYouTobe = dbObject.LinkYouTobe;
            hEntity.Title = dbObject.Title;
            hEntity.TitleVideoGuide = dbObject.TitleVideoGuide;

            hEntity.Categories = dbObject.Categories.Select(s => new Pyramid.Entity.Category()
            {
                Id = s.Id,
                Title = s.Title,
            }).ToList();

            hEntity.Images = ConvertImageToEntity.Convert(dbObject.Images);

            hEntity.Products = dbObject.Products.Select(p => new Pyramid.Entity.Product()
            {
                ThumbnailImg = p.ProductImages.FirstOrDefault(f => f.ProductId == p.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                             ConvertImageToEntity.Convert(p.ProductImages.FirstOrDefault(f => f.ProductId == p.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images) : new Pyramid.Entity.Image(),
                Id = p.Id,
                Title = p.Title,
                Price = p.Price,
                TypePrice = (Common.TypeProductPrice)p.TypePrice,
                TypeStatusProduct=( Common.TypeStatusProduct)p.TypeStatusProduct
            }).ToList();

            if (dbObject.Faq!=null)
            {
                hEntity.Faq = new FAQ()
                {
                    Id = dbObject.Faq.Id,
                    Title = dbObject.Faq.Title,
                    QuestionAnswer = dbObject.Faq.QuestionAnswer.Select(s => new Pyramid.Entity.QuestionAnswer()
                    {
                        Id = s.Id,
                        Answer = s.Answer,
                        Question = s.Question

                    }).ToList()
                };
            }
            


            return hEntity;

        }

        protected override Pyramid.Entity.HomeEntity ConvertDbObjectToEntityShort(PyramidFinalContext context, HomeEntity dbObject)
        {
            var hEntity = new Pyramid.Entity.HomeEntity()
            {
                Title = dbObject.Title,
                Id=dbObject.Id
            };

            return hEntity;
        }
        protected override IQueryable<HomeEntity> BuildDbObjectsList(PyramidFinalContext context, IQueryable<HomeEntity> dbObjects, SearchParamsBase searchParams)
        {
            dbObjects = dbObjects.OrderBy(item => item.Title).ThenBy(item => item.Id);
            return dbObjects;
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, HomeEntity dbEntity, Pyramid.Entity.HomeEntity entity, bool exists)
        {
            dbEntity.Title = entity.Title;
            if (entity.Content != null)
            {
                dbEntity.Content = entity.Content;
            }
            dbEntity.LinkYouTobe = entity.LinkYouTobe;
            if (entity.ThumbnailId != 0)
            {
                dbEntity.ThumbnailId = entity.ThumbnailId;

            }

            dbEntity.CallToAction = entity.CallToAction;
            dbEntity.TitleVideoGuide = entity.TitleVideoGuide;

        }

        public override void UpdateAfterSaving(PyramidFinalContext dbContext, HomeEntity dbEntity, Pyramid.Entity.HomeEntity entity, bool exists)
        {
            dbEntity.Categories.Clear();
            foreach (var item in entity.Categories)
            {
                var efCat = dbContext.Categories.Find(item.Id);
                if (efCat != null)
                {
                    dbEntity.Categories.Add(efCat);
                }

            }

            dbEntity.Products.Clear();
            //Save();
            foreach (var item in entity.Products)
            {
                var efPr = dbContext.Products.Find(item.Id);
                if (efPr != null)
                {
                    dbEntity.Products.Add(efPr);
                }

            }
            var efFaq = dbContext.Faq.Find(entity.Faq.Id);
            if (efFaq != null)
            {
                dbEntity.Faq = efFaq;
            }
            //Save();
            if (dbEntity.BannerWithPoints == null)
            {
                dbEntity.BannerWithPoints = new BannerWithPoints()
                {
                    BannerId = entity.Id,
                    ImageId = entity.BannerWithPoints.Images.Id != 0 ? (int?)entity.BannerWithPoints.Images.Id : null
                };
            }
            else
            {

                dbEntity.BannerWithPoints.ImageId = entity.BannerWithPoints.Images.Id != 0 ? (int?)entity.BannerWithPoints.Images.Id : null;
            }
            //efHomeModel.BannerWithPoints.PointOnImgs.Clear();

            //Save();

            if (dbEntity.BannerWithPoints.PointOnImgs != null && dbEntity.BannerWithPoints.PointOnImgs.Count > 0)
            {
                List<PointOnImgs> oldImgs = new List<PointOnImgs>(dbEntity.BannerWithPoints.PointOnImgs);

                foreach (var item in oldImgs)
                {
                    var efpoint = dbEntity.BannerWithPoints.PointOnImgs.FirstOrDefault(i => i.Id == item.Id);
                    if (efpoint != null)
                    {
                        //efHomeModel.BannerWithPoints.PointOnImgs.Remove(efpoint);
                        dbContext.PointOnImgs.Remove(efpoint);
                    }

                }
            }
            //Save();
            foreach (var item in entity.BannerWithPoints.PointOnImgs)
            {
                dbEntity.BannerWithPoints.PointOnImgs.Add(new PointOnImgs()
                {
                    BannerId = dbEntity.BannerWithPoints.BannerId,
                    CoordX = (int)item.CoordX,
                    CoordY = (int)item.CoordY,
                    Id = item.Id,
                    ReferenceProductId = item.Products != null ? item.Products.Id : (int?)null
                });
            }
        }
    }
}
