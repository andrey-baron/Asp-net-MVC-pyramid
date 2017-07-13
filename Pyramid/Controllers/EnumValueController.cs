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
            //var efmodel = 
            //var config = new MapperConfiguration(cfg =>
            //{

            //    cfg.CreateMap<DBFirstDAL.EnumValues,Entity.EnumValue>();
            //});


            //config.AssertConfigurationIsValid();

            //var mapper = config.CreateMapper();
            var modelAllValue = _enumRepositopy.GetAll().ToList();

            return View(modelAllValue);
        }
        public   ActionResult AddOrUpdate(int id=0)
        {
            var modelValue = _enumRepositopy.Get(id);
            return View(modelValue);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(Entity.EnumValue model)
        {

            _enumRepositopy.AddOrUpdate(model);

            return RedirectToAction("index");
        }
    }
}