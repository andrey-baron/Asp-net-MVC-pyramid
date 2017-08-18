using DBFirstDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]

    public class BannersOnHomePageController : Controller
    {

        BannersOnHomePageRepository _bannersOnHomePageRepository;

        public BannersOnHomePageController()
        {
            _bannersOnHomePageRepository = new BannersOnHomePageRepository();
        }
        // GET: EventBanner
        public ActionResult Index()
        {
            var model = _bannersOnHomePageRepository.GetAll();
            return View(model);
        }
        public ActionResult AddOrUpdate(int id = 0)
        {
            var model = _bannersOnHomePageRepository.Get(id);
            if (model == null)
            {
                model = new Entity.BannersOnHomePage();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(Entity.BannersOnHomePage model)
        {
            _bannersOnHomePageRepository.AddOrUpdate(model);
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            _bannersOnHomePageRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}