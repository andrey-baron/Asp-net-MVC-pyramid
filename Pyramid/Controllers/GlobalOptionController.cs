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
    public class GlobalOptionController : Controller
    {
        GlobalOptionRepository _globalOptionRepository;
        // GET: GlobalOption

        public GlobalOptionController()
        {
            _globalOptionRepository = new GlobalOptionRepository();
        }

        public ActionResult Index()
        {
            var model = _globalOptionRepository.GetAll();
            return View(model);
        }
        public ActionResult Update(int id)
        {
            var model = _globalOptionRepository.Get(id);
            return View(model);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Update(GlobalOptionEntity model)
        {
            _globalOptionRepository.AddOrUpdate(model);
            return RedirectToAction("Index");
        }
    }
}