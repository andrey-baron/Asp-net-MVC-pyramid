using Common.SearchClasses;
using DBFirstDAL.Repositories;
using Pyramid.Entity;
using Pyramid.Models.CommonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
    public class RecommendationController : Controller
    {
        RecommendationRepository _recommendationRepository;
        public RecommendationController() {
            _recommendationRepository = new RecommendationRepository();
        }
        // GET: Recommendation
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var objectsPerPage = 20;
            var searchResult = _recommendationRepository.Get(new SearchParamsBase
            {
                StartIndex = (pageNumber - 1) * objectsPerPage,
                ObjectsCount = objectsPerPage,
            });

            var viewModel = SearchResultViewModel<Recommendation>.CreateFromSearchResult(searchResult, i => i, 10);

            return View(viewModel);

            //var model = _recommendationRepository.GetAll();
            //return View(model);
        }
        public ActionResult AddOrUpdate(int id = 0)
        {
            var model = _recommendationRepository.Get(id);
            if (model == null)
            {
                model = new Entity.Recommendation();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddOrUpdate(Entity.Recommendation model)
        {
            _recommendationRepository.AddOrUpdate(model);
            return RedirectToAction("Index");

        }
    }
}