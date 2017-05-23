using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
    public class ImageManagerController : Controller
    {
        // GET: ImageManager
        public ActionResult Index()
        {
            var model = DBFirstDAL.ImageDAL.GetAll();
            return View(model);
        }
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> qqfile)
        {
            var files = this.Request.Files;
            DBFirstDAL.ImageDAL.AddOrUpdate(null, qqfile);
            return Json(new { result = "ok", success = true });
        }

        public ActionResult PartialBodyModal(int id)
        {
            return PartialView("_PartialBodyModal", DBFirstDAL.ImageDAL.Get(id));

        }
        public ActionResult GetImages()
        {
            var model = DBFirstDAL.ImageDAL.GetAll();
            return PartialView("_PartialAllImages", model);
        }
        public ActionResult AddOrUpdate(Pyramid.Entity.Image model)
        {
            DBFirstDAL.ImageDAL.AddOrUpdate(model);
            return Json(new { result = "ok", success = true });
        }
        public ActionResult Delete(int id)
        {
            DBFirstDAL.ImageDAL.Delete(id);
            return null;
        }
        public ActionResult PartialSelectImage()
        {
            var model = DBFirstDAL.ImageDAL.GetAll();
            return PartialView("_PartialSelectImage", model);
        }
    }
}