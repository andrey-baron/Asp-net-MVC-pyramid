using AutoMapper;
using DBFirstDAL.Repositories;
using Pyramid.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pyramid.Controllers
{
    public abstract class BaseController : Controller
    {
        CategoryRepository _categoryRepository;
        public BaseController() {
            _categoryRepository = new CategoryRepository();
        }
        public string HostName { get; protected set; }

        protected static string RenderViewToString(ControllerContext context,
                                 string viewPath,
                                 object model = null,
                                 bool partial = false)
        {
            // first find the ViewEngine for this view
            ViewEngineResult viewEngineResult = null;
            if (partial)
                viewEngineResult = ViewEngines.Engines.FindPartialView(context, viewPath);
            else
                viewEngineResult = ViewEngines.Engines.FindView(context, viewPath, null);

            if (viewEngineResult == null)
                throw new FileNotFoundException("Представление не найдено.");

            // get the view and attach the model to view data
            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view,
                                            context.Controller.ViewData,
                                            context.Controller.TempData,
                                            sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }

            return result;
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            if (requestContext.HttpContext.Request.Url != null)
            {
                HostName = requestContext.HttpContext.Request.Url.Authority;
            }
            if (requestContext.RouteData.Values["controller"] != null)
            {
                ViewBag.Path += requestContext.RouteData.Values["controller"] + "/";
                ViewBag.Controller = requestContext.RouteData.Values["controller"];
            }
            if (requestContext.RouteData.Values["action"] != null)
            {
                ViewBag.Path += requestContext.RouteData.Values["action"].ToString();
                ViewBag.Action = requestContext.RouteData.Values["action"];
            }

            var configForFooterCategories = new MapperConfiguration(cfg =>
            {
                #region root config

                cfg.CreateMap<DBFirstDAL.DataModels.CategoryWithThumbnail, Pyramid.Entity.Category>()
               .ForMember(d => d.Checked, o => o.Ignore())
               .ForMember(d => d.Filters, o => o.Ignore())
               .ForMember(d => d.ParentId, o => o.Ignore())
               .ForMember(d => d.FlagRoot, o => o.Ignore())
               .ForMember(d => d.Products, o => o.Ignore())
                 .ForMember(d => d.OneCId, o => o.Ignore());

                cfg.CreateMap<DBFirstDAL.DataModels.RootCategory, Models.AllCategoriesViewModel>()
                ;

                cfg.CreateMap<DBFirstDAL.Images, Entity.Image>();
                #endregion
            });
            configForFooterCategories.AssertConfigurationIsValid();
            var mapperForFooter = configForFooterCategories.CreateMapper();

            var rootCategories = _categoryRepository.GetRootCategoriesWithSubs();

            var modelRootCategories =
                mapperForFooter.Map<IEnumerable<DBFirstDAL.DataModels.RootCategory>, IEnumerable<Models.AllCategoriesViewModel>>(rootCategories);

            ViewBag.FooterCategories = modelRootCategories;
            base.Initialize(requestContext);
            Cart cart = GetCart();
            ViewBag.Cart = cart;
          

        }

        public Cart GetCart()
        {
            Cart cart = (Cart)HttpContext.Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

    }
}