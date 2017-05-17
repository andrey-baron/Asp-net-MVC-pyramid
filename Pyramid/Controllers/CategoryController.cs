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
        //DAL.UnitOfWork unitOfWork;
        //public CategoryController()
        //{
        //    unitOfWork = new DAL.UnitOfWork();
        //}
        [Authorize]
        public ActionResult AdminIndex()
        {
            var model = DBFirstDAL.CategoryDAL.GetAll();
            return View(model);
        }
        public ActionResult Index()
        {
            var model = DBFirstDAL.CategoryDAL.GetAll();
            return View(model);
        }
        [Authorize]
        public ActionResult AddOrUpdate(int id=0)
        {
            var model = DBFirstDAL.CategoryDAL.Get(id);

            ViewBag.CategoriesSelectListItem = DBFirstDAL.CategoryDAL.GetAllWithoutCategoryId(id).Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            }); ;
            

            
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddOrUpdate(Pyramid.Entity.Category model)
        {
            DBFirstDAL.CategoryDAL.AddOrUpdateEntity(model);

            return RedirectToAction("AdminIndex");
        }
    }
}