using AutoMapper;
using DBFirstDAL.Repositories;
using Pyramid.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Pyramid.Tools.Mailer;

namespace Pyramid.Controllers
{
    [Authorize]
    public class FeedBackController : BaseController
    {
        FeedBackEmailRepository feedBackEmailRepository;
        public FeedBackController()
        {
            feedBackEmailRepository = new FeedBackEmailRepository();
        }
        // GET: FeedBack
        [AllowAnonymous]
        public ActionResult Send(Common.FeedBack feedback)
        {
            ValidateModel(feedback);
            var message = RenderViewToString(this.ControllerContext, "/Views/FeedBack/_PartialEmailMessage.cshtml", feedback,true);

            MailerMessage mailerMessage = new MailerMessage();
            mailerMessage.Body = message;
            mailerMessage.Subject = "Заявка с сайта Пирамида строй";
            mailerMessage.To = feedBackEmailRepository.GetAll().Select(i => i.Email).ToList();
            mailerMessage.SenderName = "Pyramid";
            if (mailerMessage.To.Count>0)
            {
                Mailer.Send(mailerMessage);
            }
            return Json(new { isSend = true }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {
            Mapper.Initialize(cfg => cfg.CreateMap<DBFirstDAL.FeedBackEmails, Entity.FeedBack>());
            var model =
                Mapper.Map<IEnumerable<DBFirstDAL.FeedBackEmails>, List<Entity.FeedBack>>(feedBackEmailRepository.GetAll().ToList());
            return View(model);
        }

        public ActionResult AddOrUpdate(int id=0)
        {
            Mapper.Initialize(cfg => cfg.CreateMap<DBFirstDAL.FeedBackEmails, Entity.FeedBack>());
            var model =
                Mapper.Map<DBFirstDAL.FeedBackEmails, Entity.FeedBack>(feedBackEmailRepository.FindBy(i => i.Id == id).SingleOrDefault());
            if (model == null)
            {
                model = new Entity.FeedBack();
            }
            ViewBag.Title = "Редактирование Email";

            return View(model);
        }
        [HttpPost]
        public ActionResult AddOrUpdate(Entity.FeedBack model)
        {
            Mapper.Initialize(cfg => cfg.CreateMap< Entity.FeedBack, DBFirstDAL.FeedBackEmails>());
            var efmodel =
                Mapper.Map< Entity.FeedBack, DBFirstDAL.FeedBackEmails>(model);

            //feedBackEmailRepository.AddOrUpdate(efmodel);
            //feedBackEmailRepository.Save();
            return RedirectToAction("Index");
        }
    }
}