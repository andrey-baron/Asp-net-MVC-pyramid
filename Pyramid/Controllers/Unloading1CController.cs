using DBFirstDAL.DataModels._1C;
using DBFirstDAL.Repositories;
using Pyramid.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
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
            flagErr = Execute();


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
                var efCats = xmlModel.Categories.Select(s => new DBFirstDAL.Categories()
                {
                    Title = s.Title,
                    OneCId = s.Id,

                });
                foreach (var item in efCats)
                {
                    _categoryRepository.AddOrUpdateFromOneC(item);
                }
                

                var efCatWithParent = xmlModel.Categories.Select(s => new Category1CIdWithParent1CId() { Id = s.Id, ParentId = s.ParentId }).ToList();

           
                _categoryRepository.UpdateParentCategory(efCatWithParent);

            }
            catch (Exception)
            {

                flagErr = true; ;
            }
            var efProducts = xmlModel.Products.Select(s => new DBFirstDAL.Products()
            {
                Title = s.Title,
                Price = s.Price,
                DateChange = DateTime.Now,
                DateCreation = DateTime.Now,
                OneCId = s.Id,
                TypePrice = (int)s.TypePrice,
                IsPriority=s.Priority,
                IsFilled=false,
                EnumValues=new List<DBFirstDAL.EnumValues>(new DBFirstDAL.EnumValues[]{ new DBFirstDAL.EnumValues() {
                    Key =s.Brand,
                    TypeValue =(int)Common.TypeFromEnumValue.Brand} }) ,
                
                Categories = s.CategoryTextIds.Select(i=>new DBFirstDAL.Categories() { OneCId=i}).ToList()
            });
            //var efProductsWithCategories = xmlModel.Products.Select(s => new Product1cIdWith1cCategoryIds()
            //{
            //    OneCId = s.Id,
            //    CategoryIds = s.CategoryTextIds
            //});
            foreach (var item in efProducts)
            {
                _productRepository.AddOrUpdateFromOneC(item);
            }

            _categoryRepository.AddOrUpdateFilterBrand();
            //_productRepository.InsertsOrNot(efProducts);
            //_productRepository.UpdateReletedCategoriesFromProducts(efProductsWithCategories);
            return flagErr;
        }

    }
}