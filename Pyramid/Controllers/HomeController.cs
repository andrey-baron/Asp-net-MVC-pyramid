using AutoMapper;
using DBFirstDAL.Repositories;
using PagedList;
using Pyramid.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class HomeController : BaseController
    {
        CategoryRepository _categoryRepository;
        HomeEntityRepository _homeEntityRepository;
        ProductRepository _productRepository;

        

        public HomeController()
        {
            _categoryRepository = new CategoryRepository();
            _homeEntityRepository = new HomeEntityRepository();
            _productRepository = new ProductRepository();
        }
        public ActionResult Index()
        {
            var efRootCategories = _categoryRepository.GetRootCategoriesWithThumbnail((int)Entity.Enumerable.TypeImage.Thumbnail);

            var config = new MapperConfiguration(cfg =>
            {
                #region home config
                cfg.CreateMap<DBFirstDAL.DataModels.CategoryWithThumbnail, Models.CategoryModels.HeaderCategoryViewModel>();

               // cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.HomeEntityModel, Pyramid.Entity.HomeEntity>()
               //;
                cfg.CreateMap<DBFirstDAL.HomeEntity, Pyramid.Entity.HomeEntity>()
                ;
                cfg.CreateMap<DBFirstDAL.BannerWithPoints, Pyramid.Entity.BannerWithPoints>();

                cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.BannerWithPointsHomeDataModel, Pyramid.Entity.BannerWithPoints>();

                cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.PointOnImgsDataModel, Pyramid.Entity.PointOnImg>();


                cfg.CreateMap<DBFirstDAL.PointOnImgs, Pyramid.Entity.PointOnImg>();


                //   cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.CategoryHomeModel, Entity.Category>()
                //   .ForMember(d => d.Checked, o => o.Ignore())
                //   .ForMember(d => d.Filters, o => o.Ignore())
                //   .ForMember(d => d.Thumbnail, o => o.Ignore())
                //   .ForMember(d => d.ParentId, o => o.Ignore())
                //   .ForMember(d => d.FlagRoot, o => o.Ignore())
                //    .ForMember(d => d.OneCId, o => o.Ignore())
                //;

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
                 .ForMember(d => d.OneCId, o => o.Ignore())
                  .ForMember(d => d.PopularCount, o => o.Ignore())
                  .ForMember(d => d.IsFilled, o => o.Ignore())
                   .ForMember(d => d.IsPriority, o => o.Ignore())
                   .ForMember(d => d.TypeStatusProduct, o => o.Ignore())
                ;
                cfg.CreateMap<DBFirstDAL.Images, Entity.Image>();

                cfg.CreateMap<DBFirstDAL.Faq, Entity.FAQ>()
              ;
                cfg.CreateMap<DBFirstDAL.QuestionAnswer, Entity.QuestionAnswer>();



                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
              .ForMember(d => d.EnumValues, o => o.Ignore())
               .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                 .ForMember(d => d.ProductValues, o => o.Ignore())
                  .ForMember(d => d.ThumbnailId, o => o.Ignore())
                                     .ForMember(d => d.ThumbnailImg, o => o.
                MapFrom(m =>
                m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()))

                     ;

                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
               .ForMember(d => d.Thumbnail, o => o.Ignore())
               .ForMember(d => d.Checked, o => o.Ignore())
               .ForMember(d => d.Filters, o => o.Ignore())
               .ForMember(d => d.ParentId, o => o.Ignore())
               .ForMember(d => d.Products, o => o.Ignore())
               .ForMember(d => d.Thumbnail, o => o.Ignore())
               .ForMember(d => d.Seo, o => o.Ignore())
               .ForMember(d => d.SeoId, o => o.Ignore());
                #endregion


            });
            

            #region enable valid
            config.AssertConfigurationIsValid();
            #endregion

            #region init mappers
            var mapper = config.CreateMapper();
            #endregion

            var headerCategories = mapper.Map<IEnumerable<DBFirstDAL.DataModels.CategoryWithThumbnail>, List<Models.CategoryModels.HeaderCategoryViewModel>>(efRootCategories);
            ViewBag.HeaderCategories = headerCategories;
            var efHomeModels = _homeEntityRepository.GetModels(false);
            var homeEntitiesModel = efHomeModels.ToList();

            var efProducts = _productRepository.GetSeasonOffers((int)Entity.Enumerable.TypeImage.Thumbnail);
            ViewBag.SeasonOffers = mapper.Map<IEnumerable<DBFirstDAL.DataModels.HomeModels.ProductHomeModel>, List<Entity.Product>>(efProducts);

            //var rootCategories = _categoryRepository.GetRootCategoriesWithSubs();

            //var modelRootCategories =
            //    mapperForFooter.Map<IEnumerable<DBFirstDAL.DataModels.RootCategory>, IEnumerable<Models.AllCategoriesViewModel>>(rootCategories);

            //ViewBag.FooterCategories = modelRootCategories;

            ViewBag.MetaTitle = "Пирамида строй";
            return View(homeEntitiesModel);
        }

       public ActionResult GlobalSearch(string currentFilter, string searchString, int? page)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Images, Pyramid.Entity.Image>();
                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.ProductValues, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.MapFrom(p=>
                (p.ProductImages.FirstOrDefault(h=>h.ProductId==p.Id&&h.TypeImage==(int)Entity.Enumerable.TypeImage.Thumbnail)) != null ?
                p.ProductImages.FirstOrDefault(h => h.ProductId == p.Id && h.TypeImage == (int)Entity.Enumerable.TypeImage.Thumbnail).Images: new DBFirstDAL.Images()))
                .ForMember(d => d.Images, o => o.Ignore())
                ;
                //cfg.CreateMap<IPagedList<DBFirstDAL.Products>, IPagedList<Entity.Product>>()
                //;

            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var efProducts = _productRepository.GetAllWithThumbnail((int)Entity.Enumerable.TypeImage.Thumbnail);


            if (!String.IsNullOrEmpty(searchString))
            {
                efProducts = efProducts.Where(s => s.Title.Contains(searchString));
            }

            var efProductsList = efProducts.ToList();
            int pageNumber = (page ?? 1);
            var modelList = new PagedList<Entity.Product>(
            efProductsList.Select(u => mapper.Map<DBFirstDAL.Products, Entity.Product>(u)).AsQueryable(),
            pageNumber, Config.PageSize);

            return View(modelList);
        }
    }
}