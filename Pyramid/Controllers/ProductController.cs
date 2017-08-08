using AutoMapper;
using Common.SearchClasses;
using DBFirstDAL.Repositories;
using PagedList;
using Pyramid.Entity;
using Pyramid.Global;
using Pyramid.Models.CommonViewModels;
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
        RecommendationRepository _recommendationRepository;
        FilterRepository _filterRepository;
        GlobalOptionRepository _globalOptionRepository;
        const string defaulProductLink = "/Product/index/";
        const string defaulCateggorytLink = "/Category/index/";
        public ProductController()
        {
            _productRepository = new ProductRepository();
            _enumRepositopy = new EnumValueRepository();
            _categoryRepository = new CategoryRepository();
            _recommendationRepository = new RecommendationRepository();
            _globalOptionRepository = new GlobalOptionRepository();
            _filterRepository = new FilterRepository();
        }
        public ActionResult Index(int id)
        {
         
            ViewBag.BredCrumbs = _productRepository.GetBreadCrumbs(id);

            var viewModel = new SingleViewModel()
            {
                Product = _productRepository.Get(id),
                RelatedProducts = _productRepository.RelatedProducts(id)
            };
            _productRepository.EnhancementPopularField(id);
            
            //var existColer=viewModel.Product.Categories.e

            var category=viewModel.Product.Categories.FirstOrDefault();
            if (category!=null)
            {
                var cat = _categoryRepository.Get(category.Id);
                ViewBag.Recommendations = cat.Recommendations;
                ViewBag.IsFromParent = _categoryRepository.IsChildFromParent(category.Id, 1344);
            }
            else
            {
                ViewBag.IsFromParent = false;
            }
           
            ViewBag.MetaTitle = viewModel.Product.MetaTitle?? viewModel.Product.Title;
            ViewBag.Shipping = _globalOptionRepository.Get("shipping").OptionContent;
           
            return View(viewModel);

            #region old
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
        //        .ForMember(d => d.EnumValues, o => o.Ignore())
        //        .ForMember(d => d.Categories, o => o.Ignore())
        //        .ForMember(d => d.ThumbnailId, o => o.Ignore())
        //        .ForMember(d => d.ThumbnailImg, o => o.
        //        MapFrom(m =>
        //        m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
        //        m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()))
        //        .ForMember(d => d.Images, o => o.
        //                        MapFrom(m =>
        //        m.ProductImages.Where(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.GalleryItem).Count() > 0 ?
        //        m.ProductImages.Where(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.GalleryItem).Select(s => s.Images).ToList() : new List<DBFirstDAL.Images>()))

        //        .ForMember(d => d.OneCId, o => o.Ignore());

        //        cfg.CreateMap<DBFirstDAL.Categories, Entity.Category>()
        //       .ForMember(d => d.Checked, o => o.UseValue(false))
        //       .ForMember(d => d.Filters, o => o.Ignore())
        //       //.ForMember(d => d.Products, o => o.Ignore())
        //       .ForMember(d => d.Thumbnail, o => o.Ignore())
        //       .ForAllMembers(o => o.Ignore());

        //        cfg.CreateMap<DBFirstDAL.ProductValues, Entity.ProductValue>()
        //        .ForMember(d => d.Product, o => o.Ignore())
        //        ;

        //        cfg.CreateMap<DBFirstDAL.Images, Image>()
        //        ;

        //    });
           

        //    config.AssertConfigurationIsValid();

        //    var mapper = config.CreateMapper();
        //    //var efThumbnail = _productRepository.GetThumbnail(id, (int)Entity.Enumerable.TypeImage.Thumbnail);
        //    //var efGallery =_productRepository.GetGalleryImage(id, (int)Entity.Enumerable.TypeImage.GaleryItem);


        //    var efModel = _productRepository.FindBy(i => i.Id == id).SingleOrDefault();
        //    var EntityProduct = mapper.Map<DBFirstDAL.Products, Entity.Product>(_productRepository.FindBy(i => i.Id == id).SingleOrDefault());

        //    List<Models.BreadCrumbViewModel> breadcrumbs = new List<Models.BreadCrumbViewModel>();
        //    breadcrumbs.Add(new Models.BreadCrumbViewModel() {
        //        Title = EntityProduct.Title
        //    , Link = defaulProductLink + id.ToString()
        //    });
        //    var flagstop = true;
        //    var cat = efModel.Categories.FirstOrDefault();
        //    var relatedProducts = cat != null ? cat.Products.Where(s=>s.Id!=id) : new List<DBFirstDAL.Products>();
        //    while (flagstop)
        //    {

        //        if (cat != null)
        //        {
        //            breadcrumbs.Add(new Models.BreadCrumbViewModel()
        //            {
        //                Title = cat.Title,
        //                Link = defaulCateggorytLink + cat.Id.ToString()
        //            });
        //            if (cat.ParentId == null)
        //            {

        //                flagstop = false;
        //            }
        //            else
        //            {
        //                cat = cat.Categories2;
        //            }
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    breadcrumbs.Reverse();
        //    ViewBag.BredCrumbs = breadcrumbs;

        //    //var t = mapper.Map <IEnumerable<DBFirstDAL.Images>, List<Image>>(efGallery.ToList());

        //    //model.ThumbnailImg = mapper.Map<Image>(efThumbnail);
        //    var model = new SingleViewModel()
        //    {
        //        Product = EntityProduct,
        //        RelatedProducts = mapper.Map<IEnumerable<DBFirstDAL.Products>, List<Product>>(relatedProducts)
        //};
        //    _productRepository.EnhancementPopularField(id);
        //    return View(model);
            #endregion
        }
        [Authorize]
        public ActionResult AdminIndex(string currentFilter, string searchString, int? categoryId, int? page, bool priority = false)
        {
            var pageNumber = page ?? 1;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var objectsPerPage = 20;
            var startIndex = (pageNumber - 1) * objectsPerPage;

            SearchParamsProduct SearchParams = new SearchParamsProduct(searchString, categoryId, priority,  startIndex, objectsPerPage);

            var searchResult = _productRepository.Get(SearchParams);

            ViewBag.CurrentFilter = searchString;
            ViewBag.CategoryId = categoryId;
            ViewBag.Priority = priority;
            ViewBag.CategoriesSelectListItem = _categoryRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });

            var viewModel = SearchResultViewModel<Product>.CreateFromSearchResult(searchResult, i => i, 10);
            return View(viewModel);
        }
        [Authorize]
        public ActionResult GetProductTemplateDropDownListForFilterId(int id = 0,int indx=0)
        {
            var filter = _filterRepository.Get(id);
            IEnumerable<SelectListItem> outEnumValues;
            if (filter!=null)
            {
                outEnumValues= filter.EnumValues.Select(item => new SelectListItem
                {
                    Text = item.Key,
                    Value = item.Id.ToString()
                });
            }
            else
            {
                outEnumValues = new List<SelectListItem>();

            }

            ViewBag.EnumValuesSelectList = outEnumValues;
            return PartialView(indx);
        }
        [Authorize]
        public ActionResult AddOrUpdate(int id = 0)
        {
            var model = _productRepository.Get(id);
           
            var categoriesViewModel = _categoryRepository.GetAll().Select(i => new Models.CategoryAdminViewModel {
                Checked=false,
                Id=i.Id,
                Title=i.Title
            }).ToList();

          

            if (model!=null&& model.Categories!=null)
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

            ViewBag.FiltersSelectList = _filterRepository.GetAll().ToList().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });

            ViewBag.EnumValuesSelectList = _enumRepositopy.GetAll().ToList().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            });

            ViewBag.AllCategories = categoriesViewModel;
            ViewBag.AllRecommendations = _recommendationRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });

            return View(model);
        }
        [Authorize]
        [HttpPost]  
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(Product model)
        {
            if (model.Id==0)
            {
                model.DateCreation = DateTime.Now;
            }
            model.DateChange = DateTime.Now;

            model.Categories= model.Categories.Where(i => i.Checked == true).ToList();

            _productRepository.AddOrUpdate(model);
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
            var model = _productRepository.GetAllEnumValues(id);
            ViewBag.EnumValuesSelectList = _enumRepositopy.GetAll().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            });
            ViewBag.FiltersSelectList = _filterRepository.GetAll().ToList().Select(item => new SelectListItem
            {
                Text = item.Title,
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
            ViewBag.FiltersSelectList = _filterRepository.GetAll().ToList().Select(item => new SelectListItem
            {
                Text = item.Title,
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
           // _productRepository.Save();
            return null;
        }
        //[HttpPost]
        //public ActionResult DeleteRecomendation(int id, int recomendationId)
        //{
        //    _productRepository.DeleteRecomendation(id, recomendationId);
        //    // _productRepository.Save();
        //    return null;
        //}

        //public ActionResult GetTemplateRecomendation(int id, int count)
        //{
        //    ViewBag.AllRecommendations = _recommendationRepository.GetAll().Select(item => new SelectListItem
        //    {
        //        Text = item.Title,
        //        Value = item.Id.ToString()
        //    });
        //    var product = _productRepository.Get(id);
        //    var model = product.Recommendations.Count;
        //    if (count > model)
        //    {
        //        model = count;
        //    }
        //    return PartialView("_PartialProductTemplateRecomendation", model);
        //}

        public PartialViewResult GetAllImageFromCategory(int id,int imageId)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Images, Image>() ;
            });
            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var galery = _productRepository.GetGalleryImage(id, (int)Common.TypeImage.GalleryItem);
           // var model = mapper.Map<IEnumerable<Image>>(efGalery);
            return PartialView("_PartialProductGallery", galery);
        }
        [Authorize]
        public ActionResult AddToGallery(int id,int imageid)
        {
            _productRepository.AddToGallry(id, imageid, (int)Entity.Enumerable.TypeImage.GaleryItem);
            //_productRepository.Save();
            return GetAllImageFromCategory(id, imageid);
        }
        public ActionResult DeleteToGallery(int id,int imageid)
        {
            _productRepository.RemoveToGallry(id, imageid, (int)Entity.Enumerable.TypeImage.GaleryItem);
            //_productRepository.Save();
            return GetAllImageFromCategory(id, imageid);
        }
        //[Authorize]
        //public ActionResult GetProductReview(int id)
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<DBFirstDAL.Review, Entity.Review>();
        //    });
        //    config.AssertConfigurationIsValid();
        //    var mapper = config.CreateMapper();
        //    var efProduct = _productRepository.FindBy(i => i.Id == id).SingleOrDefault();
        //    if (efProduct!=null)
        //    {
        //        ViewBag.TitlePage = efProduct.Title;
        //    }
        //    var efModel = _productRepository.GetReview(id);
        //    var model = mapper.Map<Review>(efModel);
        //    return View(model);
        //}
        [Authorize]
        public ActionResult Delete(int id)
        {
            var product=_productRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (product!=null)
            {
                _productRepository.Delete(product.Id);
            }
            return RedirectToAction("AdminIndex");
        }

       
       
    }
}