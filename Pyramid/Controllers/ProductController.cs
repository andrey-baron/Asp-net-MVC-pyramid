using Pyramid.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class ProductController : Controller
    {
        DAL.UnitOfWork unitOfWork;
        public ProductController() {
            unitOfWork = new DAL.UnitOfWork();
        }
        // GET: Product
        public ActionResult Index()
        {   
           var model= unitOfWork.Products.GetAll();
            return View(model);
        }
        public ActionResult AddOrUpdate(int id = 0)
        {
            var model = unitOfWork.Products.Get(id);

            return View(model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(Product model)
        {
            if (model.Id==0)
            {
                model.DateCreation = DateTime.Now;
            }
            model.DateChange = DateTime.Now;
            unitOfWork.Products.AddOrUpdateEntity(model);

            return RedirectToAction("index");
        }
    }
}