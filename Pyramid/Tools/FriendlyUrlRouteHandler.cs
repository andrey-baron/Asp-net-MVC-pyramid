using DBFirstDAL.Repositories;
using Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Pyramid.Tools
{
    public class FriendlyUrlRouteHandler : MvcRouteHandler
    {
        private static readonly Regex TypicalLink = new Regex("^.+/.+(/.*)?");

        private RouteItemRepository _routeItemRepository;

        public FriendlyUrlRouteHandler()
        {
            _routeItemRepository = new RouteItemRepository();
        }
        protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
        {

            // Path для www.site.com/helloworld?id=1 будет равняться /helloworld
            // поэтому мы убираем начальный слэш
            var url = requestContext.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(url))
            {
                RouteItem page = _routeItemRepository.Get(url);
                if (page != null)
                {
                    FillRequest(page.ControllerName,
                        page.ActionName ?? "Index",
                        page.ContentId.ToString(),
                        requestContext);
                }
            }

            return base.GetHttpHandler(requestContext);
        }

        /// <summary> Заполнение request-контекста данными о контроллере, экшне и параметрах </summary>
        private static void FillRequest(string controller, string action, string id, RequestContext requestContext)
        {
            if (requestContext == null)
            {
                throw new ArgumentNullException("requestContext");
            }

            requestContext.RouteData.Values["controller"] = controller;
            requestContext.RouteData.Values["action"] = action;
            requestContext.RouteData.Values["id"] = id;
        }
    }
}