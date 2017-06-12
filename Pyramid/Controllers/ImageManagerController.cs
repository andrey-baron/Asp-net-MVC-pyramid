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

        public ActionResult Upload(/*HttpPostedFileWrapper qqfile*/)
        {
            var files = this.Request.Files;
            HttpPostedFileBase test = null;
            foreach (string item in files)
            {
                test = files[item];
            }
            if (test!=null)
            {
                DBFirstDAL.ImageDAL.AddOrUpdate(null, test);
            }
            
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
            var model = DBFirstDAL.ImageDAL.GetAll().Select(i=>new Pyramid.Entity.Image() {
                Id=i.Id,
                ImgAlt=i.ImgAlt,
                PathInFileSystem=i.PathInFileSystem,
                ServerPathImg=i.ServerPathImg,
                Title=i.Title
            }).ToList();
            return PartialView("_PartialSelectImage", model);
        }
    }
}