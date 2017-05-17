using Pyramid.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class ProductController : Controller
    {
        //DAL.UnitOfWork unitOfWork;
        //public ProductController() {
        //    unitOfWork = new DAL.UnitOfWork();
        //}
        // GET: Product
        public ActionResult Index()
        {
            var model = DBFirstDAL.ProductDAL.GetAll();
            return View(model);
        }
        [Authorize]
        public ActionResult AdminIndex()
        {
            var model = DBFirstDAL.ProductDAL.GetAll();
            return View(model);
        }
        [Authorize]
        public ActionResult AddOrUpdate(int id = 0)
        {
            var model = DBFirstDAL.ProductDAL.Get(id);

            var categories = DBFirstDAL.CategoryDAL.GetAll();
            foreach (var item in model.Categories)
            {
                categories.FirstOrDefault(i => i.Id == item.Id).Cheaked = true;
            }
            ViewBag.AllCategories = categories;

            return View(model);
        }
        [Authorize]
        [HttpPost]  
        public ActionResult AddOrUpdate(Product model)
        {
            if (model.Id==0)
            {
                model.DateCreation = DateTime.Now;
            }
            model.DateChange = DateTime.Now;
            DBFirstDAL.ProductDAL.AddOrUpdateEntity(model);

            return RedirectToAction("AdminIndex");
        }
        [Authorize]
        public ActionResult GetAllProductValues(int productId)
        {
            var model = DBFirstDAL.ProductValueDAL.GetAll(productId);
            return PartialView("_PartialAllProductValues", model);
        }
        [Authorize]
        public ActionResult AddOrUpdateProductValue(int productId,Entity.ProductValue model)
        {
            DBFirstDAL.ProductValueDAL.AddOrUpdate(productId, model);
            return null;
        }
        [Authorize]
        public ActionResult GetEmptyTemplateProductValue(int id)
        {
            var maxIndx = DBFirstDAL.ProductValueDAL.GetCountByProductId(id);
           //DBFirstDAL.ProductValueDAL.AddOrUpdate(id, new ProductValue());
            //maxIndx++;
            return PartialView("_PartialEmptyTemplateProductValue", maxIndx);
        }
        [Authorize]
        public ActionResult DeleteProductValue(int id)
        {
            DBFirstDAL.ProductValueDAL.Delete(id);
            return null;
        }
    }
}