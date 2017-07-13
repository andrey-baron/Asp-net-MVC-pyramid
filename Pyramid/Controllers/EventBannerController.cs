using DBFirstDAL.Repositories;
using Pyramid.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
    public class EventBannerController : Controller
    {
        EventBannerRepository _eventBannerRepository;

        public EventBannerController() {
            _eventBannerRepository = new EventBannerRepository();
        }
        // GET: EventBanner
        public ActionResult Index()
        {
            var model = _eventBannerRepository.GetAll();
            return View(model);
        }
        public ActionResult AddOrUpdate(int id = 0)
        {
            var model = _eventBannerRepository.Get(id);
            if (model==null)
            {
                model = new EventBanner();
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate( EventBanner model)
        {
            _eventBannerRepository.AddOrUpdate(model);
            return RedirectToAction("Index");

        }
    }
}