using AutoMapper;
using Common.Models;
using Common.SearchClasses;
using DBFirstDAL.Repositories;
using Entity;
using Newtonsoft.Json;
using Pyramid.Entity;
using Pyramid.Models;
using Pyramid.Models.CategoryModels;
using Pyramid.Models.CommonViewModels;
using Pyramid.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
    public class CategoryController : BaseController
    {
        //DAL.UnitOfWork unitOfWork;
        CategoryRepository _categoryRepository;
        FilterRepository _filterRepository;
        RecommendationRepository _recommendationRepository;
        GlobalOptionRepository _globalRepository;
        RouteItemRepository _routeItemRepository;
        const string defaulCateggorytLink = "/Category/index/";
        public CategoryController()
        {
            _categoryRepository = new CategoryRepository();
            _filterRepository = new FilterRepository();
            _recommendationRepository = new RecommendationRepository();
            _globalRepository = new DBFirstDAL.Repositories.GlobalOptionRepository();
            _routeItemRepository = new RouteItemRepository();
        }
        [Authorize]
        public ActionResult AdminIndex(string currentFilter, string searchString, int? categoryId, int? page)
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
            var searchResult = _categoryRepository.Get(new SearchParamsCategory
            {
                StartIndex = (pageNumber - 1) * objectsPerPage,
                ObjectsCount = objectsPerPage,
                SearchString= searchString
            });
            ViewBag.CurrentFilter = searchString;
            var viewModel = SearchResultViewModel<Category>.CreateFromSearchResult(searchResult, i=>i, 10);

            return View(viewModel);
        }
        [AllowAnonymous]
        public ActionResult Index(int id=0, int sortingOrder=0)
        {
            ViewBag.SortingOrder = sortingOrder;
            if (id == 0)
            {
                var efRootCategories = _categoryRepository.GetRootCategoriesWithSubs();

                var modelRootCategories = AllCategoriesViewModel.ToModelEnumerable(efRootCategories);

                ViewBag.MetaTitle = "Категории";
                return View("ViewRootCategories", modelRootCategories);
            }
            SearchParamsCategory searchParamsCategory;

            var curCookieName = this.HttpContext.Request.Cookies.AllKeys.FirstOrDefault(i => i == "category_" + id.ToString());

            var curCookie = this.HttpContext.Request.Cookies.Get(curCookieName);
            CategoryViewModel viewModel;
            if (curCookie != null)
            {
                var jsonObj = JsonConvert.DeserializeObject<CategoryFiltersJsonModel>(curCookie.Value);
                
                var checkedEnumValueIds = new List<int>();
                var filterSearchModel = new List<FilterSearchModel>();
                foreach (var item in jsonObj.Filters)
                {
                    filterSearchModel.Add(new FilterSearchModel()
                    {
                        Id = item.Id,
                        EventValueIds = item.EnumValueIds,
                    });
                    checkedEnumValueIds.AddRange(item.EnumValueIds);
                }
                    searchParamsCategory = new SearchParamsCategory(null, id, (int)jsonObj.MaxPrice, (int)jsonObj.MinPrice, filterSearchModel);
                
                viewModel = CategoryViewModel.ToModel(_categoryRepository.GetBySearchParams(searchParamsCategory));
                viewModel.CurrentMaxPrice = (int)jsonObj.MaxPrice;
                viewModel.CurrentMinPrice = (int)jsonObj.MinPrice;
               
                foreach (var item in viewModel.Filters)
                {
                    foreach (var enumVal in item.EnumValues)
                    {
                        if (checkedEnumValueIds.Contains(enumVal.Id))
                        {
                            enumVal.Checked = true;
                        }
                    }
                }
            }
            else
            {
                searchParamsCategory = new SearchParamsCategory(id) { IsSearchOnlyPublicProduct=true};
                var t = _categoryRepository.GetBySearchParams(searchParamsCategory);
                viewModel = CategoryViewModel.ToModel(t);
               
            }
            //var productsEnumValues = new List<int>();
            //foreach (var item in viewModel.Products)
            //{
            //    productsEnumValues.AddRange(item.EnumValues.Select(s => s.Id));
            //}
            foreach (var item in viewModel.Filters)
            {
                item.EnumValues = (item.EnumValues).Where(w => searchParamsCategory.OutEnumValueIds.Contains(w.Id)).OrderBy(o=>o,new Tools.Compare.CompareCategoryEnumValueViewModel()).ToList();
            }
                viewModel.ExistProducts = searchParamsCategory.ExistProductsInBd;
            if (sortingOrder != 0)
            {
                switch (sortingOrder)
                {
                    case (int)Common.TypeSort.Price:
                        viewModel.Products.Sort(new Tools.Compare.ProductCompareByPrice());
                        break;
                    case (int)Common.TypeSort.Name:
                        viewModel.Products.Sort(new Tools.Compare.ProductCompareByTitle());
                        break;
                    case (int)Common.TypeSort.Popular:
                        viewModel.Products.Sort(new Tools.Compare.ProductCompareByPopular());
                        break;
                    default:
                        break;
                }
            }
            
            ViewBag.BredCrumbs = _categoryRepository.GetBreadCrumbs(id);
            viewModel.NestedCategories = _categoryRepository.GetNestedCategories(id).Select(s=> CategoryShortViewModel.ToModel(s)).ToList();
            viewModel.MaxPrice = _categoryRepository.GetMaxPriceFromCategory(id);
            viewModel.MinPrice = _categoryRepository.GetMinPriceFromCategory(id);
            ViewBag.MetaTitle = viewModel.Seo != null ? viewModel.Seo.MetaTitle : viewModel.Title;
            ViewBag.Keywords = viewModel.Seo != null ? viewModel.Seo.MetaKeywords : null;
            ViewBag.Description = viewModel.Seo != null ? viewModel.Seo.MetaDescription : null;

            bool WillBeAddedFlag = viewModel.Products.Any(i => i.TypeStatusProduct == Common.TypeStatusProduct.WillBeAdded);
            ViewBag.WillBeAddedFlag = WillBeAddedFlag;
            if (WillBeAddedFlag)
            {
                ViewBag.WillBeAddedFlagText = _globalRepository.Get("footnote");
            }
            return View(viewModel);

        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Index(CategoryViewModel model , int sortingOrder=0 )
        {
            CategoryFiltersJsonModel cookieModel = CategoryFiltersJsonModel.ConvertToJsonModel(model);

            var jsonObj=JsonConvert.SerializeObject(cookieModel);

            HttpCookie cookie = new HttpCookie("category_" + cookieModel.CategoryId, jsonObj);
            cookie.Expires = DateTime.Now.AddDays(10);

            this.HttpContext.Response.Cookies.Add(cookie);
            //var obj = JsonConvert.DeserializeObject<CategoryViewModel>(cookie.Value);

            ViewBag.SortingOrder = sortingOrder;

            var max = model.CurrentMaxPrice;
            var min = model.CurrentMinPrice;

            var Filters = model.Filters.Where(i => i.EnumValues.Any(t => t.Checked == true) && i.EnumValues.Count > 0).ToList();
            //var checkedEnumValueIds = new List<int>();
            var filterSearchModel = new List<FilterSearchModel>();
            foreach (var item in Filters)
            {
                filterSearchModel.Add(new FilterSearchModel() {
                    Id=item.Id,
                    Title=item.Title,
                    EventValueIds = item.EnumValues.Where(i => i.Checked).Select(i => i.Id)
            });
                
                //checkedEnumValueIds.AddRange(item.EnumValues.Where(t => t.Checked == true).Select(s => s.Id));
            }

            SearchParamsCategory searchParamsCategory= new SearchParamsCategory(null,model.Id, model.CurrentMaxPrice, model.CurrentMinPrice, filterSearchModel);
            var viewModel = CategoryViewModel.ToModel(_categoryRepository.GetBySearchParams(searchParamsCategory));
            viewModel.ExistProducts = searchParamsCategory.ExistProductsInBd;
            foreach (var item in viewModel.Filters)
            {
                item.EnumValues = (item.EnumValues).Where(w => searchParamsCategory.OutEnumValueIds.Contains(w.Id)).ToList();
            }
            if (sortingOrder != 0)
            {
                switch (sortingOrder)
                {
                    case (int)Common.TypeSort.Price:
                        viewModel.Products.Sort(new Tools.Compare.ProductCompareByPrice());
                        break;
                    case (int)Common.TypeSort.Name:
                        viewModel.Products.Sort(new Tools.Compare.ProductCompareByTitle());
                        break;
                    case (int)Common.TypeSort.Popular:
                        viewModel.Products.Sort(new Tools.Compare.ProductCompareByPopular());
                        break;
                    default:
                        break;
                }
            }
            ViewBag.BredCrumbs = _categoryRepository.GetBreadCrumbs(model.Id);
            ViewBag.MetaTitle = viewModel.Seo != null ? viewModel.Seo.MetaTitle : viewModel.Title;
            ViewBag.Keywords = viewModel.Seo != null ? viewModel.Seo.MetaKeywords : null;
            ViewBag.Description = viewModel.Seo != null ? viewModel.Seo.MetaDescription : null;

            viewModel.MaxPrice = _categoryRepository.GetMaxPriceFromCategory(model.Id);
            viewModel.MinPrice = _categoryRepository.GetMinPriceFromCategory(model.Id);
            viewModel.CurrentMaxPrice = max;
            viewModel.CurrentMinPrice = min;
            bool WillBeAddedFlag = viewModel.Products.Any(i => i.TypeStatusProduct == Common.TypeStatusProduct.WillBeAdded);
            ViewBag.WillBeAddedFlag = WillBeAddedFlag;
            if (WillBeAddedFlag)
            {
                ViewBag.WillBeAddedFlagText = _globalRepository.Get("footnote");
            }
            return View(viewModel);
        }
        [Authorize]
        public ActionResult AddOrUpdate(int id=0)
        {
            Category category = null;
            if (id==0)
            {
                category = new Category();
            }
            else
            {
                category= _categoryRepository.Get(id);
            }

         
            ViewBag.CategoriesSelectListItem = CategoryRepository.GetAllWithoutCategoryId(id).Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            }); ;

            var routeItem = _routeItemRepository.Get((string)ControllerContext.RequestContext.RouteData.Values["controller"],
               "Index",
               (int)ControllerContext.RequestContext.RouteData.Values["id"]);
            ViewBag.CurrentFriendlyUrl = routeItem != null ? routeItem.FriendlyUrl : null;

            return View(category);
        }
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(Pyramid.Entity.Category model)
        {
            _categoryRepository.AddOrUpdate(model);
            var routeItem = new RouteItem(0, null, (string)ControllerContext.RequestContext.RouteData.Values["controller"],
               "Index",
               model.Id)
            { Type=Common.TypeEntityFromRouteEnum.CategoryType};
            _routeItemRepository.AddOrUpdate(routeItem);
            return RedirectToAction("AdminIndex");
        }

        public ActionResult GetAllFilter(int id)
        {
            var model = _categoryRepository.GetFilters(id);
            return PartialView("_PartialCategoryAllFilters", model);
        }
       
        public ActionResult GetTemplateFilter(int id,int count)
        {
            
            var model = _categoryRepository.GetFilters(id).Count();
            if (id == 0|| count > model)
            {
                model = count;
            }
            
            ViewBag.FiltersSelectList=_filterRepository.GetAll() .Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            return PartialView("_PartialCategoryEmptyFilter", model);
        }
        public ActionResult DeleteFilter(int id,int filterid)
        {
            _categoryRepository.DeleteFilter(id, filterid);
            return null;
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            var efmodel = _categoryRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (efmodel!=null)
            {
                _categoryRepository.Delete(efmodel.Id);
            }
            return RedirectToAction("AdminIndex");
        }
        [Authorize]
        public ActionResult DeleteCategory(int id)
        {
            var efmodel = _categoryRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (efmodel != null)
            {
                _categoryRepository.Delete(efmodel.Id);
            }
            return RedirectToActionPermanent("AdminIndex");
        }

        [Authorize]
        public ActionResult GetProductTemplateDropDownListForPointId(int id,int pointindex)
        {
            ViewBag.ProductsSelectListItem = _categoryRepository.GetProductsByCategoryId(id).Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            return PartialView("_PartialGetProductTemplateDropDownList", pointindex);
        }

        public ActionResult GetRecommendationTemplate(int id,int count)
        {
            var model = _categoryRepository.GetRecommendations(id).Count();
            if (id == 0 || count > model)
            {
                model = count;
            }

            ViewBag.RecommendationSelectList = _recommendationRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            return PartialView("_PartialRecommendationTemplate", model);
        }
        public ActionResult DeleteRecommendation(int id, int recommendationid)
        {
            _categoryRepository.DeleteRecommendation(id, recommendationid);
            return null;
        }
        public ActionResult GetAllRecommendation(int id)
        {
            var model = _categoryRepository.GetRecommendations(id);
            return PartialView("_PartialCategoryAllRecommendation", model);
        }

    }
}