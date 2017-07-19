using AutoMapper;
using Common.SearchClasses;
using DBFirstDAL.Repositories;
using PagedList;
using Pyramid.Entity;
using Pyramid.Global;
using Pyramid.Models.CommonViewModels;
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
        public ActionResult AdminIndex(string productTitle, string currentFilter, bool? isApproved,bool? isNotRead, int? page)
        {
            var pageNumber = page ?? 1;
            if (productTitle != null)
            {
                page = 1;
            }
            else
            {
                productTitle = currentFilter;
            }
            var objectsPerPage = 20;
            var searchResult = _reviewRepository.Get(new SearchParamsReview(productTitle, isApproved, isNotRead, (pageNumber - 1) * objectsPerPage, objectsPerPage));
            ViewBag.CurrentFilter = productTitle;
            ViewBag.IsApproved = isApproved;
            ViewBag.IsRead = isNotRead;

            var viewModel = SearchResultViewModel<Review>.CreateFromSearchResult(searchResult, i => i, 10);

            return View(viewModel);
        }
        //[Authorize]
        //public ActionResult NewReviews( int? page)
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<DBFirstDAL.Review, Pyramid.Entity.Review>()
        //        .ForMember(d=>d.Content,
        //                   o=>o.ResolveUsing(r=>r.Content.Length>80? r.Content.Substring(0, 80): r.Content))
        //        .ForMember(d => d.Product, o => o.ResolveUsing(r => r.Products));

        //        cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
        //        .ForMember(d => d.Categories, o => o.Ignore())
        //        .ForMember(d => d.EnumValues, o => o.Ignore())
        //        .ForMember(d => d.Images, o => o.Ignore())
        //        .ForMember(d => d.ProductValues, o => o.Ignore())
        //        .ForMember(d => d.ThumbnailImg, o => o.Ignore())
        //        .ForMember(d => d.ThumbnailId, o => o.Ignore());
        //    });
        //    config.AssertConfigurationIsValid();

        //    var mapper = config.CreateMapper();

           
        //    var efReviews = _reviewRepository.GetNewReviews().ToList();

        //    int pageNumber = (page ?? 1);
        //    var modelList = new PagedList<Entity.Review>(
        //    efReviews.Select(u => mapper.Map<DBFirstDAL.Review, Entity.Review>(u)).AsQueryable(),
        //    pageNumber, Config.PageSize);
        //    ViewBag.Title = "Новые отзывы(не прочитанные)";
        //    return View("Reviews", modelList);
        //}
        //[Authorize]
        //public ActionResult NotApproved(int? page)
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<DBFirstDAL.Review, Pyramid.Entity.Review>()
        //        .ForMember(d => d.Content,
        //                   o => o.ResolveUsing(r => r.Content.Length > 80 ? r.Content.Substring(0, 80) : r.Content))
        //        .ForMember(d => d.Product, o => o.ResolveUsing(r => r.Products));

        //        cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
        //        .ForMember(d => d.Categories, o => o.Ignore())
        //        .ForMember(d => d.EnumValues, o => o.Ignore())
        //        .ForMember(d => d.Images, o => o.Ignore())
        //        .ForMember(d => d.ProductValues, o => o.Ignore())
        //        .ForMember(d => d.ThumbnailImg, o => o.Ignore())
        //        .ForMember(d => d.ThumbnailId, o => o.Ignore());
        //    });
        //    config.AssertConfigurationIsValid();

        //    var mapper = config.CreateMapper();


        //    var efReviews = _reviewRepository.GetNotApprovedReviews().ToList();

        //    int pageNumber = (page ?? 1);
        //    var modelList = new PagedList<Entity.Review>(
        //    efReviews.Select(u => mapper.Map<DBFirstDAL.Review, Entity.Review>(u)).AsQueryable(),
        //    pageNumber, Config.PageSize);
        //    ViewBag.Title = "Не одобренные отзывы";
        //    return View("Reviews", modelList);
        //}
        //[Authorize]
        //public ActionResult Approved(int? page)
        //{
        //    var config = new MapperConfiguration(cfg =>
        //    {
        //        cfg.CreateMap<DBFirstDAL.Review, Pyramid.Entity.Review>()
        //        .ForMember(d => d.Content,
        //                   o => o.ResolveUsing(r => r.Content.Length > 80 ? r.Content.Substring(0, 80) : r.Content))
        //        .ForMember(d => d.Product, o => o.ResolveUsing(r => r.Products));

        //        cfg.CreateMap<DBFirstDAL.Products, Pyramid.Entity.Product>()
        //        .ForMember(d => d.Categories, o => o.Ignore())
        //        .ForMember(d => d.EnumValues, o => o.Ignore())
        //        .ForMember(d => d.Images, o => o.Ignore())
        //        .ForMember(d => d.ProductValues, o => o.Ignore())
        //        .ForMember(d => d.ThumbnailImg, o => o.Ignore())
        //        .ForMember(d => d.ThumbnailId, o => o.Ignore());
        //    });
        //    config.AssertConfigurationIsValid();

        //    var mapper = config.CreateMapper();


        //    var efReviews = _reviewRepository.GetApprovedReviews().ToList();

        //    int pageNumber = (page ?? 1);
        //    var modelList = new PagedList<Entity.Review>(
        //    efReviews.Select(u => mapper.Map<DBFirstDAL.Review, Entity.Review>(u)).AsQueryable(),
        //    pageNumber, Config.PageSize);
        //    ViewBag.Title = "Одобренные отзывы";
        //    return View("Reviews", modelList);
        //}
        [Authorize]
        public ActionResult ToNotApproved(int id)
        {
            _reviewRepository.ToNotApproved(id);
            return Redirect(ControllerContext.HttpContext.Request.UrlReferrer.PathAndQuery);
        }
        [Authorize]
        public ActionResult Update(int id)
        {
            var model = _reviewRepository.Get(id);
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
                _reviewRepository.Delete(efReview.Id);
                //_reviewRepository.Save();
            }
        
            return RedirectPermanent("Index");
        }
        #endregion

        #region public method
        public ActionResult AddReview(Entity.Review model)
        {


            model.DateCreation = DateTime.Now;
           
            try
            {
                _reviewRepository.AddOrUpdate(model);
              
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
            var pageNumber = page ?? 1;
            var objectsPerPage = 20;
            var searchResult = _reviewRepository.Get(new SearchParamsReview(productid, (pageNumber - 1) * objectsPerPage, objectsPerPage));
            ViewBag.ProductId = productid;

            var viewModel = SearchResultViewModel<Review>.CreateFromSearchResult(searchResult, i => i, 10);

            return PartialView("_PartialProductReviews", viewModel);
        }
        #endregion
    }
}