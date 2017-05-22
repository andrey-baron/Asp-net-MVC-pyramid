using AutoMapper;
using DBFirstDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class FilterController : Controller
    {
        FilterRepository _filterRepository;
        EnumValueRepository _enumRepositopy;
        public FilterController()
        {
            _filterRepository = new FilterRepository();
            _enumRepositopy = new EnumValueRepository();

        }
        // GET: Filter
        [Authorize]
        public ActionResult Index()
        {
            var efmodel = _filterRepository.GetAll().ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Filters, Pyramid.Entity.Filter>()
                .ForMember(d => d.Categories, o => o.Ignore());
                cfg.CreateMap<DBFirstDAL.EnumValues,Entity.EnumValue>();
                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
                   .ForMember(d => d.Thumbnail, o => o.Ignore())
                   .ForMember(d => d.Checked, o => o.Ignore())
                   .ForMember(d => d.Products, o => o.Ignore())
                   ;
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var modelAllFilters =
    mapper.Map<IEnumerable<DBFirstDAL.Filters>, List<Pyramid.Entity.Filter>>(efmodel);


            // var model = DBFirstDAL.FilterDAL.GetAll();
            return View(modelAllFilters);
        }
        
        public ActionResult AddOrUpdate(int id=0)
        {
            var efmodel = _filterRepository.FindBy(i=>i.Id==id).SingleOrDefault();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Filters, Pyramid.Entity.Filter>()
                .ForMember(d => d.Categories, o => o.Ignore()); ;
                cfg.CreateMap<DBFirstDAL.EnumValues, Entity.EnumValue>();
                cfg.CreateMap<DBFirstDAL.Categories, Pyramid.Entity.Category>()
                   .ForMember(d => d.Thumbnail, o => o.Ignore())
                   .ForMember(d => d.Checked, o => o.Ignore())
                   .ForMember(d => d.Products, o => o.Ignore())
                   ;
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var modelFilter =
    mapper.Map<DBFirstDAL.Filters, Pyramid.Entity.Filter>(efmodel);


            ViewBag.EnumValuesSelectList= _enumRepositopy.GetAll().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            }); 
            return View(modelFilter);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddOrUpdate(Pyramid.Entity.Filter model)
        {
           
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Pyramid.Entity.Filter,DBFirstDAL.Filters >()
                .ForMember(d=>d.Categories,o=>o.Ignore());
                cfg.CreateMap<Entity.EnumValue,DBFirstDAL.EnumValues>()
                .ForMember(d => d.Filters, o => o.Ignore())
                .ForMember(d => d.Products, o => o.Ignore()); ;
                //cfg.CreateMap<Pyramid.Entity.Category,DBFirstDAL.Categories>();
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var efmodel =
    mapper.Map<Pyramid.Entity.Filter, DBFirstDAL.Filters>(model);
            _filterRepository.AddOrUpdate(efmodel);
            _filterRepository.Save();
           // DBFirstDAL.FilterDAL.AddOrDefault(model);
            return RedirectToAction("index");
        }
        [Authorize]
        public ActionResult GetAllEnumValues(int filterid) {

            var model = _filterRepository.GetAllEnumValues(filterid);
            ViewBag.EnumValuesSelectList= _enumRepositopy.GetAll().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            });
            return PartialView("_PartialFilterAllEnumValues", model);
        }
        public ActionResult GetTemplateEnumValue(int filterid)
        {
            ViewBag.EnumValuesSelectList= _enumRepositopy.GetAll().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            });
            var model = _filterRepository.GetAllEnumValues(filterid).Count();
            return PartialView("_PartialFilterEmptyEnumValue", model);
        }

        public ActionResult DeleteEnumValue(int id,int enumValueId)
        {
            _filterRepository.DeleteEnumValue(id, enumValueId);
            _filterRepository.Save();
            return null;
        }
    }
}