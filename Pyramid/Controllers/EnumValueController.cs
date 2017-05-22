using AutoMapper;
using DBFirstDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class EnumValueController : Controller
    {
        EnumValueRepository _enumRepositopy;
        public EnumValueController()
        {
            _enumRepositopy = new EnumValueRepository();

        }
        // GET: EnumValue
        public ActionResult Index()
        {
            var efmodel = _enumRepositopy.GetAll().ToList();
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<DBFirstDAL.EnumValues,Entity.EnumValue>();
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var modelAllValue =
    mapper.Map<IEnumerable<DBFirstDAL.EnumValues>, List<Entity.EnumValue>>(efmodel);

            return View(modelAllValue);
        }
        public   ActionResult AddOrUpdate(int id=0)
        {
            var efmodel = _enumRepositopy.FindBy(i=>i.Id==id).SingleOrDefault();
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<DBFirstDAL.EnumValues, Entity.EnumValue>();
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var modelValue =
    mapper.Map<DBFirstDAL.EnumValues, Entity.EnumValue>(efmodel);
            return View(modelValue);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(Entity.EnumValue model)
        {

            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<Entity.EnumValue, DBFirstDAL.EnumValues>()
                .ForMember(d=>d.Products,o=>o.Ignore())
                .ForMember(d => d.Filters, o => o.Ignore());
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var efmodel = mapper.Map<Entity.EnumValue, DBFirstDAL.EnumValues>(model);

            _enumRepositopy.AddOrUpdate(efmodel);
            _enumRepositopy.Save();
            return RedirectToAction("index");
        }
    }
}