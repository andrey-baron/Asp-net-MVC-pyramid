using AutoMapper;
using DBFirstDAL.Repositories;
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
        ProductRepository _productRepository;
        EnumValueRepository _enumRepositopy;
        CategoryRepository _categoryRepository;
       public ProductController()
        {
            _productRepository = new ProductRepository();
            _enumRepositopy = new EnumValueRepository();
            _categoryRepository = new CategoryRepository();
        }
        public ActionResult Index()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.ProductValues, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.Ignore()); ;

              
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var model = mapper.Map<IEnumerable<DBFirstDAL.Products>,List<Entity.Product>>(_productRepository.GetAll().ToList());
           
            return View(model);
        }
        [Authorize]
        public ActionResult AdminIndex()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.ProductValues, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                ; 


            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var model = mapper.Map<IEnumerable<DBFirstDAL.Products>, List<Entity.Product>>(_productRepository.GetAll().ToList());

            return View(model);
        }
        [Authorize]
        public ActionResult AddOrUpdate(int id = 0)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.ThumbnailId, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                
                //.ForMember(d => d.Categories, o => o.Ignore())
                ;

                cfg.CreateMap<DBFirstDAL.Categories, Entity.Category>()
                .ForMember(d => d.Checked, o => o.UseValue(false))
                .ForMember(d => d.Filters, o => o.Ignore())
                .ForMember(d => d.Products, o => o.Ignore())
                .ForMember(d => d.Thumbnail, o => o.Ignore())


                //.ForMember(d => d, o => o.MapFrom(m=>m.Categories1
                ;
                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Models.CategoryAdminViewModel>()
                .ForMember(d => d.Checked, o => o.UseValue(false))
                ;

                cfg.CreateMap<DBFirstDAL.ProductValues, Entity.ProductValue>()
                .ForMember(d => d.Product, o => o.Ignore())
                ;
                cfg.CreateMap<DBFirstDAL.EnumValues, EnumValue>()
                ;
                cfg.CreateMap<DBFirstDAL.Images, Image>()
                ;

            });
            config.AssertConfigurationIsValid();


            var mapper = config.CreateMapper();
            var efModel = _productRepository.FindBy(i => i.Id == id).SingleOrDefault();
            var efProductImagesThubnail = efModel.ProductImages
                .FirstOrDefault(i => i.TypeImage == (int)Entity.Enumerable.TypeImage.Thumbnail);

            var efThumbnail = _productRepository.GetThumbnail(id, (int)Entity.Enumerable.TypeImage.Thumbnail);
            var efGalery = _productRepository.GetThumbnail(id, (int)Entity.Enumerable.TypeImage.GaleryItem);

            var model = mapper.Map<DBFirstDAL.Products, Entity.Product>(efModel);
            model.ThumbnailImg = mapper.Map<Image>(efThumbnail);

            model.Images= mapper.Map<List<Image>>(efGalery);

            var categoriesViewModel =
                mapper.Map<IEnumerable<DBFirstDAL.Categories>, List<Models.CategoryAdminViewModel>>(_categoryRepository.GetAll().ToList());


            if (model!=null)
            {
                foreach (var item in model.Categories)
                {
                    categoriesViewModel.FirstOrDefault(i => i.Id == item.Id).Checked = true;
                }
            }
            else
            {
                model = new Product();
            }

            ViewBag.EnumValuesSelectList = _enumRepositopy.GetAll().ToList().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            });

            ViewBag.AllCategories = categoriesViewModel;

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

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Pyramid.Entity.Product, DBFirstDAL.Products>()
                .ForMember(d => d.PointOnImg_Id, o => o.Ignore())
                .ForMember(d => d.ProductImages, o => o.Ignore());

                cfg.CreateMap<Pyramid.Entity.Category, DBFirstDAL.Categories>()
                .ForMember(d => d.Categories1, o => o.Ignore())
                .ForMember(d => d.Categories2, o => o.Ignore())
                .ForMember(d => d.Filters, o => o.Ignore())
                .ForMember(d => d.Products, o => o.Ignore())
                .ForMember(d => d.CategoryImages, o => o.Ignore())
                ;

                cfg.CreateMap<EnumValue, DBFirstDAL.EnumValues>()
                .ForMember(d => d.Filters, o => o.Ignore())
                .ForMember(d => d.Products, o => o.Ignore())
                ;
                cfg.CreateMap<ProductValue, DBFirstDAL.ProductValues>()
                .ForMember(d => d.Products, o => o.Ignore())
                ;


            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            model.Categories= model.Categories.Where(i => i.Checked == true).ToList();

            var efModel = mapper.Map<DBFirstDAL.Products>(model);
            if (model.ThumbnailId!=0)
            {
                efModel.ProductImages = new List<DBFirstDAL.ProductImages>();
                efModel.ProductImages
                    .Add(new DBFirstDAL.ProductImages()
                {
                    ImageId = model.ThumbnailId,
                    ProductId = model.Id,
                    TypeImage = (int)Entity.Enumerable.TypeImage.Thumbnail
                } );
            }
            
            _productRepository.AddOrUpdate(efModel);
            _productRepository.Save();

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

        public ActionResult GetAllEnumValues(int id)
        {

            var model = _productRepository.GetAllEnumValues(id);
            ViewBag.EnumValuesSelectList = _enumRepositopy.GetAll().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            });
            return PartialView("_PartialProductAllEnumValues", model);
        }
        public ActionResult GetTemplateEnumValue(int id)
        {
            ViewBag.EnumValuesSelectList = _enumRepositopy.GetAll().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            });
            var model = _productRepository.GetAllEnumValues(id).Count();
            return PartialView("_PartialProductEmptyEnumValue", model);
        }

        public ActionResult DeleteEnumValue(int id, int enumValueId)
        {
            _productRepository.DeleteEnumValue(id, enumValueId);
            _productRepository.Save();
            return null;
        }

    }
}