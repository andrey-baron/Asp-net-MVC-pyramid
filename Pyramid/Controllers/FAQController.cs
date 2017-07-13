using AutoMapper;
using Common.Models;
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
    public class FAQController : BaseController
    {
        FaqRepository _faqRepository;
        QuestionAnswerRepository _questionAnswerRepository;
        public FAQController()
        {
            _faqRepository = new FaqRepository();
            _questionAnswerRepository = new QuestionAnswerRepository();
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
          
          
            var efFaqs = _faqRepository.GetAllWithQuestionAnswer().ToList();
            var faqlist = _faqRepository.GetAll();
            var faqSingle = faqlist.FirstOrDefault(i => i.Id == id);
            Models.Faq.FaqViewModel model = new Models.Faq.FaqViewModel();
            model.AllFaq = faqlist;
            if (faqSingle!=null)
            {
                model.CurrentFaq = faqSingle;
            }
            if (model.CurrentFaq == null)
            {
                model.CurrentFaq = new Entity.FAQ();
            }
            List<BreadCrumbViewModel> breadcrumbs = new List<BreadCrumbViewModel>();

            breadcrumbs.Add(new BreadCrumbViewModel()
            {
                Title = "Актуальные вопросы"
            });
            breadcrumbs.Add(new BreadCrumbViewModel()
            {
                Title = model.CurrentFaq.Title
            });
            ViewBag.BredCrumbs = breadcrumbs;

            ViewBag.MetaTitle = model.CurrentFaq.Title;
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
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddOrUpdate(Pyramid.Entity.FAQ model)
        {
            _faqRepository.AddOrUpdate(model);
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
            //var efmodel = _questionAnswerRepository.FindBy(i => i.Id == id).SingleOrDefault();
            var model = _faqRepository.Get(id);
            if (model != null)
            {
                _questionAnswerRepository.Delete(model.Id);
               // _questionAnswerRepository.Save();
            }
            return null;
        }
    }
}