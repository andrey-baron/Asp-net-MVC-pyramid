using AutoMapper;
using DBFirstDAL.Repositories;
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
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Faq, Pyramid.Entity.FAQ>()
                .ForMember(d => d.QuestionAnswer, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.QuestionAnswer, Pyramid.Entity.QuestionAnswer>()
                ;
            });


            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();

            var configFaq = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Faq, Pyramid.Entity.FAQ>()
                ;

                cfg.CreateMap<DBFirstDAL.QuestionAnswer, Pyramid.Entity.QuestionAnswer>()
                ;
            });


            configFaq.AssertConfigurationIsValid();
            var mapperFaq = configFaq.CreateMapper();
            var efFaqs = _faqRepository.GetAll().ToList();
            var faqlist = mapper.Map<IEnumerable<DBFirstDAL.Faq>, IEnumerable<Pyramid.Entity.FAQ>>(efFaqs);
            Models.Faq.FaqViewModel model = new Models.Faq.FaqViewModel();
            model.AllFaq = faqlist;
            
            if (model.CurrentFaq == null)
            {
                model.CurrentFaq = new Entity.FAQ();
            }

            return View(model);
        }
        public ActionResult Get(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Faq, Pyramid.Entity.FAQ>()
                .ForMember(d=>d.QuestionAnswer,o=>o.Ignore());

                cfg.CreateMap<DBFirstDAL.QuestionAnswer, Pyramid.Entity.QuestionAnswer>()
                ;
            });
            var configFaq = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Faq, Pyramid.Entity.FAQ>()
                ;

                cfg.CreateMap<DBFirstDAL.QuestionAnswer, Pyramid.Entity.QuestionAnswer>()
                ;
            });


            configFaq.AssertConfigurationIsValid();
            var mapperFaq = configFaq.CreateMapper();

            config.AssertConfigurationIsValid();
            var mapper = config.CreateMapper();
            var efFaqs = _faqRepository.GetAllWithQuestionAnswer().ToList();
            var faqlist=mapper.Map<IEnumerable<DBFirstDAL.Faq>, IEnumerable<Pyramid.Entity.FAQ>>(efFaqs);
            var efFaqSingle = _faqRepository.FindBy(i => i.Id == id).SingleOrDefault();
            Models.Faq.FaqViewModel model = new Models.Faq.FaqViewModel();
            model.AllFaq = faqlist;
            if (efFaqSingle!=null)
            {
                
                model.CurrentFaq = mapperFaq.Map<Entity.FAQ>(efFaqSingle);
            }
            if (model.CurrentFaq == null)
            {
                model.CurrentFaq = new Entity.FAQ();
            }
            
            return View("Index",model);
        }
        [Authorize]
        public ActionResult ManageIndex()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Faq, Pyramid.Entity.FAQ>();

                cfg.CreateMap<DBFirstDAL.QuestionAnswer, Pyramid.Entity.QuestionAnswer>()
                ;
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var efModel = _faqRepository.GetAll().ToList();

            var model = mapper.Map<IEnumerable<DBFirstDAL.Faq>, IEnumerable<Pyramid.Entity.FAQ>>(efModel);

            return View(model);
        }
        [Authorize]
        public ActionResult AddOrUpdate(int id = 0)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Faq, Pyramid.Entity.FAQ>();

                cfg.CreateMap<DBFirstDAL.QuestionAnswer, Pyramid.Entity.QuestionAnswer>()

                ;
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var efModel = _faqRepository.FindBy(i=>i.Id==id).SingleOrDefault();

            var model = mapper.Map<DBFirstDAL.Faq, Pyramid.Entity.FAQ>(efModel);
            if (model==null)
            {
                model = new Entity.FAQ();
            }
            return View(model);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddOrUpdate(Pyramid.Entity.FAQ model)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Pyramid.Entity.FAQ,DBFirstDAL.Faq>()
                 .ForMember(d => d.HomeEntity, o => o.Ignore())
                ;

                cfg.CreateMap< Pyramid.Entity.QuestionAnswer, DBFirstDAL.QuestionAnswer>()
                .ForMember(d => d.FaqId, o => o.Ignore())
                .ForMember(d => d.Faq, o => o.Ignore())
                ;
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            

            var efModel = mapper.Map<Pyramid.Entity.FAQ, DBFirstDAL.Faq>(model);
            _faqRepository.AddOrUpdate(efModel);
            _faqRepository.Save();
            return RedirectToActionPermanent("ManageIndex");
        }

        public ActionResult PartialGetAllQuestionAnswer(int id)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<DBFirstDAL.Faq, Pyramid.Entity.FAQ>();

                cfg.CreateMap<DBFirstDAL.QuestionAnswer, Pyramid.Entity.QuestionAnswer>()
                ;
            });


            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();

            var efModel = _faqRepository.FindBy(i => i.Id == id).SingleOrDefault();

            var model = mapper.Map<DBFirstDAL.Faq, Pyramid.Entity.FAQ>(efModel);
            if (model==null)
            {
                model = new Entity.FAQ();
            }
            return PartialView("_PartialAllQuestionAnswer", model.QuestionAnswer);
        }

        public ActionResult AddNewDefault(int id, int count)
        {
            var newIndex = 0;
            var effaq = _faqRepository.FindBy(i => i.Id == id).SingleOrDefault();
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
            var efmodel = _questionAnswerRepository.FindBy(i => i.Id == id).SingleOrDefault();
            if (efmodel!=null)
            {
                _questionAnswerRepository.Delete(efmodel);
                _questionAnswerRepository.Save();
            }
            return null;
        }
    }
}