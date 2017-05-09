using Pyramid.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class CategoryController : Controller
    {
        DAL.UnitOfWork unitOfWork;
        public CategoryController()
        {
            unitOfWork = new DAL.UnitOfWork();
        }
        public ActionResult Index()
        {
            var model = unitOfWork.Categories.GetAll();
            return View(model);
        }
        public ActionResult AddOrUpdate(int id=0)
        {
            var model = unitOfWork.Categories.Get(id);

            return View(model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(Category model)
        {
            unitOfWork.Categories.AddOrUpdateEntity(model);

            return RedirectToAction("index");
        }
    }
}