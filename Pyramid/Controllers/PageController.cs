using AutoMapper;
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

        public PageController()
        {
            _pageRepo = new PageRepository();
        }
        // GET: Page
        public ActionResult AdminIndex()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Pages, Pyramid.Entity.Page>());
            // var model = _pageRepo.GetAll().ToList();
            var model =
                Mapper.Map<IEnumerable<Pages>, List<Entity.Page>>(_pageRepo.GetAll().ToList());
           
            return View(model);
        }

        public ActionResult AddOrUpdate(int id=0)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<Pages, Pyramid.Entity.Page>());
            var ef = _pageRepo.FindBy(p => p.Id == id).SingleOrDefault();

            
            var model = Mapper.Map<Pages, Entity.Page>(_pageRepo.FindBy(p=>p.Id==id).SingleOrDefault());
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
            Mapper.Initialize(cfg => cfg.CreateMap<Pages, Entity.Page>());
            var efTest = Mapper.Map<Pages, Entity.Page>(_pageRepo.FindBy(p => p.Id == model.Id).SingleOrDefault());
            Mapper.Initialize(cfg => cfg.CreateMap< Entity.Page, Pages>());
            var efmodel = Mapper.Map<Entity.Page, Pages>(model);
            if (efTest == null)
            {
                _pageRepo.Add(efmodel);
            }
            else
            {
                _pageRepo.Edit(efmodel);
            }


            _pageRepo.Save();

            return RedirectToAction("AdminIndex");
            try
            {
               
            }
            catch
            {
                return RedirectToAction("AdminIndex");
            }
        }

      

        // GET: Page/Delete/5
        public ActionResult Delete(int id)
        {
            var efPage = _pageRepo.FindBy(i => i.Id == id).SingleOrDefault();
            if (efPage != null)
            {
                _pageRepo.Delete(efPage);
                _pageRepo.Save();
            }
           
            return RedirectToAction("AdminIndex");
        }
        [AllowAnonymous]
        public ActionResult Index(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                #region root config

                cfg.CreateMap<DBFirstDAL.Pages, Pyramid.Entity.Page>(); 

               
                #endregion
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var efPage = _pageRepo.FindBy(i => i.Id == id).SingleOrDefault();
            if (efPage!=null)
            {
                List<Models.BreadCrumbViewModel> breadcrumbs = new List<Models.BreadCrumbViewModel>();
                
                breadcrumbs.Add(new Models.BreadCrumbViewModel()
                {
                    Title = efPage.Title
                });
                ViewBag.BredCrumbs = breadcrumbs;
            }
            var modelPage = mapper.Map<Pyramid.Entity.Page>(efPage);
            return View(modelPage);
        }


    }
}
