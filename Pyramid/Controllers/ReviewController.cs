using AutoMapper;
using DBFirstDAL.Repositories;
using PagedList;
using Pyramid.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
   
    public class ReviewController : Controller
    {
        ReviewRepository _reviewRepository;

        public ReviewController()
        {
            _reviewRepository = new ReviewRepository();
        }

        #region admin method
      
        // GET: Review
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult NewReviews( int? page)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Review, Pyramid.Entity.Review>()
                .ForMember(d=>d.Content,
                           o=>o.ResolveUsing(r=>r.Content.Length>80? r.Content.Substring(0, 80): r.Content))
                .ForMember(d => d.Product, o => o.ResolveUsing(r => r.Products));

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.ProductValues, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore());
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

           
            var efReviews = _reviewRepository.GetNewReviews().ToList();

            int pageNumber = (page ?? 1);
            var modelList = new PagedList<Entity.Review>(
            efReviews.Select(u => mapper.Map<DBFirstDAL.Review, Entity.Review>(u)).AsQueryable(),
            pageNumber, Config.PageSize);
            ViewBag.Title = "Новые отзывы(не прочитанные)";
            return View("Reviews", modelList);
        }
        [Authorize]
        public ActionResult NotApproved(int? page)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Review, Pyramid.Entity.Review>()
                .ForMember(d => d.Content,
                           o => o.ResolveUsing(r => r.Content.Length > 80 ? r.Content.Substring(0, 80) : r.Content))
                .ForMember(d => d.Product, o => o.ResolveUsing(r => r.Products));

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.ProductValues, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore());
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();


            var efReviews = _reviewRepository.GetNotApprovedReviews().ToList();

            int pageNumber = (page ?? 1);
            var modelList = new PagedList<Entity.Review>(
            efReviews.Select(u => mapper.Map<DBFirstDAL.Review, Entity.Review>(u)).AsQueryable(),
            pageNumber, Config.PageSize);
            ViewBag.Title = "Не одобренные отзывы";
            return View("Reviews", modelList);
        }
        [Authorize]
        public ActionResult Approved(int? page)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Review, Pyramid.Entity.Review>()
                .ForMember(d => d.Content,
                           o => o.ResolveUsing(r => r.Content.Length > 80 ? r.Content.Substring(0, 80) : r.Content))
                .ForMember(d => d.Product, o => o.ResolveUsing(r => r.Products));

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.ProductValues, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore());
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();


            var efReviews = _reviewRepository.GetApprovedReviews().ToList();

            int pageNumber = (page ?? 1);
            var modelList = new PagedList<Entity.Review>(
            efReviews.Select(u => mapper.Map<DBFirstDAL.Review, Entity.Review>(u)).AsQueryable(),
            pageNumber, Config.PageSize);
            ViewBag.Title = "Одобренные отзывы";
            return View("Reviews", modelList);
        }
        [Authorize]
        public ActionResult ToNotApproved(int id)
        {
            _reviewRepository.ToNotApproved(id);
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.PathAndQuery);
        }
        [Authorize]
        public ActionResult Update(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Review, Pyramid.Entity.Review>()
                .ForMember(d => d.Content,
                           o => o.ResolveUsing(r => r.Content))
                .ForMember(d => d.Product, o => o.ResolveUsing(r => r.Products));

                cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
                .ForMember(d => d.Categories, o => o.Ignore())
                .ForMember(d => d.EnumValues, o => o.Ignore())
                .ForMember(d => d.Images, o => o.Ignore())
                .ForMember(d => d.ProductValues, o => o.Ignore())
                .ForMember(d => d.ThumbnailImg, o => o.Ignore())
                .ForMember(d => d.ThumbnailId, o => o.Ignore());
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var efModel = _reviewRepository.FindBy(i => i.Id == id).SingleOrDefault();
            efModel.IsRead = true;
            _reviewRepository.AddOrUpdate(efModel);
            _reviewRepository.Save();
            var model = mapper.Map<Entity.Review>(efModel);
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult Update(Entity.Review model)
        {
            _reviewRepository.UpdateApproved(model.Id, model.IsApproved);
            return RedirectToAction("Update",new {id=model.Id });
        }
        [Authorize]
        public ActionResult Delete( int id)
        {
            var efReview = _reviewRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (efReview!=null)
            {
                _reviewRepository.Delete(efReview);
                _reviewRepository.Save();
            }
        
            return RedirectPermanent("Index");
        }
        #endregion

        #region public method
        public ActionResult AddReview(Entity.Review model)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap< Pyramid.Entity.Review, DBFirstDAL.Review>()
                .ForMember(d=>d.Products,o=>o.Ignore());

            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            model.DateCreation = DateTime.Now;
            var efMReview = mapper.Map<DBFirstDAL.Review>(model);
            try
            {
                _reviewRepository.Add(efMReview);
                _reviewRepository.Save();
            }
            catch (Exception)
            {
                return Json(new { Status = "Fall" });
            }
            return Json(new { Status = "Ok" });
        }

        public ActionResult PartialGetReviewsByProductId(int productid,int? page)
        {
            ViewBag.ProductId = productid;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Review, Pyramid.Entity.Review>()
                .ForMember(d => d.Content,
                           o => o.ResolveUsing(r => r.Content))
                .ForMember(d => d.Product, o => o.Ignore());

               
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var efReviews = _reviewRepository.GetReviewsByProductId(productid).ToList();

            int pageNumber = (page ?? 1);
            var modelList = new PagedList<Entity.Review>(
            efReviews.Select(u => mapper.Map<DBFirstDAL.Review, Entity.Review>(u)).AsQueryable(),
            pageNumber, Config.PageSize);

            return PartialView("_PartialProductReviews", modelList);
        }
        #endregion
    }
}