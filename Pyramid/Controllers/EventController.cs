using Common.Models;
using Common.SearchClasses;
using DBFirstDAL.Repositories;
using Entity;
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
{   [Authorize]
    public class EventController : BaseController
    {
        private CategoryRepository _categoryRepository;
        private EventRepository _eventRepository;
        EventBannerRepository _eventBannerRepository;
        RouteItemRepository _routeItemRepository;

        public EventController()
        {
            _eventRepository = new EventRepository();
            _categoryRepository = new CategoryRepository();
            _eventBannerRepository = new EventBannerRepository();
            _routeItemRepository = new RouteItemRepository();

        }
        // GET: Event
        [AllowAnonymous]
        public ActionResult Index()
        {
            List<BreadCrumbViewModel> breadcrumbs = new List<BreadCrumbViewModel>();
            breadcrumbs.Add(new BreadCrumbViewModel()
            {
                Title = "Акции"
            });
            ViewBag.BredCrumbs = breadcrumbs;
            var model  = _eventRepository.GetAll().ToList();
            ViewBag.Banners = _eventBannerRepository.GetAll();
            ViewBag.MetaTitle = "Акции";
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Get(int id)
        {
            var model = _eventRepository.Get(id);
            if (model != null)
            {
                List<BreadCrumbViewModel> breadcrumbs = new List<BreadCrumbViewModel>();
                breadcrumbs.Add(new BreadCrumbViewModel()
                {
                    Link = "/Event/Index",
                    Title = "Акции",
                    FriendlyUrl = string.Format("/{0}", new GlobalOptionRepository().Get(Common.Constant.KeyEvent).OptionContent)
                });
                breadcrumbs.Add(new BreadCrumbViewModel()
                {
                    Title = model.Title
                });
                ViewBag.BredCrumbs = breadcrumbs;
            }
            var friendlyUrl = _routeItemRepository.GetFriendlyUrl(model.Id, Common.TypeEntityFromRouteEnum.Event);
            ViewBag.CurrentFriendlyUrl = friendlyUrl;
            ViewBag.Banners = _eventBannerRepository.GetAll();
            ViewBag.MetaTitle = model.Title;

            return View(model);
        }
        public ActionResult ManageIndex(int? page)
        {
            var pageNumber = page ?? 1;

            var objectsPerPage = 20;
            var startIndex = (pageNumber - 1) * objectsPerPage;

            SearchParamsBase SearchParams = new SearchParamsBase(startIndex, objectsPerPage);

            var searchResult = _eventRepository.Get(SearchParams);

            var viewModel = SearchResultViewModel<Pyramid.Entity.Event>.CreateFromSearchResult(searchResult, i => i, 10);

            return View(viewModel);
        }
        public ActionResult AddOrUpdate(int id=0)
        {
           var model= _eventRepository.Get(id);
            if (model == null)
            {
                model = new Event() {
                    DateEventEnd=DateTime.Now,
                    DateEventStart= DateTime.Now,
                };
            }            
            var friendlyUrl = _routeItemRepository.GetFriendlyUrl(model.Id, Common.TypeEntityFromRouteEnum.Event);
            ViewBag.CurrentFriendlyUrl = friendlyUrl;

            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(Event model)
        {
            _eventRepository.AddOrUpdate(model);
            var routeItem = new RouteItem(0, null, (string)ControllerContext.RequestContext.RouteData.Values["controller"],
              "Get",
              model.Id)
            { Type = Common.TypeEntityFromRouteEnum.Event };

            _routeItemRepository.AddOrUpdate(routeItem);
            ViewData["OperationResult"] = "Операция прошла успешно";
            return RedirectToAction("ManageIndex");
        }

        public ActionResult Delete(int id)
        {
            var efEvent = _eventRepository.FindBy(i => i.Id == id).SingleOrDefault();
            _eventRepository.Delete(id);
            return RedirectToAction("ManageIndex");
        }

        public ActionResult TemplateCategoryFromEventProduct(int count,int eventId= 0)
        {
            var efEvent = _eventRepository.Get(eventId);
            var indx = 0;
            if (efEvent!=null&& efEvent.Products!=null)
            {
                indx = efEvent.Products.Count();
            }
            if (count>indx)
            {
                indx = count;
            }
            ViewBag.CategoriesSelectListItem = _categoryRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            return PartialView("TemplateCategoryFromEventProduct", indx);
        }
        public ActionResult TemplateEventProductFromCategoryId(int id, int index)
        {
            ViewBag.ProductsSelectListItem = _categoryRepository.GetProductsByCategoryId(id).Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            return PartialView("TemplateEventProductFromCategoryId", index);
        }
        public ActionResult DeleteProduct(int id,int productid)
        {
            var result=_eventRepository.DeleteReletedProduct(id, productid);
            return Json(new {Status="ok",Result=result });
        }
    }
}