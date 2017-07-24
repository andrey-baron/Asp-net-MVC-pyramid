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
            var model =_homeEntityRepository.GetModels(true).ToList();
            return View(model);
        }

        public ActionResult AddOrUpdate(int id=0)
        {
            var model = _homeEntityRepository.Get( id);
            if (model == null)
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
            _homeEntityRepository.AddOrUpdate(model);
            return RedirectToAction("Index");
        }

        public ActionResult JsonGetNewPoint(int id,int count)
        {
            var homeEntity=_homeEntityRepository.Get(id);
            var model = 0;
            if (homeEntity != null)
            {
                int countInDb = homeEntity.BannerWithPoints!=null? homeEntity.BannerWithPoints.PointOnImgs.Count:0;
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
        }
        
       public ActionResult DeleteHomeEntity(int id)
        {
                _homeEntityRepository.Delete(id);
               
            return RedirectToAction("Index");
        }
        public ActionResult DeletePoint(int id)
        {
            _homeEntityRepository.DeletePoint(id);
            return null;
        }
        public ActionResult PartialAllPoints(int id,bool isview)
        {
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.HomeEntityModel, Pyramid.Entity.HomeEntity>()
            //     .ForMember(d => d.Categories, o => o.Ignore())
            //      .ForMember(d => d.Faq, o => o.Ignore());

            //    cfg.CreateMap<DBFirstDAL.HomeEntity, Pyramid.Entity.HomeEntity>()
            //    .ForMember(d => d.Categories, o => o.Ignore())
            //     .ForMember(d => d.Faq, o => o.Ignore())
            //     .ForMember(d => d.Products, o => o.Ignore());

            //    cfg.CreateMap<DBFirstDAL.BannerWithPoints, Pyramid.Entity.BannerWithPoints>();

            //    cfg.CreateMap<DBFirstDAL.PointOnImgs, Pyramid.Entity.PointOnImg>();

            //    //cfg.CreateMap<List<DBFirstDAL.PointOnImgs>, List<Pyramid.Entity.PointOnImg>>();

            //    cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
            //     .ForMember(d => d.EnumValues, o => o.Ignore())
            //      .ForMember(d => d.Categories, o => o.Ignore())
            //       .ForMember(d => d.Images, o => o.Ignore())
            //        .ForMember(d => d.ProductValues, o => o.Ignore())
            //         .ForMember(d => d.ThumbnailId, o => o.Ignore())
            //            .ForMember(d => d.ThumbnailImg, o => o.Ignore());

            

            //    cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.ProductHomeModel, Entity.Product>()
            //    .ForMember(d => d.Categories, o => o.Ignore())
            //    .ForMember(d => d.EnumValues, o => o.Ignore())
            //    .ForMember(d => d.ProductValues, o => o.Ignore())
            //    .ForMember(d => d.ThumbnailId, o => o.Ignore())
            //    .ForMember(d => d.MetaDescription, o => o.Ignore())
            //    .ForMember(d => d.MetaKeywords, o => o.Ignore())
            //    .ForMember(d => d.MetaTitle, o => o.Ignore())
            //    .ForMember(d => d.Images, o => o.Ignore())
            //    .ForMember(d => d.IsSEOReady, o => o.Ignore())
            //    .ForMember(d => d.Alias, o => o.Ignore())
            //    .ForMember(d => d.DateCreation, o => o.Ignore())
            //    .ForMember(d => d.DateChange, o => o.Ignore())
            //    ;

            //    cfg.CreateMap<DBFirstDAL.Images, Entity.Image>()
            //  ;
            //});


            //config.AssertConfigurationIsValid();

            //var mapper = config.CreateMapper();



            //var efModel = _homeEntityRepository.GetModel(id);

            Entity.HomeEntity model= _homeEntityRepository.Get(id);
            if (model == null)
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
            var homeModel = _homeEntityRepository.Get(entityId);
            var indx = 0;
            if (homeModel != null&& homeModel.Categories!=null)
            {
                indx = homeModel.Categories.Count();
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
            var homeModel = _homeEntityRepository.Get(entityId);
            var indx = 0;
            if (homeModel != null && homeModel.Products != null)
            {
                indx = homeModel.Products.Count();
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
            var homeModel = _homeEntityRepository.Get(id);
            if (homeModel != null)
            {
                var cat = homeModel.Categories;
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
            var homeModel = _homeEntityRepository.Get(id);
            if (homeModel != null)
            {
                var products = homeModel.Products;
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