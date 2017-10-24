using Common.SearchClasses;
using DBFirstDAL.Repositories;
using Pyramid.Entity;
using Pyramid.Models.CommonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
    public class ImageManagerController : Controller
    {
        ImageRepository _imageRepository;
        // GET: ImageManager
        public ImageManagerController() {
            _imageRepository = new ImageRepository();
        }
        [HttpGet]
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            
            var objectsPerPage = 20;
            var searchResult = _imageRepository.Get(new SearchParamsImage
            {
                StartIndex = (pageNumber - 1) * objectsPerPage,
                ObjectsCount = objectsPerPage,
            });
            var viewModel = SearchResultViewModel<Image>.CreateFromSearchResult(searchResult, i => i, 10);

            return View(viewModel);
            //var model = DBFirstDAL.ImageDAL.GetAll();
            //return View(model);
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileWrapper qqfile)
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
            
            return Json(new {  success = true });
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
        public ActionResult PartialSelectImage(int? page)
        {
            var pageNumber = page ?? 1;

            var objectsPerPage = 20;
            var searchResult = _imageRepository.Get(new SearchParamsImage
            {
                StartIndex = (pageNumber - 1) * objectsPerPage,
                ObjectsCount = objectsPerPage,
            });
            var viewModel = SearchResultViewModel<Image>.CreateFromSearchResult(searchResult, i => i, 10);

           // return View(viewModel);

            return PartialView("_PartialSelectImage", viewModel);
        }


        #region from dropzonejs

        public ActionResult IndexDropzone()
        {
            var model = DBFirstDAL.ImageDAL.GetAll();
            return View(model);
        }
        [HttpPost]
         public  ActionResult SaveUploadedFile()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];


                    
                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        DBFirstDAL.ImageDAL.AddOrUpdate(null, file);
                        //var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\WallImages", Server.MapPath(@"\")));

                        //string pathString = System.IO.Path.Combine(originalDirectory.ToString(), "imagepath");

                        //var fileName1 = Path.GetFileName(file.FileName);

                        //bool isExists = System.IO.Directory.Exists(pathString);

                        //if (!isExists)
                        //    System.IO.Directory.CreateDirectory(pathString);

                        //var path = string.Format("{0}\\{1}", pathString, file.FileName);
                        //file.SaveAs(path);

                    }

                }

            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }
            return null;

            if (isSavedSuccessfully)
            {
                return  Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }
        #endregion
    }
}