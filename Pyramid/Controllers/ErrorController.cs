using DBFirstDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class ErrorController : BaseController    {
        CategoryRepository _categoryRepository;
        ProductRepository _productRepository;

        public ErrorController()
        {
            _categoryRepository = new CategoryRepository();
            _productRepository = new ProductRepository();
        }
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            var headerCategories = _categoryRepository.GetRootCategoriesWithThumbnail((int)Entity.Enumerable.TypeImage.Thumbnail);
            ViewBag.HeaderCategories = headerCategories;
            //var homeModels = _homeEntityRepository.GetModels(false);
            var products = _productRepository.GetSeasonOffers((int)Entity.Enumerable.TypeImage.Thumbnail);

            //var banners = _bannersOnHomePageRepository.GetAll();
            //ViewBag.BannersOnHomePage = banners;
            ViewBag.SeasonOffers = products;
            ViewBag.MetaTitle = "Пирамида строй";
            return View();
        }

        public ActionResult Forbidden()
        {
            Response.StatusCode = 403;
            return View();
        }
    }
}