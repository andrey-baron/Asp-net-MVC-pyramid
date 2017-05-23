using AutoMapper;
using DBFirstDAL.Repositories;
using Pyramid.Models;
using Pyramid.Models.CategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class CategoryController : Controller
    {
        //DAL.UnitOfWork unitOfWork;
        CategoryRepository _categoryRepository;
        FilterRepository _filterRepository;

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
                .ForMember(d => d.Products, o => o.Ignore());

                //cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                //.ForAllMembers(i => i.Ignore());
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var model = mapper.Map<IEnumerable<DBFirstDAL.Categories>, List<Pyramid.Entity.Category>>(_categoryRepository.GetAll().ToList());
            
            return View(model);
        }
        public ActionResult Index(int id=0)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
                .ForMember(d => d.Thumbnail, o => o.Ignore())
                .ForMember(d => d.Checked, o => o.Ignore())
                ;

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.Ignore())
                .ForMember(d => d.ProductValues, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.Filters, Pyramid.Entity.Filter>()
                .ForMember(d => d.Categories, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.EnumValues, Entity.EnumValue>();

                cfg.CreateMap < DBFirstDAL.EnumValues,CategoryEnumValueViewModel > ()
                .ForMember(d => d.Checked, o => o.UseValue(false));

                cfg.CreateMap<DBFirstDAL.Filters, CategoryFilterViewModel>()
                ;

                cfg.CreateMap<DBFirstDAL.Categories, CategoryViewModel>();

                cfg.CreateMap<DBFirstDAL.DataModels.RootCategory, Models.AllCategoriesViewModel>();

            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            if (id == 0)
            {
                var rootCategories = _categoryRepository.GetRootCategoriesWithSubs();
               
                var modelRootCategories =
                    mapper.Map<IEnumerable<DBFirstDAL.DataModels.RootCategory>, IEnumerable< Models.AllCategoriesViewModel>>(rootCategories );
                return View("ViewRootCategories", modelRootCategories);
            }

            var EfModel= _categoryRepository.FindBy(i => i.Id == id).SingleOrDefault();
            CategoryViewModel model = mapper.Map<CategoryViewModel>(EfModel);

            return View(model);
        }
        [HttpPost]
        public ActionResult Index(CategoryViewModel model)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
                .ForMember(d => d.Thumbnail, o => o.Ignore())
                .ForMember(d => d.Checked, o => o.Ignore())
                ;

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.Ignore())
                .ForMember(d => d.ProductValues, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.EnumValues, CategoryEnumValueViewModel>()
                .ForMember(d => d.Checked, o => o.UseValue(false));

                cfg.CreateMap<DBFirstDAL.Filters, CategoryFilterViewModel>()
                ;
                cfg.CreateMap<DBFirstDAL.Filters, Pyramid.Entity.Filter>()
                .ForMember(d => d.Categories, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.EnumValues, Entity.EnumValue>();

                cfg.CreateMap<CategoryFilterViewModel,DBFirstDAL.Filters >()
                .ForMember(d => d.Categories, o => o.Ignore());

                cfg.CreateMap<CategoryEnumValueViewModel,DBFirstDAL.EnumValues>()
               .ForMember(d => d.Filters, o => o.Ignore())
               .ForMember(d => d.Products, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.Categories, CategoryViewModel>();


            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();


            var Filters= model.Filters.Where(i=>i.EnumValues.All(t => t.Checked==true)&& i.EnumValues.Count>0).ToList();
             var efFilters= mapper.Map<IEnumerable<CategoryFilterViewModel>,List<DBFirstDAL.Filters>>(Filters.ToList());
            var EfModel = _categoryRepository.FindBy(i => i.Id == model.Id).SingleOrDefault();
            model = mapper.Map<CategoryViewModel>(EfModel);
            var efnumValues=efFilters.Select(l => l.EnumValues);
            IEnumerable<DBFirstDAL.EnumValues> unionValues = new List<DBFirstDAL.EnumValues>();
            foreach (var item in efnumValues)
            {
                unionValues = unionValues.Union(item);
            }

            var efoutProduct =_categoryRepository.GetWithCheckedEnumValues(model.Id, unionValues);
            model.Products = mapper.Map<List<Pyramid.Entity.Product>>(efoutProduct);

            
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
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var efmodel=_categoryRepository.FindBy(i => i.Id == id).SingleOrDefault();
            var model = mapper.Map<DBFirstDAL.Categories, Pyramid.Entity.Category>(efmodel);
            if (model == null)
            {
                model = new Entity.Category();
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
                ;

                cfg.CreateMap<Pyramid.Entity.Product,DBFirstDAL.Products >()
                .ForAllMembers(i => i.Ignore());

                cfg.CreateMap< Pyramid.Entity.Filter, DBFirstDAL.Filters>()
                 .ForMember(d => d.EnumValues, o => o.Ignore()); ;
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var efModel = mapper.Map<DBFirstDAL.Categories>(model);

            _categoryRepository.AddOrUpdate(efModel);
            _categoryRepository.Save();
            //DBFirstDAL.CategoryDAL.AddOrUpdateEntity(model);

            return RedirectToAction("AdminIndex");
        }
        public ActionResult GetAllFilter(int id)
        {
            var efmodel = _categoryRepository.GetFilters(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Pyramid.Entity.Filter, DBFirstDAL.Filters>()
                .ForMember(d=>d.Categories,o=>o.Ignore())
                .ForMember(d => d.EnumValues, o => o.Ignore());
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var model = mapper.Map<DBFirstDAL.Filters>(efmodel);
            return PartialView("_PartialCategoryAllFilters", model);
        }
        public ActionResult GetTemplateFilter(int id)
        {
            var model = _categoryRepository.GetFilters(id).Count();
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
    }
}