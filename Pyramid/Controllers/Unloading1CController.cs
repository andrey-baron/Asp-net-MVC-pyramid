using DBFirstDAL.Repositories;
using Pyramid.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class Unloading1CController : Controller
    {
        ProductRepository _productRepository;
        CategoryRepository _categoryRepository;
        // GET: Unloading1C

        public Unloading1CController()
        {
            _categoryRepository = new CategoryRepository();
            _productRepository = new ProductRepository();
        }
        public ActionResult Index()
        {

            return View();
        }
        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase uploadxml)
        {
            var files = this.Request.Files;
            var errFlag=UploadXmlToFileSystem.Upload(uploadxml);
            ViewBag.ResultUpload = !errFlag?"Файл успешно загружен":"Ошибка загрузки файла";
            
            return View();
        }

        public ActionResult Start()
        {
            var flagErr = false;
            //flagErr = Execute();


            ViewData["resultMapping"] = "success";
            ViewBag.ResultMapping = flagErr ? "Ошибка загрузки данных" : "Загрузка успешно завершилась";
            return View();
        }

        private bool Execute()
        {

            bool flagErr = false;
            var xmlModel = Load1CDataFromXml.GetXmlModel(out flagErr);
            try
            {
                foreach (var xCategory in xmlModel.Categories)
                {

                    var efCategory = _categoryRepository.FindBy(i => i.OneCId == xCategory.Id).SingleOrDefault();
                    if (efCategory == null)
                    {
                        efCategory = new DBFirstDAL.Categories();
                        efCategory.Title = xCategory.Title;
                        efCategory.OneCId = xCategory.Id;
                        if (xCategory.ParentId != null)
                        {
                            var efCarParent = _categoryRepository.FindBy(i => i.OneCId == xCategory.ParentId).SingleOrDefault();
                            if (efCarParent != null)
                            {
                                efCategory.ParentId = efCarParent.Id;
                            }
                        }
                        _categoryRepository.AddOrUpdate(efCategory);
                        _categoryRepository.Save();
                    }
                    else
                    {
                        efCategory.Title = xCategory.Title;
                        if (xCategory.ParentId != null)
                        {
                            var efCarParent = _categoryRepository.FindBy(i => i.OneCId == xCategory.ParentId).SingleOrDefault();
                            if (efCarParent != null)
                            {
                                efCategory.ParentId = efCarParent.Id;
                            }
                        }
                        _categoryRepository.Save();
                    }

                }

                foreach (var xProduct in xmlModel.Products)
                {
                    var efProduct = _productRepository.FindBy(i => i.OneCId == xProduct.Id).SingleOrDefault();
                    if (efProduct == null)
                    {
                        efProduct = new DBFirstDAL.Products();
                        efProduct.OneCId = xProduct.Id;
                        double price = 0;
                        double.TryParse(xProduct.Price, out price);
                        efProduct.Title = xProduct.Title;
                        efProduct.Price = price;
                        efProduct.TypePrice = (int)Entity.Enumerable.TypeProductPrice.SimplePrice;
                        efProduct.DateChange = DateTime.Now;
                        efProduct.DateCreation = DateTime.Now;
                        foreach (var catProduct in xProduct.CategoryTextIds)
                        {
                            var efCategory = _categoryRepository.FindBy(i => i.OneCId == catProduct).SingleOrDefault();
                            if (efCategory != null)
                            {
                                efProduct.Categories.Add(efCategory);
                            }
                        }
                    }
                    _productRepository.AddOrUpdate(efProduct);
                   
                }
                _productRepository.Save();
            }
            catch (Exception)
            {

                flagErr = true; ;
            }
            return flagErr;
        }

    }
}