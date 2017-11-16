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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tools;

namespace Pyramid.Controllers
{
    public class HomeController : BaseController
    {
        CategoryRepository _categoryRepository;
        HomeEntityRepository _homeEntityRepository;
        ProductRepository _productRepository;
        BannersOnHomePageRepository _bannersOnHomePageRepository;

        public HomeController()
        {
            _categoryRepository = new CategoryRepository();
            _homeEntityRepository = new HomeEntityRepository();
            _productRepository = new ProductRepository();
            _bannersOnHomePageRepository = new BannersOnHomePageRepository();
        }
        public ActionResult Index()
        {
            var headerCategories = _categoryRepository.GetRootCategoriesWithThumbnail((int)Entity.Enumerable.TypeImage.Thumbnail);
            ViewBag.HeaderCategories = headerCategories;
            var homeModels = _homeEntityRepository.GetModels(false);
            var products = _productRepository.GetSeasonOffers((int)Entity.Enumerable.TypeImage.Thumbnail);

            var banners = _bannersOnHomePageRepository.GetAll();
            ViewBag.BannersOnHomePage = banners;
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

            SearchParamsProduct SearchParams = new SearchParamsProduct(searchString, startIndex, objectsPerPage)
            { IsSearchOnlyPublicProduct=true};
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
        }

        public FileResult xmlMap() {
            SiteMapGenerator generator = new SiteMapGenerator("http://pyramidastroy.com", AppDomain.CurrentDomain.BaseDirectory);
            var _routeItemRepository = new RouteItemRepository();
            var items = _routeItemRepository.GetAll().Select(s => new SiteMapGenerator.SiteMapNode(s.FriendlyUrl, DateTime.Now, SiteMapGenerator.ChangeFrequency.Monthly));
            try
            {
                generator.SaveMap(items);
            }
            catch (Exception ex)
            {

                throw;
            }
            
            return File(generator.SiteMapPath, "text/xml");
           
        }
    }
}