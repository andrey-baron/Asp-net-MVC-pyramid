using AutoMapper;
using DBFirstDAL.Repositories;
using PagedList;
using Pyramid.Entity;
using Pyramid.Global;
using Pyramid.Models.Single;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class ProductController : BaseController
    {
        ProductRepository _productRepository;
        EnumValueRepository _enumRepositopy;
        CategoryRepository _categoryRepository;
        const string defaulProductLink = "/Product/index/";
        const string defaulCateggorytLink = "/Category/index/";
        public ProductController()
        {
            _productRepository = new ProductRepository();
            _enumRepositopy = new EnumValueRepository();
            _categoryRepository = new CategoryRepository();
        }
        public ActionResult Index(int id)
        {
            #region config mapper product
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.
                MapFrom(m =>
                m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()))
                .ForMember(d => d.Images, o => o.
                                MapFrom(m =>
                m.ProductImages.Where(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.GalleryItem).Count() > 0 ?
                m.ProductImages.Where(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.GalleryItem).Select(s => s.Images).ToList() : new List<DBFirstDAL.Images>()))

                .ForMember(d => d.OneCId, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.Categories, Entity.Category>()
               .ForMember(d => d.Checked, o => o.UseValue(false))
               .ForMember(d => d.Filters, o => o.Ignore())
               //.ForMember(d => d.Products, o => o.Ignore())
               .ForMember(d => d.Thumbnail, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.ProductValues, Entity.ProductValue>()
                .ForMember(d => d.Product, o => o.Ignore())
                ;

                cfg.CreateMap<DBFirstDAL.Images, Image>()
                ;

            });
            #endregion

            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            //var efThumbnail = _productRepository.GetThumbnail(id, (int)Entity.Enumerable.TypeImage.Thumbnail);
            //var efGallery =_productRepository.GetGalleryImage(id, (int)Entity.Enumerable.TypeImage.GaleryItem);


            var efModel = _productRepository.FindBy(i => i.Id == id).SingleOrDefault();
            var EntityProduct = mapper.Map<DBFirstDAL.Products, Entity.Product>(_productRepository.FindBy(i => i.Id == id).SingleOrDefault());

            List<Models.BreadCrumbViewModel> breadcrumbs = new List<Models.BreadCrumbViewModel>();
            breadcrumbs.Add(new Models.BreadCrumbViewModel() {
                Title = EntityProduct.Title
            , Link = defaulProductLink + id.ToString()
            });
            var flagstop = true;
            var cat = efModel.Categories.FirstOrDefault();
            var relatedProducts = cat != null ? cat.Products.Where(s=>s.Id!=id) : new List<DBFirstDAL.Products>();
            while (flagstop)
            {

                if (cat != null)
                {
                    breadcrumbs.Add(new Models.BreadCrumbViewModel()
                    {
                        Title = cat.Title,
                        Link = defaulCateggorytLink + cat.Id.ToString()
                    });
                    if (cat.ParentId == null)
                    {

                        flagstop = false;
                    }
                    else
                    {
                        cat = cat.Categories2;
                    }
                }
                else
                {
                    break;
                }
            }
            breadcrumbs.Reverse();
            ViewBag.BredCrumbs = breadcrumbs;

            //var t = mapper.Map <IEnumerable<DBFirstDAL.Images>, List<Image>>(efGallery.ToList());

            //model.ThumbnailImg = mapper.Map<Image>(efThumbnail);
            var model = new SingleViewModel()
            {
                Product = EntityProduct,
                RelatedProducts = mapper.Map<IEnumerable<DBFirstDAL.Products>, List<Product>>(relatedProducts)
        };
            return View(model);
        }
        [Authorize]
        public ActionResult AdminIndex(string currentFilter, string searchString,int? categoryId, int? page)
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
                //cfg.CreateMap<IPagedList<DBFirstDAL.Products>, IPagedList<Entity.Product>>()
                //;

            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            ViewBag.CategoryId = categoryId;
            ViewBag.CategoriesSelectListItem = _categoryRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });

            IQueryable<DBFirstDAL.Products> efProducts;
            if (categoryId!=null)
            {
                efProducts=_categoryRepository.GetProductsByCategoryId(categoryId.Value).AsQueryable();
            }
            else
            {
                efProducts = _productRepository.GetAll();
            }
             


            if (!String.IsNullOrEmpty(searchString))
            {
                efProducts = efProducts.Where(s => s.Title.Contains(searchString));
            }
            var efProductsList = efProducts.ToList();
            int pageNumber = (page ?? 1);
            var modelList = new PagedList<Entity.Product>(
            efProductsList.Select(u => mapper.Map<DBFirstDAL.Products, Entity.Product>(u)),
            pageNumber, Config.PageSize);
            //var pagedProducts= efProducts.OrderBy(i=>i.Title).ToPagedList(pageNumber, pageSize);
            //var model = mapper.Map<IEnumerable<DBFirstDAL.Products>, IPagedList<Entity.Product>>(pagedProducts);
            return View(modelList);
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
            var model = mapper.Map<DBFirstDAL.Products, Entity.Product>(efModel);
            if (efModel!=null)
            {
                var efProductImagesThubnail = efModel.ProductImages
                .FirstOrDefault(i => i.TypeImage == (int)Entity.Enumerable.TypeImage.Thumbnail);

                var efThumbnail = _productRepository.GetThumbnail(id, (int)Entity.Enumerable.TypeImage.Thumbnail);
                var efGalery = _productRepository.GetGalleryImage(id, (int)Entity.Enumerable.TypeImage.GaleryItem);
                model.ThumbnailImg = mapper.Map<Image>(efThumbnail);

                model.Images = mapper.Map<List<Image>>(efGalery);
            }

            
           

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
                .ForMember(d => d.ProductImages, o => o.Ignore())
                 .ForMember(d => d.Review, o => o.Ignore())
                 .ForMember(d => d.PointOnImgs, o => o.Ignore())
                 .ForMember(d => d.HomeEntity, o => o.Ignore())
                 .ForMember(d => d.ProductOrders, o => o.Ignore());

                cfg.CreateMap<Pyramid.Entity.Category, DBFirstDAL.Categories>()
                .ForMember(d => d.Categories1, o => o.Ignore())
                .ForMember(d => d.Categories2, o => o.Ignore())
                .ForMember(d => d.Filters, o => o.Ignore())
                .ForMember(d => d.Products, o => o.Ignore())
                .ForMember(d => d.CategoryImages, o => o.Ignore())
                .ForMember(d => d.Recommendations, o => o.Ignore())
                .ForMember(d => d.HomeEntity, o => o.Ignore())
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
        public ActionResult GetEmptyTemplateProductValue(int id,int count)
        {
            var maxIndx = DBFirstDAL.ProductValueDAL.GetCountByProductId(id);
            if (count>maxIndx)
            {
                maxIndx = count;
            }
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
            var config = new MapperConfiguration(cfg =>
            {
         
                cfg.CreateMap<DBFirstDAL.EnumValues,EnumValue >()
               
                ;
               

            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var efmodel = _productRepository.GetAllEnumValues(id);
            var model = mapper.Map<IEnumerable<DBFirstDAL.EnumValues>, List<EnumValue>>(efmodel);
            ViewBag.EnumValuesSelectList = _enumRepositopy.GetAll().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            });
            return PartialView("_PartialProductAllEnumValues", model);
        }
        public ActionResult GetTemplateEnumValue(int id,int count)
        {
            ViewBag.EnumValuesSelectList = _enumRepositopy.GetAll().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            });
            var model = _productRepository.GetAllEnumValues(id).Count();
            if (count > model)
            {
                model = count;
            }
            return PartialView("_PartialProductEmptyEnumValue", model);
        }

        public ActionResult DeleteEnumValue(int id, int enumValueId)
        {
            _productRepository.DeleteEnumValue(id, enumValueId);
            _productRepository.Save();
            return null;
        }

        public PartialViewResult GetAllImageFromCategory(int id,int imageId)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Images, Image>() ;
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var efGalery = _productRepository.GetGalleryImage(id, (int)Entity.Enumerable.TypeImage.GaleryItem);
            var model = mapper.Map<IEnumerable<Image>>(efGalery);
            return PartialView("_PartialProductGallery", model);
        }

        public ActionResult AddToGallery(int id,int imageid)
        {
            _productRepository.AddToGallry(id, imageid, (int)Entity.Enumerable.TypeImage.GaleryItem);
            _productRepository.Save();
            return GetAllImageFromCategory(id, imageid);
        }
        public ActionResult DeleteToGallery(int id,int imageid)
        {
            _productRepository.RemoveToGallry(id, imageid, (int)Entity.Enumerable.TypeImage.GaleryItem);
            _productRepository.Save();
            return GetAllImageFromCategory(id, imageid);
        }
        [Authorize]
        public ActionResult GetProductReview(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Review, Entity.Review>();
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var efProduct = _productRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (efProduct!=null)
            {
                ViewBag.TitlePage = efProduct.Title;
            }
            var efModel = _productRepository.GetReview(id);
            var model = mapper.Map<Review>(efModel);
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var product=_productRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (product!=null)
            {
                _productRepository.Delete(product);
                _productRepository.Save();
            }
            return RedirectToAction("AdminIndex");
        }

       
    }
}