using AutoMapper;
using Common.Models;
using DBFirstDAL;
using DBFirstDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
    public class PageController : BaseController
    {
        
        PageRepository _pageRepo;
        EventBannerRepository _eventBannerRepository;

        public PageController()
        {
            _pageRepo = new PageRepository();
            _eventBannerRepository = new EventBannerRepository();
        }
        // GET: Page
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

        // POST: Page/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(Entity.Page model)
        {
            //Mapper.Initialize(cfg => cfg.CreateMap<Pages, Entity.Page>());
            //var efTest = Mapper.Map<Pages, Entity.Page>(_pageRepo.FindBy(p => p.Id == model.Id).SingleOrDefault());
            //Mapper.Initialize(cfg => cfg.CreateMap< Entity.Page, Pages>());
            //var efmodel = Mapper.Map<Entity.Page, Pages>(model);

            _pageRepo.AddOrUpdate(model);

            return RedirectToAction("AdminIndex");
           
        }

      

        // GET: Page/Delete/5
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
            //var config = new MapperConfiguration(cfg =>
            //{
            //    #region root config

            //    cfg.CreateMap<DBFirstDAL.Pages, Pyramid.Entity.Page>(); 

               
            //    #endregion
            //});
            //config.AssertConfigurationIsValid();
            //var mapper = config.CreateMapper();

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
            //var modelPage = mapper.Map<Pyramid.Entity.Page>(page);
            ViewBag.Banners = _eventBannerRepository.GetAll();
            ViewBag.MetaTitle = page.Title;
            return View(page);
        }


    }
}
