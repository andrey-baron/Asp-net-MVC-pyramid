using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            DAL.ProductDAL.AddOrUpdateEntity(new DAL.Entity.Product
            {
                DateCreation = DateTime.Now,
                Price = 111,
                Title = "2продукт2",
                TypePrice = DAL.Entity.Enumerable.TypeProductPrice.SimplePrice,
                DateChange = DateTime.Now
            });
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}