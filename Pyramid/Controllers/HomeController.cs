using AutoMapper;
using Common.Models;
using Common.SearchClasses;
using DBFirstDAL.Repositories;
using PagedList;
using Pyramid.Entity;
using Pyramid.Global;
using Pyramid.Models.CommonViewModels;
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

            //var config = new MapperConfiguration(cfg =>
            //{
            //    #region home config
            //    cfg.CreateMap<DBFirstDAL.DataModels.CategoryWithThumbnail, Models.CategoryModels.HeaderCategoryViewModel>();

            //   // cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.HomeEntityModel, Pyramid.Entity.HomeEntity>()
            //   //;
            //    cfg.CreateMap<DBFirstDAL.HomeEntity, Pyramid.Entity.HomeEntity>()
            //    ;
            //    cfg.CreateMap<DBFirstDAL.BannerWithPoints, Pyramid.Entity.BannerWithPoints>();

            //    cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.BannerWithPointsHomeDataModel, Pyramid.Entity.BannerWithPoints>();

            //    cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.PointOnImgsDataModel, Pyramid.Entity.PointOnImg>();


            //    cfg.CreateMap<DBFirstDAL.PointOnImgs, Pyramid.Entity.PointOnImg>();


            //    //   cfg.CreateMap<DBFirstDAL.DataModels.HomeModels.CategoryHomeModel, Entity.Category>()
            //    //   .ForMember(d => d.Checked, o => o.Ignore())
            //    //   .ForMember(d => d.Filters, o => o.Ignore())
            //    //   .ForMember(d => d.Thumbnail, o => o.Ignore())
            //    //   .ForMember(d => d.ParentId, o => o.Ignore())
            //    //   .ForMember(d => d.FlagRoot, o => o.Ignore())
            //    //    .ForMember(d => d.OneCId, o => o.Ignore())
            //    //;

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
            //     .ForMember(d => d.OneCId, o => o.Ignore())
            //      .ForMember(d => d.PopularCount, o => o.Ignore())
            //      .ForMember(d => d.IsFilled, o => o.Ignore())
            //       .ForMember(d => d.IsPriority, o => o.Ignore())
            //       .ForMember(d => d.TypeStatusProduct, o => o.Ignore())
            //    ;
            //    cfg.CreateMap<DBFirstDAL.Images, Entity.Image>();

            //    cfg.CreateMap<DBFirstDAL.Faq, Entity.FAQ>()
            //  ;
            //    cfg.CreateMap<DBFirstDAL.QuestionAnswer, Entity.QuestionAnswer>();



            //    cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
            //  .ForMember(d => d.EnumValues, o => o.Ignore())
            //   .ForMember(d => d.Categories, o => o.Ignore())
            //    .ForMember(d => d.Images, o => o.Ignore())
            //     .ForMember(d => d.ProductValues, o => o.Ignore())
            //      .ForMember(d => d.ThumbnailId, o => o.Ignore())
            //                         .ForMember(d => d.ThumbnailImg, o => o.
            //    MapFrom(m =>
            //    m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
            //    m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()))

            //         ;

            //    cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
            //   .ForMember(d => d.Thumbnail, o => o.Ignore())
            //   .ForMember(d => d.Checked, o => o.Ignore())
            //   .ForMember(d => d.Filters, o => o.Ignore())
            //   .ForMember(d => d.ParentId, o => o.Ignore())
            //   .ForMember(d => d.Products, o => o.Ignore())
            //   .ForMember(d => d.Thumbnail, o => o.Ignore())
            //   .ForMember(d => d.Seo, o => o.Ignore())
            //   .ForMember(d => d.SeoId, o => o.Ignore());
            //    #endregion


            //});
            

            //#region enable valid
            //config.AssertConfigurationIsValid();
            //#endregion

            //#region init mappers
            //var mapper = config.CreateMapper();
            //#endregion

           

            var headerCategories = _categoryRepository.GetRootCategoriesWithThumbnail((int)Entity.Enumerable.TypeImage.Thumbnail);
            ViewBag.HeaderCategories = headerCategories;
            var homeModels = _homeEntityRepository.GetModels(false);
           

            var products = _productRepository.GetSeasonOffers((int)Entity.Enumerable.TypeImage.Thumbnail);
            ViewBag.SeasonOffers = products;


            ViewBag.MetaTitle = "Пирамида строй";
            return View(homeModels);
        }

       public ActionResult GlobalSearch(string currentFilter, string searchString, int? page)
        {
            var pageNumber = page ?? 1;
            if (string.IsNullOrWhiteSpace(searchString))
            {
                searchString = null;
            }
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var objectsPerPage = 20;
            var startIndex = (pageNumber - 1) * objectsPerPage;

            SearchParamsProduct SearchParams = new SearchParamsProduct(searchString, startIndex, objectsPerPage);
            SearchResult<Product> searchResult;
            if (searchString==null)
            {
                 searchResult = new SearchResult<Product>()
                 {
                     Objects = new List<Product>(),
                     RequestedStartIndex = 0,
                     Total = objectsPerPage,
                     RequestedObjectsCount = objectsPerPage
                 };
            }
            else
            {
                 searchResult = _productRepository.Get(SearchParams);
            }
          
            ViewBag.CurrentFilter = searchString;
            var viewModel = SearchResultViewModel<Product>.CreateFromSearchResult(searchResult, i => i, 10);
            List<BreadCrumbViewModel> breadcrumbs = new List<BreadCrumbViewModel>();
            breadcrumbs.Add(new BreadCrumbViewModel()
            {
                Title = "Поиск"
            });
            ViewBag.BredCrumbs = breadcrumbs;
            ViewBag.MetaTitle = "Поиск: " + searchString;
            return View(viewModel);

            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<DBFirstDAL.Images, Pyramid.Entity.Image>();
            //    cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
            //    .ForMember(d => d.EnumValues, o => o.Ignore())
            //    .ForMember(d => d.Categories, o => o.Ignore())
            //    .ForMember(d => d.ProductValues, o => o.Ignore())
            //    .ForMember(d => d.ThumbnailId, o => o.Ignore())
            //    .ForMember(d => d.ThumbnailImg, o => o.MapFrom(p=>
            //    (p.ProductImages.FirstOrDefault(h=>h.ProductId==p.Id&&h.TypeImage==(int)Entity.Enumerable.TypeImage.Thumbnail)) != null ?
            //    p.ProductImages.FirstOrDefault(h => h.ProductId == p.Id && h.TypeImage == (int)Entity.Enumerable.TypeImage.Thumbnail).Images: new DBFirstDAL.Images()))
            //    .ForMember(d => d.Images, o => o.Ignore())
            //    ;
            //    //cfg.CreateMap<IPagedList<DBFirstDAL.Products>, IPagedList<Entity.Product>>()
            //    //;

            //});
            //config.AssertConfigurationIsValid();

            //var mapper = config.CreateMapper();

            //var pageNumber = page ?? 1;
            //if (searchString != null)
            //{
            //    page = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}
            //var objectsPerPage = 20;
            //var startIndex = (pageNumber - 1) * objectsPerPage;

            //SearchParamsProduct SearchParams = new SearchParamsProduct(searchString,null,null, startIndex, objectsPerPage);

            //var searchResult = _productRepository.Get(SearchParams);
            //var viewModel = SearchResultViewModel<Product>.CreateFromSearchResult(searchResult, i => i, 10);

            //ViewBag.CurrentFilter = searchString;

            //var efProducts = _productRepository.GetAllWithThumbnail((int)Entity.Enumerable.TypeImage.Thumbnail);


            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    efProducts = efProducts.Where(s => s.Title.Contains(searchString));
            //}

            //var efProductsList = efProducts.ToList();
            //int pageNumber = (page ?? 1);
            //var modelList = new PagedList<Entity.Product>(
            //efProductsList.Select(u => mapper.Map<DBFirstDAL.Products, Entity.Product>(u)).AsQueryable(),
            //pageNumber, Config.PageSize);

            //return View(viewModel);
        }
    }
}