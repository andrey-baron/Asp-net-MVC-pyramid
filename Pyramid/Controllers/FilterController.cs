using AutoMapper;
using Common.SearchClasses;
using DBFirstDAL.Repositories;
using Pyramid.Models.CommonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
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
        public ActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            var objectsPerPage = 20;
            var startIndex = (pageNumber - 1) * objectsPerPage;
            SearchParamsBase SearchParams = new SearchParamsBase( startIndex, objectsPerPage);
            var searchResult = _filterRepository.Get(SearchParams);
            var viewModel = SearchResultViewModel<Pyramid.Entity.Filter>.CreateFromSearchResult(searchResult, i => i, 10);
            return View(viewModel);
        }
        [Authorize]
        public ActionResult AddOrUpdate(int id=0)
        {
            var filter = _filterRepository.Get(id);
            if (filter == null)
            {
                filter = new Entity.Filter();
            }
            ViewBag.EnumValuesSelectList= _enumRepositopy.GetAll().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            }); 
            return View(filter);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddOrUpdate(Pyramid.Entity.Filter model)
        {
            _filterRepository.AddOrUpdate(model);
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
        public ActionResult GetTemplateEnumValue(int filterid,int count)
        {
            ViewBag.EnumValuesSelectList= _enumRepositopy.GetAll().Select(item => new SelectListItem
            {
                Text = item.Key,
                Value = item.Id.ToString()
            });
            var model = _filterRepository.GetAllEnumValues(filterid).Count();
            if (count>model)
            {
                model = count;
            }
            return PartialView("_PartialFilterEmptyEnumValue", model);
        }

        public ActionResult DeleteEnumValue(int id,int enumValueId)
        {
            _filterRepository.DeleteEnumValue(id, enumValueId);
            return null;
        }
        public ActionResult Delete(int id)
        {
            _filterRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}