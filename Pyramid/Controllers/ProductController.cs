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
            bool WillBeAddedFlag = viewModel.Product.TypeStatusProduct == Common.TypeStatusProduct.WillBeAdded;
            bool willbeRelatedFlag= viewModel.RelatedProducts.Any(i => i.TypeStatusProduct == Common.TypeStatusProduct.WillBeAdded);
            WillBeAddedFlag = willbeRelatedFlag || willbeRelatedFlag;
            ViewBag.WillBeAddedFlag = WillBeAddedFlag;
            if (WillBeAddedFlag)
            {
                ViewBag.WillBeAddedFlagText = _globalOptionRepository.Get("footnote");
            }
            return View(viewModel);

        }
        [Authorize]
        public ActionResult AdminIndex(string currentFilter, string searchString, int? categoryId, int? page, bool priority = false, int filled=0,bool isNotUnloading1C=false)
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

            SearchParamsProduct SearchParams = new SearchParamsProduct(searchString, categoryId, priority, filled,  startIndex, objectsPerPage);
            SearchParams.IsNotUnloading1C = isNotUnloading1C;

            var searchResult = _productRepository.Get(SearchParams);

            ViewBag.CurrentFilter = searchString;
            ViewBag.CategoryId = categoryId;
            ViewBag.Filled = (Common.TypeFilledProduct) filled;
            ViewBag.Priority = priority;
            ViewBag.IsNotUnloading1C = isNotUnloading1C;
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