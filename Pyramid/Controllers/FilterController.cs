using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class FilterController : Controller
    {
        // GET: Filter
        [Authorize]
        public ActionResult Index()
        {
            var model = DBFirstDAL.FilterDAL.GetAll();
            return View(model);
        }
        [Authorize]
        public ActionResult AddOrUpdate(int id=0)
        {
            var model = DBFirstDAL.FilterDAL.Get(id);
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddOrUpdate(Pyramid.Entity.Filter model)
        {
            DBFirstDAL.FilterDAL.AddOrDefault(model);
            return RedirectToAction("index");
        }
    }
}