using AutoMapper;
using DBFirstDAL.Repositories;
using PagedList;
using Pyramid.Entity;
using Pyramid.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{   [Authorize]
    public class EventController : BaseController
    {
        private CategoryRepository _categoryRepository;
        private EventRepository _eventRepository;

        public EventController()
        {
            _eventRepository = new EventRepository();
            _categoryRepository = new CategoryRepository();
        }
        // GET: Event
        [AllowAnonymous]
        public ActionResult Index()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Events, Pyramid.Entity.Event>()
                .ForMember(d => d.Image, o => o
                    .MapFrom(m =>
                m.EventImages.Images));

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
              .ForMember(d => d.EnumValues, o => o.Ignore())
               .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                 .ForMember(d => d.ProductValues, o => o.Ignore())
                  .ForMember(d => d.ThumbnailId, o => o.Ignore())
                   .ForMember(d => d.ThumbnailImg, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.Images, Entity.Image>();


            });


            List<Models.BreadCrumbViewModel> breadcrumbs = new List<Models.BreadCrumbViewModel>();
            breadcrumbs.Add(new Models.BreadCrumbViewModel()
            {
                Title = "Акции"
            });
            ViewBag.BredCrumbs = breadcrumbs;

            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var efEvents = _eventRepository.GetAll().ToList();

            var model = mapper.Map <IEnumerable<DBFirstDAL.Events>, IEnumerable<Event>>(efEvents);

            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Get(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Events, Pyramid.Entity.Event>()
                .ForMember(d => d.Image, o => o
                    .MapFrom(m =>
                m.EventImages.Images));

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
              .ForMember(d => d.EnumValues, o => o.Ignore())
               .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                 .ForMember(d => d.ProductValues, o => o.Ignore())
                  .ForMember(d => d.ThumbnailId, o => o.Ignore())
                                     .ForMember(d => d.ThumbnailImg, o => o.
                MapFrom(m =>
                m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail) != null ?
                m.ProductImages.FirstOrDefault(f => f.ProductId == m.Id && f.TypeImage == (int)Common.TypeImage.Thumbnail).Images : new DBFirstDAL.Images()));

                cfg.CreateMap<DBFirstDAL.Images, Entity.Image>();


            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            var efEvent = _eventRepository.FindBy(f => f.Id == id).SingleOrDefault();
            var model = mapper.Map<Event>(efEvent);
            if (efEvent!=null)
            {
                List<Models.BreadCrumbViewModel> breadcrumbs = new List<Models.BreadCrumbViewModel>();
                breadcrumbs.Add(new Models.BreadCrumbViewModel()
                {
                    Link = "/Event/Index",
                    Title = "Акции"
                });
                breadcrumbs.Add(new Models.BreadCrumbViewModel()
                {
                    Title = model.Title
                });
                ViewBag.BredCrumbs = breadcrumbs;
            }
           

           

            return View(model);
        }
        public ActionResult ManageIndex(int? page)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Events, Pyramid.Entity.Event>()
                .ForMember(d => d.Image, o => o
                    .MapFrom(m =>
                m.EventImages.Images))
                 .ForMember(d => d.Products, o => o.Ignore());


                cfg.CreateMap<DBFirstDAL.Images, Entity.Image>();

            });

            config.AssertConfigurationIsValid();
            var pageNumber = page ?? 1;
            var mapper = config.CreateMapper();
            var efEvents = _eventRepository.GetAll().ToList();

            var modelList = new PagedList<Entity.Event>(
            efEvents.Select(u => mapper.Map<DBFirstDAL.Events, Entity.Event>(u)),
            pageNumber, Config.PageSize);
            return View(modelList);
        }
        public ActionResult AddOrUpdate(int id=0)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Events, Pyramid.Entity.Event>()
                .ForMember(d => d.Image, o => o
                    .MapFrom(m =>
                m.EventImages.Images));

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
              .ForMember(d => d.EnumValues, o => o.Ignore())
               .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                 .ForMember(d => d.ProductValues, o => o.Ignore())
                  .ForMember(d => d.ThumbnailId, o => o.Ignore())
                   .ForMember(d => d.ThumbnailImg, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.Images, Entity.Image>();


            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var efEvent = _eventRepository.FindBy(i => i.Id == id).SingleOrDefault();
            var model = new Entity.Event();
            if (efEvent!=null)
            {
                model = mapper.Map<Entity.Event>(efEvent);
            }

            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(Event model)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap< Pyramid.Entity.Event, DBFirstDAL.Events>()
                .ForMember(d => d.EventImages, o => o
                    .MapFrom(m => new DBFirstDAL.EventImages()
                    {
                        EventId=m.Id,
                        ImageId=m.Image.Id
                    }
                ));

                cfg.CreateMap<Pyramid.Entity.Product, DBFirstDAL.Products>()
              .ForMember(d => d.Id, o => o.MapFrom(m => m.Id))
               .ForAllOtherMembers(a => a.Ignore());

                cfg.CreateMap< Entity.Image, DBFirstDAL.Images>()
                .ForMember(d => d.Id, o => o.MapFrom(m => m.Id))
                .ForAllOtherMembers(a => a.Ignore());
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var efmodelFromEntity = mapper.Map<DBFirstDAL.Events>(model);
            _eventRepository.AddOrUpdate(efmodelFromEntity);  
            ViewData["OperationResult"] = "Операция прошла успешно";
            return RedirectToAction("ManageIndex");
        }

        public ActionResult Delete(int id)
        {
            var efEvent = _eventRepository.FindBy(i => i.Id == id).SingleOrDefault();
            _eventRepository.Delete(efEvent);
            _eventRepository.Save();
            return RedirectToAction("ManageIndex");
        }

        public ActionResult TemplateCategoryFromEventProduct(int count,int eventId= 0)
        {
            var efEvent = _eventRepository.FindBy(i => i.Id == eventId).SingleOrDefault();
            var indx = 0;
            if (efEvent!=null&& efEvent.Products!=null)
            {
                indx = efEvent.Products.Count;
            }
            if (count>indx)
            {
                indx = count;
            }
            ViewBag.CategoriesSelectListItem = _categoryRepository.GetAll().Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            return PartialView("TemplateCategoryFromEventProduct", indx);
        }
        public ActionResult TemplateEventProductFromCategoryId(int id, int index)
        {
            ViewBag.ProductsSelectListItem = _categoryRepository.GetProductsByCategoryId(id).Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString()
            });
            return PartialView("TemplateEventProductFromCategoryId", index);
        }
        public ActionResult DeleteProduct(int id,int productid)
        {
            var result=_eventRepository.DeleteReletedProduct(id, productid);
            return Json(new {Status="ok",Result=result });
        }
    }
}