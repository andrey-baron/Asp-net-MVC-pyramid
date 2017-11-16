using Common.Models;
using Common.SearchClasses;
using DBFirstDAL.Repositories;
using Entity;
using Pyramid.Entity;
using Pyramid.Models.CommonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
    public class RecommendationController : BaseController
    {
        RecommendationRepository _recommendationRepository;
        EventBannerRepository _eventBannerRepository;
        RouteItemRepository _routeItemRepository;

        public RecommendationController() {
            _recommendationRepository = new RecommendationRepository();
            _eventBannerRepository = new EventBannerRepository();
            _routeItemRepository = new RouteItemRepository();
        }
        // GET: Recommendation
        [AllowAnonymous]
        public ActionResult Index(int? page) {
            var pageNumber = page ?? 1;
            var objectsPerPage = 20;
            var searchResult = _recommendationRepository.Get(new SearchParamsBase
            {
                StartIndex = (pageNumber - 1) * objectsPerPage,
                ObjectsCount = objectsPerPage,
            });

            var viewModel = SearchResultViewModel<Recommendation>.CreateFromSearchResult(searchResult, i => i, 10);
            ViewBag.Banners = _eventBannerRepository.GetAll();
            List<BreadCrumbViewModel> breadcrumbs = new List<BreadCrumbViewModel>();
            breadcrumbs.Add(new BreadCrumbViewModel()
            {
                Title = "Советы"
            });
            ViewBag.BredCrumbs = breadcrumbs;
            ViewBag.MetaTitle = "Советы";
            return View(viewModel);
        
        }
        [AllowAnonymous]
        public ActionResult Get(int id)
        {
            var model = _recommendationRepository.Get(id);
            if (model != null)
            {
                List<BreadCrumbViewModel> breadcrumbs = new List<BreadCrumbViewModel>();
                breadcrumbs.Add(new BreadCrumbViewModel()
                {
                    Link = "/Recommendation/Index",
                    Title = "Советы"
                });
                breadcrumbs.Add(new BreadCrumbViewModel()
                {
                    Title = model.Title
                });
                ViewBag.BredCrumbs = breadcrumbs;
            }

            ViewBag.Banners = _eventBannerRepository.GetAll();
            ViewBag.MetaTitle = model.Title;
            return View(model);
        }

        public ActionResult AdminIndex(int? page)
        {
            var pageNumber = page ?? 1;
            var objectsPerPage = 20;
            var searchResult = _recommendationRepository.Get(new SearchParamsBase
            {
                StartIndex = (pageNumber - 1) * objectsPerPage,
                ObjectsCount = objectsPerPage,
            });

            var viewModel = SearchResultViewModel<Recommendation>.CreateFromSearchResult(searchResult, i => i, 10);

            return View(viewModel);

            //var model = _recommendationRepository.GetAll();
            //return View(model);
        }
        public ActionResult AddOrUpdate(int id = 0)
        {
            var model = _recommendationRepository.Get(id);
            if (model == null)
            {
                model = new Entity.Recommendation();
            }
            var friendlyUrl = _routeItemRepository.GetFriendlyUrl(model.Id, Common.TypeEntityFromRouteEnum.RecommendationType);
            ViewBag.CurrentFriendlyUrl = friendlyUrl;
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(Entity.Recommendation model)
        {
            _recommendationRepository.AddOrUpdate(model);
            var routeItem = new RouteItem(0, null, (string)ControllerContext.RequestContext.RouteData.Values["controller"],
             "Get",
             model.Id)
            { Type = Common.TypeEntityFromRouteEnum.RecommendationType };

            _routeItemRepository.AddOrUpdate(routeItem);
            return RedirectToAction("AdminIndex");

        }
    }
}