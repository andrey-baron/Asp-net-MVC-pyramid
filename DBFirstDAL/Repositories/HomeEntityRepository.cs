﻿using DBFirstDAL.DataModels.HomeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DBFirstDAL.Repositories
{
    public class HomeEntityRepository : GenericRepository<PyramidFinalContext, HomeEntity>
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

        public IEnumerable< HomeEntity> GetModels(bool adminManage)
        {
            if (adminManage)
            {
                return GetAll()as IEnumerable<HomeEntity>;
            }
            else
            {
                var efEntity = Context.HomeEntity
                    .Include(b => b.BannerWithPoints)
                    .Include(b => b.Faq)
                    .Include(b => b.VideoGuide)
                    .Include(i => i.Products)
                    .Include(i => i.Categories)
                    .ToList();
                //var t= efEntity.Select(i => new HomeEntityModel()
                //{
                //    Id = i.Id,
                //    Title = i.Title,
                //    Category= ToModelCategory(i.Categories),
                //   Faq=i.Faq,
                //   BannerWithPoints= new BannerWithPointsHomeDataModel()
                //   {
                //       BannerId = i.BannerWithPoints.BannerId,
                //       Images = i.BannerWithPoints.Images,
                //       PointOnImgs = i.BannerWithPoints.PointOnImgs.Select(s => new PointOnImgsDataModel()
                //       {
                //           BannerId = s.BannerId,
                //           CoordX = s.CoordX,
                //           CoordY = s.CoordY,
                //           Products = ToModelProduct(s.Products),
                //           Id = s.Id
                //       }).ToList()
                //   }, 
                //   VideoGuide=i.VideoGuide 
                //});
                return efEntity.ToList();
            }
           
        }

        private ProductHomeModel ToModelProduct(Products product)
        {
            var model = new ProductHomeModel();
            if (product!=null)
            {
                model.Id = product.Id;
                model.InStock = product.InStock.HasValue ? product.InStock.Value:false ;
                model.Price = product.Price;
                model.SeasonOffer = product.SeasonOffer.HasValue? product.SeasonOffer.Value:false;
                model.ThumbnailImg = product.ProductImages.FirstOrDefault(i => i.TypeImage == 1 && i.ProductId == product.Id) != null ? product.ProductImages.FirstOrDefault(i => i.TypeImage == 1 && i.ProductId == product.Id).Images : null;
                model.Title = product.Title;
                model.TypePrice = product.TypePrice;
                
            }
            return model;
        }

        private CategoryHomeModel ToModelCategory(Categories cat)
        {
            CategoryHomeModel catModel = new CategoryHomeModel();
            if (cat!=null)
            {
                catModel.Title = cat.Title;
                catModel.Id = cat.Id;
            }
            
            return catModel;
        }
        
        public void AddOrUpdateModel(HomeEntity model)
        {
            var efHomeModel = FindBy(i => i.Id == model.Id).SingleOrDefault();
            if (efHomeModel==null)
            {
                efHomeModel = new HomeEntity();

                efHomeModel.Title = model.Title;
                if (model.Content!=null)
                {
                    efHomeModel.Content = model.Content;

                }
                Context.HomeEntity.Add(efHomeModel);
                Save();
                foreach (var item in model.Categories)
                {
                    var efCat = Context.Categories.Find(item.Id);
                    if (efCat != null)
                    {
                        efHomeModel.Categories.Add(efCat);
                    }
                    
                }
                foreach (var item in model.Products)
                {
                    var efPr = Context.Products.Find(item.Id);
                    if (efPr != null)
                    {
                        efHomeModel.Products.Add(efPr);
                    }

                }
                var efFaq = Context.Faq.Find(model.Faq.Id);
                if (efFaq != null)
                {
                    efHomeModel.Faq = efFaq;
                }
                Save();
                efHomeModel.BannerWithPoints = new BannerWithPoints()
                {
                    BannerId = model.Id,
                    ImageId = model.BannerWithPoints.Images.Id
                };
                foreach (var item in model.BannerWithPoints.PointOnImgs)
                {
                    efHomeModel.BannerWithPoints.PointOnImgs.Add(new PointOnImgs()
                    {
                        BannerId = efHomeModel.BannerWithPoints.BannerId,
                        CoordX = item.CoordX,
                        CoordY = item.CoordY,
                        Id = item.Id,
                        ReferenceProductId = item.Products != null ? item.Products.Id : (int?)null
                    });
                }
                Save();
            }
            else
            {
                efHomeModel.Title = model.Title;
                if (model.Content != null)
                {
                    efHomeModel.Content = model.Content;
                    Save();
                }
                efHomeModel.Categories.Clear();
                foreach (var item in model.Categories)
                {
                    var efCat = Context.Categories.Find(item.Id);
                    if (efCat != null)
                    {
                        efHomeModel.Categories.Add(efCat);
                    }

                }
                efHomeModel.Products.Clear();
                foreach (var item in model.Products)
                {
                    var efPr = Context.Products.Find(item.Id);
                    if (efPr != null)
                    {
                        efHomeModel.Products.Add(efPr);
                    }

                }
                var efFaq = Context.Faq.Find(model.Faq.Id);
                if (efFaq != null)
                {
                    efHomeModel.Faq = efFaq;
                }
                if (efHomeModel.BannerWithPoints == null)
                {
                    efHomeModel.BannerWithPoints = new BannerWithPoints() {
                        BannerId =model.Id,
                        ImageId=model.BannerWithPoints.Images.Id
                    };
                }
                else
                {
                    
                    efHomeModel.BannerWithPoints.ImageId = model.BannerWithPoints.Images.Id;
                }
                //efHomeModel.BannerWithPoints.PointOnImgs.Clear();

                

                if (efHomeModel.BannerWithPoints.PointOnImgs!=null&& efHomeModel.BannerWithPoints.PointOnImgs.Count>0)
                {
                    List<PointOnImgs> oldImgs = new List<PointOnImgs>(efHomeModel.BannerWithPoints.PointOnImgs);

                    foreach (var item in oldImgs)
                    {
                        var efpoint = efHomeModel.BannerWithPoints.PointOnImgs.FirstOrDefault(i => i.Id == item.Id);
                        if (efpoint!=null)
                        {
                            //efHomeModel.BannerWithPoints.PointOnImgs.Remove(efpoint);
                             Context.PointOnImgs.Remove(efpoint);
                        }

                    }
                }
                Save();
                foreach (var item in model.BannerWithPoints.PointOnImgs)
                {
                    efHomeModel.BannerWithPoints.PointOnImgs.Add(new PointOnImgs()
                    {
                        BannerId = efHomeModel.BannerWithPoints.BannerId,
                        CoordX = item.CoordX,
                        CoordY = item.CoordY,
                        Id = item.Id,
                        ReferenceProductId = item.Products!=null? item.Products.Id: (int?) null
                    });
                }
                Save();
               
            }
        }

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
            var efPoint = Context.PointOnImgs.Find(pointId);
            if (efPoint!=null)
            {
                Context.PointOnImgs.Remove(efPoint);
                Context.SaveChanges();
            }
        }

        public void DeleteProduct(int homeEntityId,int productId)
        {
            var efEntity = FindBy(i => i.Id == homeEntityId).SingleOrDefault();
            var efProd = Context.Products.Find(productId);
            if (efProd!=null)
            {
                efEntity.Products.Remove(efProd);
            }
            Save();
        }
        public void DeleteCategory(int homeEntityId, int categoryId )
        {
            var efEntity = FindBy(i => i.Id == homeEntityId).SingleOrDefault();
            var cat = Context.Categories.Find(categoryId);
            if (cat != null)
            {
                efEntity.Categories.Remove(cat);
            }
            Save();
        }
    }
}
