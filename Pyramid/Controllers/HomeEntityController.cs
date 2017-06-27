using AutoMapper;
using DBFirstDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
    public class HomeEntityController : BaseController
    {
        HomeEntityRepository _homeEntityRepository;
        FaqRepository _faqRepository;
        CategoryRepository _categoryRepository;
        public HomeEntityController()
        {
            _homeEntityRepository = new HomeEntityRepository();
            _faqRepository = new FaqRepository();
            _categoryRepository = new CategoryRepository();
        }
        // GET: HomeEntity
        public ActionResult Index()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.HomeEntity, Pyramid.Entity.HomeEntity>()
                .ForMember(d => d.BannerWithPoints, o => o.Ignore())

                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.Products, o => o.Ignore())
                .ForMember(d => d.VideoGuide, o => o.Ignore())
                .ForMember(d => d.Faq, o => o.Ignore())

                ;
              
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var model = mapper.Map<IEnumerable<DBFirstDAL.HomeEntity>, List<Pyramid.Entity.HomeEntity>>(_homeEntityRepository.GetModels(true).ToList());

            return View(model);
        }

        public ActionResult AddOrUpdate(int id=0)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.HomeEntity, Pyramid.Entity.HomeEntity>()
            ;
                //cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.ManageHomeEntityDataModel, Pyramid.Entity.HomeEntity>();

                cfg.CreateMap<DBFirstDAL.BannerWithPoints, Pyramid.Entity.BannerWithPoints>();

                cfg.CreateMap<DBFirstDAL.PointOnImgs, Pyramid.Entity.PointOnImg>();

                //cfg.CreateMap<List<DBFirstDAL.PointOnImgs>, List<Pyramid.Entity.PointOnImg>>();

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                 .ForMember(d => d.EnumValues, o => o.Ignore())
                  .ForMember(d => d.Categories, o => o.Ignore())
                   .ForMember(d => d.Images, o => o.Ignore())
                    .ForMember(d => d.ProductValues, o => o.Ignore())
                     .ForMember(d => d.ThumbnailId, o => o.Ignore())
                        .ForMember(d => d.ThumbnailImg, o => o.Ignore())
                        .ForMember(d => d.OneCId, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
              .ForMember(d => d.Thumbnail, o => o.Ignore())
              .ForMember(d => d.Checked, o => o.Ignore())
              .ForMember(d => d.Filters, o => o.Ignore())
              .ForMember(d => d.ParentId, o => o.Ignore())
              .ForMember(d => d.Products, o => o.Ignore())
              .ForMember(d => d.Thumbnail, o => o.Ignore());

                //   cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.CategoryHomeModel, Entity.Category>()
                //   .ForMember(d => d.Checked, o => o.Ignore())
                //   .ForMember(d => d.Filters, o => o.Ignore())
                //   .ForMember(d => d.Thumbnail, o => o.Ignore())
                //   .ForMember(d => d.ParentId, o => o.Ignore())
                //   .ForMember(d => d.FlagRoot, o => o.Ignore())
                //   .ForMember(d => d.OneCId, o => o.Ignore())
                //;

                //cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.ProductHomeModel, Entity.Product>()
                //.ForMember(d => d.Categories, o => o.Ignore())
                //.ForMember(d => d.EnumValues, o => o.Ignore())
                //.ForMember(d => d.ProductValues, o => o.Ignore())
                //.ForMember(d => d.ThumbnailId, o => o.Ignore())
                //.ForMember(d => d.MetaDescription, o => o.Ignore())
                //.ForMember(d => d.MetaKeywords, o => o.Ignore())
                //.ForMember(d => d.MetaTitle, o => o.Ignore())
                //.ForMember(d => d.Images, o => o.Ignore())
                //.ForMember(d => d.IsSEOReady, o => o.Ignore())
                //.ForMember(d => d.Alias, o => o.Ignore())
                //.ForMember(d => d.DateCreation, o => o.Ignore())
                //.ForMember(d => d.DateChange, o => o.Ignore())
                //.ForMember(d => d.OneCId, o => o.Ignore())
                //;
                cfg.CreateMap<DBFirstDAL.Images, Entity.Image>()
              ;

                cfg.CreateMap<DBFirstDAL.Faq, Entity.FAQ>()
               ;
                cfg.CreateMap<DBFirstDAL.QuestionAnswer, Entity.QuestionAnswer>();

                cfg.CreateMap<DBFirstDAL.VideoGuide, Entity.VideoGuide>();

            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var efModel = _homeEntityRepository.GetManageModel( id);
            Entity.HomeEntity model;
            if (efModel!=null)
            {
                //efModel.BannerWithPoints.PointOnImgs = efModel.BannerWithPoints.PointOnImgs.ToList();
                model = mapper.Map<Entity.HomeEntity>(efModel);
                
            }
            else
            {
                 model = new Entity.HomeEntity();
            }
            ViewBag.CategoriesSelectListItem = _categoryRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });

            ViewBag.FaqSelectListItem = _faqRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(Entity.HomeEntity model)
            {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Pyramid.Entity.HomeEntity, DBFirstDAL.HomeEntity>();

                cfg.CreateMap<Pyramid.Entity.BannerWithPoints, DBFirstDAL.BannerWithPoints>()
                .ForMember(d => d.HomeEntity, o => o.Ignore())
                .ForMember(d => d.ImageId, o => o.Ignore());

                cfg.CreateMap<Pyramid.Entity.PointOnImg, DBFirstDAL.PointOnImgs>()
               .ForMember(d => d.BannerWithPoints, o => o.Ignore())
               .ForMember(d => d.BannerId, o => o.Ignore())
               .ForMember(d => d.ReferenceProductId, o => o.Ignore()); 

                cfg.CreateMap<Entity.Category, DBFirstDAL.Categories>()
                .ForMember(d => d.Categories1, o => o.Ignore())
                .ForMember(d => d.Categories2, o => o.Ignore())
                .ForMember(d => d.CategoryImages, o => o.Ignore())
                .ForMember(d => d.HomeEntity, o => o.Ignore())
                .ForMember(d => d.Recommendations, o => o.Ignore())
                .ForMember(d => d.Filters, o => o.Ignore())
             ;

                //cfg.CreateMap<Entity.Product, DBFirstDAL.Products>()
                //;

                cfg.CreateMap<Entity.Image, DBFirstDAL.Images>()
                .ForMember(d => d.BannerWithPoints, o => o.Ignore())
                .ForMember(d => d.CategoryImages, o => o.Ignore())
                .ForMember(d => d.ProductImages, o => o.Ignore())
                .ForMember(d => d.Recommendations, o => o.Ignore())
                .ForMember(d => d.EventImages, o => o.Ignore())

              ;

                cfg.CreateMap<Entity.FAQ, DBFirstDAL.Faq>()
                .ForMember(d => d.HomeEntity, o => o.Ignore())
                .ForMember(d => d.QuestionAnswer, o => o.Ignore())
               ;


                cfg.CreateMap<Entity.VideoGuide, DBFirstDAL.VideoGuide>()
                  .ForMember(d => d.HomeEntity, o => o.Ignore())
                    .ForMember(d => d.HomeEntityId, o => o.Ignore());

                cfg.CreateMap<Entity.Product,DBFirstDAL.Products>()
                 .ForMember(d => d.Categories, o => o.Ignore())
                 .ForMember(d => d.EnumValues, o => o.Ignore())
                 .ForMember(d => d.Review, o => o.Ignore())
                 .ForMember(d => d.PointOnImgs, o => o.Ignore())
                 .ForMember(d => d.PointOnImg_Id, o => o.Ignore())
                 .ForMember(d => d.ProductImages, o => o.Ignore())
                  .ForMember(d => d.ProductValues, o => o.Ignore())
                  .ForMember(d => d.ProductOrders, o => o.Ignore())
                  .ForMember(d => d.HomeEntity, o => o.Ignore())
                   .ForMember(d => d.Events, o => o.Ignore())
                 ;
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var efModel = mapper.Map<DBFirstDAL.HomeEntity>(model);
            _homeEntityRepository.AddOrUpdateModel(efModel);
            return RedirectToAction("Index");
        }

        public ActionResult JsonGetNewPoint(int id,int count)
        {
            var efHomeEntity=_homeEntityRepository.FindBy(i => i.Id == id).SingleOrDefault();
            var model = 0;
            if (efHomeEntity!=null)
            {
                int countInDb = efHomeEntity.BannerWithPoints!=null? efHomeEntity.BannerWithPoints.PointOnImgs.Count:0;
                if (count>countInDb)
                {
                    model = count;
                }
                else
                {
                    model = countInDb;
                }
            }
            else
            {
                model = count;
            }
            return JsonTemplateNewPoint(model);
        }
        public ActionResult JsonTemplateNewPoint(int indexPoint)
        {
            var j = new JsonResult();
            ViewBag.CategoriesSelectListItem = _categoryRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            var model = new Models.JsonModels.TemplatePointJsonModel() {
                Data =  RenderViewToString(ControllerContext, "_PartialTemplateNewPoint", indexPoint, true),
                Id=indexPoint
            };
            
            return Json(model,JsonRequestBehavior.AllowGet);
            
            //return PartialView("_PartialTemplateNewPoint", indexPoint);
        }
        
       public ActionResult DeleteHomeEntity(int id)
        {
            var efEntity = _homeEntityRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (efEntity!=null)
            {
                _homeEntityRepository.Delete(efEntity);
                _homeEntityRepository.Save();
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeletePoint(int id)
        {
            _homeEntityRepository.DeletePoint(id);
            return null;
        }
        public ActionResult PartialAllPoints(int id,bool isview)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.HomeEntityModel, Pyramid.Entity.HomeEntity>()
                 .ForMember(d => d.Categories, o => o.Ignore())
                  .ForMember(d => d.Faq, o => o.Ignore())
                  .ForMember(d => d.VideoGuide, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.HomeEntity, Pyramid.Entity.HomeEntity>()
                .ForMember(d => d.Categories, o => o.Ignore())
                 .ForMember(d => d.Faq, o => o.Ignore())
                 .ForMember(d => d.Products, o => o.Ignore())
                 .ForMember(d => d.VideoGuide, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.BannerWithPoints, Pyramid.Entity.BannerWithPoints>();

                cfg.CreateMap<DBFirstDAL.PointOnImgs, Pyramid.Entity.PointOnImg>();

                //cfg.CreateMap<List<DBFirstDAL.PointOnImgs>, List<Pyramid.Entity.PointOnImg>>();

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                 .ForMember(d => d.EnumValues, o => o.Ignore())
                  .ForMember(d => d.Categories, o => o.Ignore())
                   .ForMember(d => d.Images, o => o.Ignore())
                    .ForMember(d => d.ProductValues, o => o.Ignore())
                     .ForMember(d => d.ThumbnailId, o => o.Ignore())
                        .ForMember(d => d.ThumbnailImg, o => o.Ignore());

            

                cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.ProductHomeModel, Entity.Product>()
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.ProductValues, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore())
                .ForMember(d => d.MetaDescription, o => o.Ignore())
                .ForMember(d => d.MetaKeywords, o => o.Ignore())
                .ForMember(d => d.MetaTitle, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.IsSEOReady, o => o.Ignore())
                .ForMember(d => d.Alias, o => o.Ignore())
                .ForMember(d => d.DateCreation, o => o.Ignore())
                .ForMember(d => d.DateChange, o => o.Ignore())
                ;

                cfg.CreateMap<DBFirstDAL.Images, Entity.Image>()
              ;
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();



            var efModel = _homeEntityRepository.GetModel(id);
            Entity.HomeEntity model;
            if (efModel != null)
            {
                model = mapper.Map<Entity.HomeEntity>(efModel);
            }
            else
            {
                model = new Entity.HomeEntity();
            }
            ViewBag.CategoriesSelectListItem = _categoryRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });


            if(model.BannerWithPoints!=null&& model.BannerWithPoints.PointOnImgs != null){
                if (isview)
                {
                    return PartialView("_PartialAllPointOnImg", model.BannerWithPoints.PointOnImgs);
                }
                else
                {
                    return PartialView("_PartialAllPoints", model.BannerWithPoints.PointOnImgs);
                }
               
            }
            return PartialView("_PartialAllPoints", new List<Pyramid.Entity.PointOnImg>());
        }

        public ActionResult GetTemplateCategory(int entityId, int count)
        {
            var efHomeModel = _homeEntityRepository.FindBy(i => i.Id == entityId).SingleOrDefault();
            var indx = 0;
            if (efHomeModel!=null&& efHomeModel.Categories!=null)
            {
                indx = efHomeModel.Categories.Count();
            }
            if (count > indx)
            {
                indx = count;
            }
            ViewBag.CategoriesSelectListItem = _categoryRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            return PartialView("_PartialHomeEntityTemplateCategory", indx);
        }
        public ActionResult GetTemplateCategoryFromProduct(int entityId, int count)
        {
            var efHomeModel = _homeEntityRepository.FindBy(i => i.Id == entityId).SingleOrDefault();
            var indx = 0;
            if (efHomeModel != null && efHomeModel.Products != null)
            {
                indx = efHomeModel.Products.Count();
            }
            if (count > indx)
            {
                indx = count;
            }
            ViewBag.CategoriesSelectListItem = _categoryRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            return PartialView("_PartialHomeEntityTemplateCategoryFromProduct",indx);
        }
        //public ActionResult GetTemplateDropDownListProduct(int entityId, int count)
        //{
        //    var efHomeModel = _homeEntityRepository.FindBy(i => i.Id == entityId).SingleOrDefault();
        //    var indx = 0;
        //    if (efHomeModel != null && efHomeModel.Products != null)
        //    {
        //        indx = efHomeModel.Products.Count();
        //    }
        //    if (count > indx)
        //    {
        //        indx = count;
        //    }

        //    return PartialView("_PartialHomeEntityTemplateProduct", indx);
        //}

        [Authorize]
        public ActionResult GetProductTemplateDropDownListForCategoryId(int id, int index)
        {
            ViewBag.ProductsSelectListItem = _categoryRepository.GetProductsByCategoryId(id).Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            return PartialView("_PartialGetProductTemplateDropDownListForHomeEntity", index);
        }

        public ActionResult GetTemplateAllCategories(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
            .ForMember(d => d.Checked, o => o.Ignore())
            .ForMember(d => d.Filters, o => o.Ignore())
            .ForMember(d => d.ParentId, o => o.Ignore())
            .ForMember(d => d.Products, o => o.Ignore())
            .ForMember(d => d.Thumbnail, o => o.Ignore());
            });
            var mapper = config.CreateMapper();
            ViewBag.CategoriesSelectListItem = _categoryRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            var efEntity = _homeEntityRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (efEntity!=null)
            {
                var cat = mapper.Map<IEnumerable< Entity.Category>>(efEntity.Categories);
                return PartialView("GetTemplateAllCategories", cat);
            }
            return PartialView("GetTemplateAllCategories", new List<Entity.Category>());
        }
        public ActionResult DeleteCategory(int id,int categoryId)
        {
            _homeEntityRepository.DeleteCategory(id, categoryId);
            return GetTemplateAllCategories(id);
        }
        public ActionResult GetTemplateAllProducts(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                   .ForMember(d => d.EnumValues, o => o.Ignore())
                    .ForMember(d => d.Categories, o => o.Ignore())
                     .ForMember(d => d.Images, o => o.Ignore())
                      .ForMember(d => d.ProductValues, o => o.Ignore())
                       .ForMember(d => d.ThumbnailId, o => o.Ignore())
                          .ForMember(d => d.ThumbnailImg, o => o.Ignore())
                          .ForMember(d => d.OneCId, o => o.Ignore());

            });
            var mapper = config.CreateMapper();
            var efEntity = _homeEntityRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (efEntity!=null)
            {
                var products = mapper.Map<IEnumerable<Entity.Product>>(efEntity.Products);
                return PartialView("GetTemplateAllProducts", products);

            }
            return PartialView("GetTemplateAllProducts", new List<Entity.Product>());

        }
        public ActionResult DeleteProduct(int id,int productId)
        {
            _homeEntityRepository.DeleteProduct(id, productId);

            return GetTemplateAllProducts(id);


        }
    }
}