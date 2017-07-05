using AutoMapper;
using DBFirstDAL.Repositories;
using Entity;
using Pyramid.Models;
using Pyramid.Models.CategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class CategoryController : BaseController
    {
        //DAL.UnitOfWork unitOfWork;
        CategoryRepository _categoryRepository;
        FilterRepository _filterRepository;
        const string defaulCateggorytLink = "/Category/index/";
        public CategoryController()
        {
            _categoryRepository = new CategoryRepository();
            _filterRepository = new FilterRepository();

        }
        [Authorize]
        public ActionResult AdminIndex()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
                .ForMember(d => d.Thumbnail, o => o.Ignore())
                .ForMember(d => d.Checked, o => o.Ignore())
                .ForMember(d => d.Filters, o => o.Ignore())
                .ForMember(d => d.Products, o => o.Ignore())
                .ForMember(d => d.Seo, o => o.Ignore())
                ;

                //cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                //.ForAllMembers(i => i.Ignore());
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var model = mapper.Map<IEnumerable<DBFirstDAL.Categories>, List<Pyramid.Entity.Category>>(_categoryRepository.GetAll().ToList());
            model.Reverse();
            return View(model);
        }
        public ActionResult Index(int id=0, int sortingOrder=0)
        {
            ViewBag.SortingOrder = sortingOrder;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
                .ForMember(d => d.Thumbnail, o => o.MapFrom(m=>
                m.CategoryImages.FirstOrDefault(f=>f.CategoryId==m.Id&&f.TypeImage==(int)Common.TypeImage.Thumbnail)!=null?
                m.CategoryImages.FirstOrDefault(f => f.CategoryId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images:new DBFirstDAL.Images()))
                .ForMember(d => d.Checked, o => o.Ignore())
                ;

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.
                MapFrom(m =>
                m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()))
                .ForMember(d => d.ProductValues, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.Filters, Pyramid.Entity.Filter>()
                .ForMember(d => d.Categories, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.EnumValues, Entity.EnumValue>();

                cfg.CreateMap < DBFirstDAL.EnumValues,CategoryEnumValueViewModel > ()
                .ForMember(d => d.Checked, o => o.UseValue(false));

                cfg.CreateMap<DBFirstDAL.Filters, CategoryFilterViewModel>()
                ;

                cfg.CreateMap<DBFirstDAL.Categories, CategoryViewModel>()
                .ForMember(d => d.MinPrice, o => o.UseValue(false))
                .ForMember(d => d.MaxPrice, o => o.UseValue(false))
                .ForMember(d => d.CurrentMinPrice, o => o.UseValue(false))
                .ForMember(d => d.CurrentMaxPrice, o => o.UseValue(false))
                .ForMember(d => d.Thumbnail, o => o.MapFrom(m =>
                m.CategoryImages.FirstOrDefault(f => f.CategoryId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                m.CategoryImages.FirstOrDefault(f => f.CategoryId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()))
                .ForMember(d => d.NestedCategories, o => o.MapFrom(m=>m.Categories1))
;
                cfg.CreateMap<DBFirstDAL.Categories, CategoryShortViewModel>()
                .ForMember(d => d.Thumbnail, o => o.MapFrom(m =>
                m.CategoryImages.FirstOrDefault(f => f.CategoryId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                m.CategoryImages.FirstOrDefault(f => f.CategoryId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()));


                cfg.CreateMap<DBFirstDAL.DataModels.CategoryWithThumbnail, Pyramid.Entity.Category>()
               .ForMember(d => d.Checked, o => o.Ignore())
               .ForMember(d => d.Filters, o => o.Ignore())
               .ForMember(d => d.ParentId, o => o.Ignore())
               .ForMember(d => d.FlagRoot, o => o.Ignore())
               .ForMember(d => d.Products, o => o.Ignore())
                .ForMember(d => d.OneCId, o => o.Ignore())
                .ForMember(d => d.Seo, o => o.Ignore())
                .ForMember(d => d.SeoId, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.DataModels.RootCategory, Models.AllCategoriesViewModel>()
                ;
                cfg.CreateMap<DBFirstDAL.Images, Entity.Image>();

                cfg.CreateMap<DBFirstDAL.Seo, Seo>();

            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            if (id == 0)
            {
                var rootCategories = _categoryRepository.GetRootCategoriesWithSubs();
               
                var modelRootCategories =
                    mapper.Map<IEnumerable<DBFirstDAL.DataModels.RootCategory>, IEnumerable< Models.AllCategoriesViewModel>>(rootCategories );

                ViewBag.MetaTitle = "Категории";
                return View("ViewRootCategories", modelRootCategories);
            }

            var EfModel= _categoryRepository.FindBy(i => i.Id == id).SingleOrDefault();
            //ViewBag.Title = EfModel.Title;
            //var IsNestedCategory = _categoryRepository.IsCategoryNested(EfModel.Id);
            //if (IsNestedCategory&& EfModel.ParentId != null)
            //{

            //    var rootCategory = _categoryRepository.GetRootCategoryWithSubs(id);
            //    var modelRootCategory =
            //    mapper.Map<DBFirstDAL.DataModels.RootCategory, Models.AllCategoriesViewModel>(rootCategory);

            //    return View("ViewNestedCategories", modelRootCategory);

            //}


            //if (EfModel.ParentId==null)
            //{
            //    var rootCategories = _categoryRepository.GetRootCategoriesWithSubs();

            //    var modelRootCategories =
            //        mapper.Map<IEnumerable<DBFirstDAL.DataModels.RootCategory>, IEnumerable<Models.AllCategoriesViewModel>>(rootCategories);
               
            //    return View("ViewRootCategories", modelRootCategories);
            //}
           
            List<Models.BreadCrumbViewModel> breadcrumbs = new List<Models.BreadCrumbViewModel>();
            breadcrumbs.Add(new Models.BreadCrumbViewModel()
            {
                Title = EfModel.Title
            ,
                Link = defaulCateggorytLink + id.ToString()
            });
            var flagstop = true;
            var cat = EfModel.Categories2;
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
            CategoryViewModel model = mapper.Map<CategoryViewModel>(EfModel);
            if (sortingOrder != 0)
            {
                switch (sortingOrder)
                {
                    case (int)Common.TypeSort.Price:
                        model.Products.Sort(new Tools.Compare.ProductCompareByPrice());
                        break;
                    case (int)Common.TypeSort.Name:
                        model.Products.Sort(new Tools.Compare.ProductCompareByTitle());
                        break;
                    case (int)Common.TypeSort.Popular:
                        model.Products.Sort(new Tools.Compare.ProductCompareByPopular());
                        break;
                    default:
                        break;
                }

            }
            model.MaxPrice = _categoryRepository.GetMaxPriceFromCategory(id);
            model.MinPrice = _categoryRepository.GetMinPriceFromCategory(id);

            ViewBag.MetaTitle = model.Seo.MetaTitle;
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(CategoryViewModel model , int sortingOrder=0 )
        {
            ViewBag.SortingOrder = sortingOrder;
            var max = model.CurrentMaxPrice;
            var min = model.CurrentMinPrice;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
               .ForMember(d => d.Thumbnail, o => o.MapFrom(m =>
               m.CategoryImages.FirstOrDefault(f => f.CategoryId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
               m.CategoryImages.FirstOrDefault(f => f.CategoryId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()))
               .ForMember(d => d.Checked, o => o.Ignore())
               .ForMember(d => d.SeoId, o => o.Ignore())
               
               ;

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.
                MapFrom(m =>
                m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()))
                .ForMember(d => d.ProductValues, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.Filters, Pyramid.Entity.Filter>()
                .ForMember(d => d.Categories, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.EnumValues, Entity.EnumValue>();

                cfg.CreateMap<DBFirstDAL.EnumValues, CategoryEnumValueViewModel>()
                .ForMember(d => d.Checked, o => o.UseValue(false));

                cfg.CreateMap<DBFirstDAL.Filters, CategoryFilterViewModel>()
                ;
                cfg.CreateMap< CategoryFilterViewModel, DBFirstDAL.Filters>()
                .ForMember(d => d.Categories, o => o.Ignore())
                //.ForMember(d => d.EnumValues, o => o.Ignore())
               ;
                cfg.CreateMap<CategoryEnumValueViewModel, DBFirstDAL.EnumValues>()
                .ForMember(d => d.Filters, o => o.Ignore())
                .ForMember(d => d.Products, o => o.Ignore())
                .ForMember(d => d.TypeValue, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.Categories, CategoryViewModel>()
                .ForMember(d => d.MinPrice, o => o.UseValue(false))
                .ForMember(d => d.MaxPrice, o => o.UseValue(false))
                .ForMember(d => d.CurrentMinPrice, o => o.UseValue(false))
                .ForMember(d => d.CurrentMaxPrice, o => o.UseValue(false))
                .ForMember(d => d.Thumbnail, o => o.MapFrom(m =>
                m.CategoryImages.FirstOrDefault(f => f.CategoryId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                m.CategoryImages.FirstOrDefault(f => f.CategoryId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()))
                .ForMember(d => d.NestedCategories, o => o.MapFrom(m => m.Categories1))
                .ForMember(d => d.Seo, o => o.MapFrom(m=>m.Seo))
;
                cfg.CreateMap<DBFirstDAL.Categories, CategoryShortViewModel>()
                .ForMember(d => d.Thumbnail, o => o.MapFrom(m =>
                m.CategoryImages.FirstOrDefault(f => f.CategoryId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                m.CategoryImages.FirstOrDefault(f => f.CategoryId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()));


                cfg.CreateMap<DBFirstDAL.DataModels.CategoryWithThumbnail, Pyramid.Entity.Category>()
               .ForMember(d => d.Checked, o => o.Ignore())
               .ForMember(d => d.Filters, o => o.Ignore())
               .ForMember(d => d.ParentId, o => o.Ignore())
               .ForMember(d => d.FlagRoot, o => o.Ignore())
               .ForMember(d => d.Products, o => o.Ignore())
                .ForMember(d => d.OneCId, o => o.Ignore())
                 .ForMember(d => d.SeoId, o => o.Ignore())
                 .ForMember(d => d.Seo, o => o.Ignore())
                 ;

                cfg.CreateMap<DBFirstDAL.DataModels.RootCategory, Models.AllCategoriesViewModel>()
                ;
                cfg.CreateMap<DBFirstDAL.Images, Entity.Image>();

                cfg.CreateMap<DBFirstDAL.Seo, Seo>();
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();


            var Filters= model.Filters.Where(i=>i.EnumValues.Any(t => t.Checked==true)&& i.EnumValues.Count>0).ToList();
            var checkedEnumValueIds = new List<int>();
            foreach (var item in Filters)
            {
                checkedEnumValueIds.AddRange(item.EnumValues.Where(t => t.Checked == true).Select(s => s.Id));
            }


             //var efFilters= mapper.Map<IEnumerable<CategoryFilterViewModel>,List<DBFirstDAL.Filters>>(Filters.ToList());
            var EfModel = _categoryRepository.FindBy(i => i.Id == model.Id).SingleOrDefault();
            model = mapper.Map<CategoryViewModel>(EfModel);
            if (sortingOrder!=null)
            {
                switch (sortingOrder)
                {
                    case (int)Common.TypeSort.Price:
                        break;
                    case (int)Common.TypeSort.Name:
                        model.Products.Sort(new Tools.Compare.ProductCompareByTitle());
                        break;
                    case (int)Common.TypeSort.Popular:
                        break;
                    default:
                        break;
                }
                
            }
            //var efnumValues=efFilters.Select(l => l.EnumValues);
            //IEnumerable<DBFirstDAL.EnumValues> unionValues = new List<DBFirstDAL.EnumValues>();
            //foreach (var item in efnumValues)
            //{
            //    unionValues = unionValues.Union(item);
            //}
            List<Models.BreadCrumbViewModel> breadcrumbs = new List<Models.BreadCrumbViewModel>();
            breadcrumbs.Add(new Models.BreadCrumbViewModel()
            {
                Title = EfModel.Title
            ,
                Link = defaulCateggorytLink + model.Id.ToString()
            });
            var flagstop = true;
            var cat = EfModel.Categories2;
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

            var efoutProduct =_categoryRepository.GetWithCheckedEnumValues(model.Id, checkedEnumValueIds);
            efoutProduct=efoutProduct.Where(i => i.Price >= min && i.Price <= max).ToList();
            model.Products = mapper.Map<List<Pyramid.Entity.Product>>(efoutProduct);
            model.MaxPrice = _categoryRepository.GetMaxPriceFromCategory(model.Id);
            model.MinPrice = _categoryRepository.GetMinPriceFromCategory(model.Id);
            model.CurrentMinPrice = min;
            model.CurrentMaxPrice = max;

            ViewBag.MetaTitle = EfModel.Seo.MetaTitle;
            return View(model);
        }
        [Authorize]
        public ActionResult AddOrUpdate(int id=0)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
                .ForMember(d => d.Thumbnail, o => o.Ignore())
                .ForMember(d => d.Checked, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForAllMembers(i => i.Ignore());

                cfg.CreateMap<DBFirstDAL.Filters, Pyramid.Entity.Filter>()
                 .ForMember(d => d.EnumValues, o => o.Ignore())
                 .ForMember(d => d.Categories, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.Images, Pyramid.Entity.Image>()
                ;

                cfg.CreateMap<DBFirstDAL.Seo, Seo>()
                ;
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var efmodel=_categoryRepository.FindBy(i => i.Id == id).SingleOrDefault();
            
            var model = mapper.Map<DBFirstDAL.Categories, Pyramid.Entity.Category>(efmodel);
            
            if (model == null)
            {
                model = new Entity.Category();
            }
            else
            {
               var efImage= _categoryRepository.GetThumbnail(efmodel.Id, (int)Entity.Enumerable.TypeImage.Thumbnail);
               model.Thumbnail= mapper.Map<DBFirstDAL.Images, Pyramid.Entity.Image>(efImage);
            }
            ViewBag.CategoriesSelectListItem = DBFirstDAL.CategoryDAL.GetAllWithoutCategoryId(id).Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            }); ;
            

            
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddOrUpdate(Pyramid.Entity.Category model)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Pyramid.Entity.Category, DBFirstDAL.Categories>()
               
                .ForMember(d => d.Categories1, o => o.Ignore())
                .ForMember(d => d.Categories2, o => o.Ignore())
                .ForMember(d => d.CategoryImages, o => o.Ignore())
                .ForMember(d => d.Recommendations, o => o.Ignore())
                .ForMember(d => d.HomeEntity, o => o.Ignore())
                ;

                cfg.CreateMap<Pyramid.Entity.Product,DBFirstDAL.Products >()
                .ForAllMembers(i => i.Ignore());

                cfg.CreateMap< Pyramid.Entity.Filter, DBFirstDAL.Filters>()
                 .ForMember(d => d.EnumValues, o => o.Ignore());

                cfg.CreateMap< Seo, DBFirstDAL.Seo>()
                .ForMember(d => d.Categories, o => o.Ignore());
                ;

            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var efModel = mapper.Map<DBFirstDAL.Categories>(model);

            

            _categoryRepository.AddOrUpdate(efModel);
            if (model.Thumbnail != null && model.Thumbnail.Id != 0)
            {
                _categoryRepository.SetThumbnail(efModel.Id, model.Thumbnail.Id, (int)Entity.Enumerable.TypeImage.Thumbnail);

            }
            _categoryRepository.Save();
            //DBFirstDAL.CategoryDAL.AddOrUpdateEntity(model);

            return RedirectToAction("AdminIndex");
        }
        public ActionResult GetAllFilter(int id)
        {
            var efmodel = _categoryRepository.GetFilters(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Filters,Pyramid.Entity.Filter>()
                .ForMember(d=>d.Categories,o=>o.Ignore())
                .ForMember(d => d.EnumValues, o => o.Ignore());
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var model = mapper.Map<List<Pyramid.Entity.Filter>>(efmodel);
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
            _categoryRepository.Save();
            return null;
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            var efmodel = _categoryRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (efmodel!=null)
            {
                _categoryRepository.Delete(efmodel);
            }
            _categoryRepository.Save();
            return RedirectToAction("AdminIndex");
        }
        [Authorize]
        public ActionResult DeleteCategory(int id)
        {
            var efmodel = _categoryRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (efmodel != null)
            {
                _categoryRepository.Delete(efmodel);
            }
            _categoryRepository.Save();
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

        
    }
}