using AutoMapper;
using Common.SearchClasses;
using DBFirstDAL.Repositories;
using PagedList;
using Pyramid.Global;
using Pyramid.Models.CommonViewModels;
using Pyramid.Models.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private OrderRepository _orederRepository;

        public OrderController()
        {
            _orederRepository = new OrderRepository();
        }
        public ActionResult Index(int? page)
        {
          var pageNumber = page ?? 1;

            var objectsPerPage = Config.PageSize;
            var startIndex = (pageNumber - 1) * objectsPerPage;


            var searchResult = _orederRepository.Get(new SearchParamsOrder(startIndex,objectsPerPage) {SortByDate =true });

            var viewModel = SearchResultViewModel<Pyramid.Entity.Order>.CreateFromSearchResult(searchResult, i => i, 10);

            return View(viewModel);
        }
        public ActionResult Update(int id)
        {
          
            var order = _orederRepository.Get(id);
            return View(order);
        }
        [HttpPost]
        public ActionResult Update(OrderModel order)
        {
            _orederRepository.UpdateType(order.Id, (int)order.TypeProgressOrder);
            return RedirectToAction("Index");
        }
        public ActionResult Delete( int id)
        {
            _orederRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}