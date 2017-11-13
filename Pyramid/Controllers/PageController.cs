using AutoMapper;
using Common.Models;
using DBFirstDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Entity;


namespace Pyramid.Controllers
{
    [Authorize]
    public class PageController : BaseController
    {
        PageRepository _pageRepo;
        EventBannerRepository _eventBannerRepository;
        RouteItemRepository _routeItemRepository;
        public PageController()
        {
            _pageRepo = new PageRepository();
            _eventBannerRepository = new EventBannerRepository();
            _routeItemRepository = new RouteItemRepository();

        }

        public ActionResult AdminIndex()
        {
           // Mapper.Initialize(cfg => cfg.CreateMap<Pages, Pyramid.Entity.Page>());
            // var model = _pageRepo.GetAll().ToList();
            var model = _pageRepo.GetAll();
           
            return View(model);
        }

        public ActionResult AddOrUpdate(int id=0)
        {
            //Mapper.Initialize(cfg => cfg.CreateMap<Pages, Pyramid.Entity.Page>());
            //var ef = _pageRepo.FindBy(p => p.Id == id).SingleOrDefault();

            
            var model = _pageRepo.Get(id);
            if (model==null)
            {
                model = new Entity.Page();
            }
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(Entity.Page model)
        {
            

            _pageRepo.AddOrUpdate(model);
            var routeItem = new RouteItem(0, null, (string)ControllerContext.RequestContext.RouteData.Values["controller"],
               "Index",
               (int)ControllerContext.RequestContext.RouteData.Values["id"])
            { Type = Common.TypeEntityFromRouteEnum.PageType };
            _routeItemRepository.AddOrUpdate(routeItem);
            return RedirectToAction("AdminIndex");
           
        }

        public ActionResult Delete(int id)
        {
            var efPage = _pageRepo.FindBy(i => i.Id == id).SingleOrDefault();
            if (efPage != null)
            {
                _pageRepo.Delete(efPage.Id);
            }
           
            return RedirectToAction("AdminIndex");
        }
        [AllowAnonymous]
        public ActionResult Index(int id)
        {
            var page = _pageRepo.Get(id);
            if (page != null)
            {
                List<BreadCrumbViewModel> breadcrumbs = new List<BreadCrumbViewModel>();
                
                breadcrumbs.Add(new BreadCrumbViewModel()
                {
                    Title = page.Title
                });
                ViewBag.BredCrumbs = breadcrumbs;
            }
            ViewBag.Banners = _eventBannerRepository.GetAll();
            ViewBag.MetaTitle = page.Title;
            return View(page);
        }


    }
}
