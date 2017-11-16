using AutoMapper;
using Common.Models;
using Common.SearchClasses;
using DBFirstDAL.Repositories;
using Entity;
using Pyramid.Models.CommonViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public class FAQController : BaseController
    {
        FaqRepository _faqRepository;
        QuestionAnswerRepository _questionAnswerRepository;
        RouteItemRepository _routeItemRepository;
        public FAQController()
        {
            _faqRepository = new FaqRepository();
            _questionAnswerRepository = new QuestionAnswerRepository();
            _routeItemRepository = new RouteItemRepository();
        }
        // GET: FAQ
        public ActionResult Index()
        {
           List<BreadCrumbViewModel> breadcrumbs = new List<BreadCrumbViewModel>();
            breadcrumbs.Add(new BreadCrumbViewModel()
            {
                Title = "Актуальные вопросы"
            });
            ViewBag.BredCrumbs = breadcrumbs;


            var faqlist = _faqRepository.GetAll();
          
            Models.Faq.FaqViewModel model = new Models.Faq.FaqViewModel();
            model.AllFaq = faqlist;
           
            if (model.CurrentFaq == null)
            {
                model.CurrentFaq = new Entity.FAQ();
            }

            ViewBag.MetaTitle = "Актуальные вопросы";
            return View(model);
        }
        public ActionResult Get(int id)
        {
            //var efFaqs = _faqRepository.GetAllWithQuestionAnswer().ToList();
            var faqlist = _faqRepository.GetAll();
            var faqSingle = _faqRepository.Get(id);
            Models.Faq.FaqViewModel model = new Models.Faq.FaqViewModel();
            model.AllFaq = faqlist;
            if (faqSingle!=null)
            {
                model.CurrentFaq = faqSingle;
            }
            else
            {
                model.CurrentFaq = new Entity.FAQ();
            }
            List<BreadCrumbViewModel> breadcrumbs = new List<BreadCrumbViewModel>();

            breadcrumbs.Add(new BreadCrumbViewModel()
            {
                Link="/faq",
                FriendlyUrl= string.Format("/{0}", new GlobalOptionRepository().Get(Common.Constant.KeyFaq).OptionContent),
                Title = "Актуальные вопросы"
            });
            breadcrumbs.Add(new BreadCrumbViewModel()
            {
                Title = model.CurrentFaq.Title
            });
            ViewBag.BredCrumbs = breadcrumbs;

            ViewBag.MetaTitle = model.CurrentFaq.Title;
            ViewBag.MetaTitle = model.CurrentFaq.Seo != null ? model.CurrentFaq.Seo.MetaTitle : model.CurrentFaq.Title;
            ViewBag.Keywords = model.CurrentFaq.Seo != null ? model.CurrentFaq.Seo.MetaKeywords : null;
            ViewBag.Description = model.CurrentFaq.Seo != null ? model.CurrentFaq.Seo.MetaDescription : null;
            return View("Index",model);
        }
        [Authorize]
        public ActionResult ManageIndex(int? page)
        {
            var pageNumber = page ?? 1;

            var objectsPerPage = 20;
            var startIndex = (pageNumber - 1) * objectsPerPage;

            SearchParamsBase SearchParams = new SearchParamsBase(startIndex, objectsPerPage);

            var searchResult = _faqRepository.Get(SearchParams);

            var viewModel = SearchResultViewModel<Pyramid.Entity.FAQ>.CreateFromSearchResult(searchResult, i => i, 10);

            return View(viewModel);
        }
        [Authorize]
        public ActionResult AddOrUpdate(int id = 0)
        {
            var model=_faqRepository.Get(id);
            if (model== null)
            {
                model = new Entity.FAQ();
            }
            var friendlyUrl = _routeItemRepository.GetFriendlyUrl(model.Id,Common.TypeEntityFromRouteEnum.Faq);
            ViewBag.CurrentFriendlyUrl = friendlyUrl;
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddOrUpdate(Pyramid.Entity.FAQ model)
        {
            _faqRepository.AddOrUpdate(model);
            var routeItem = new RouteItem(0, null, (string)ControllerContext.RequestContext.RouteData.Values["controller"],
              "Get",
              model.Id)
            { Type = Common.TypeEntityFromRouteEnum.Faq };

            _routeItemRepository.AddOrUpdate(routeItem);
            return RedirectToActionPermanent("ManageIndex");
        }

        public ActionResult PartialGetAllQuestionAnswer(int id)
        {
            var model = _faqRepository.Get(id);
            if (model==null)
            {
                model = new Entity.FAQ();
            }
            return PartialView("_PartialAllQuestionAnswer", model.QuestionAnswer);
        }

        public ActionResult AddNewDefault(int id, int count)
        {
            var newIndex = 0;
            var effaq = _faqRepository.Get(id);
            if (effaq != null)
            {
                newIndex = effaq.QuestionAnswer.Count;
            }
            if (count > newIndex)
            {
                newIndex = count;
            }
           // _faqRepository.AddEmptyQuestionAnswer(id);
            return PartialView("_PartialTemplateQuestionAnswer", newIndex);

        }

        public ActionResult DeleteQuestionAnswer(int id)
        {
            var model = _faqRepository.Get(id);
            if (model != null)
            {
                _questionAnswerRepository.Delete(model.Id);
            }
            return null;
        }
    }
}