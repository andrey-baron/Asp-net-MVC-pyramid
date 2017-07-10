using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Pyramid.Helpers
{
    public static class PagedListHelper
    {
        public static MvcHtmlString PagedList(this AjaxHelper helper, int itemsCount, int itemsPerPage, int currentPage,
            int displayedPages, string actionName, string controllerName, Func<int, RouteValueDictionary> routeValuesGenFunction,
            AjaxOptions ajaxOptions, object pagerAttributes = null, string activeLinkClass = null, bool appendPrevNextLinks = true,
            bool appendFirstLastLinks = true, string pageNumberAttributeName = "data-page")
        {
            if (itemsCount == 0)
                return new MvcHtmlString("");
            if (pagerAttributes == null)
                pagerAttributes = new { @class = "pager" };
            if (activeLinkClass == null)
                activeLinkClass = "active";
            int pagesCount = (itemsCount + itemsPerPage - 1) / itemsPerPage;
            int firstPage = Math.Max(currentPage - displayedPages / 2, 1);
            int lastPage = Math.Min(firstPage + displayedPages - 1, pagesCount);
            currentPage = Math.Max(currentPage, 1);
            currentPage = Math.Min(lastPage, currentPage);
            var pages = new List<string>();
            MvcHtmlString link = null;
            if (currentPage > 2 && appendFirstLastLinks)
            {
                link = helper.ActionLink("‹‹", actionName, controllerName, routeValuesGenFunction(1), ajaxOptions,
                    new Dictionary<string, object> { { pageNumberAttributeName, 1 } });
                pages.Add(link + " ");
            }
            if (currentPage != firstPage && appendPrevNextLinks)
            {
                link = helper.ActionLink("‹", actionName, controllerName, routeValuesGenFunction(currentPage - 1), ajaxOptions,
                    new Dictionary<string, object> { { pageNumberAttributeName, currentPage - 1 } });
                pages.Add(link + " ");
            }
            for (int page = firstPage; page <= lastPage; ++page)
            {
                var attrDictionary = new Dictionary<string, object>();
                if (page == currentPage)
                    attrDictionary["class"] = activeLinkClass;
                attrDictionary[pageNumberAttributeName] = page;
                link = helper.ActionLink(page.ToString(), actionName, controllerName, routeValuesGenFunction(page), ajaxOptions, attrDictionary);
                pages.Add(link + " ");
            }
            if (currentPage != lastPage && appendPrevNextLinks)
            {
                link = helper.ActionLink("›", actionName, controllerName, routeValuesGenFunction(currentPage + 1), ajaxOptions,
                    new Dictionary<string, object> { { pageNumberAttributeName, currentPage + 1 } });
                pages.Add(link + " ");
            }
            if (currentPage < pagesCount - 1 && appendFirstLastLinks)
            {
                link = helper.ActionLink("››", actionName, controllerName, routeValuesGenFunction(pagesCount), ajaxOptions,
                    new Dictionary<string, object> { { pageNumberAttributeName, pagesCount } });
                pages.Add(link + " ");
            }

            var builder = new TagBuilder("div");
            builder.MergeAttributes(new RouteValueDictionary(pagerAttributes));
            builder.InnerHtml = string.Concat(pages);
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString PagedList(this HtmlHelper helper, int itemsCount, int itemsPerPage, int currentPage,
            int displayedPages, string actionName, string controllerName, Func<int, RouteValueDictionary> routeValuesGenFunction,
            object pagerAttributes = null, string activeLinkClass = null, bool appendPrevNextLinks = true,
            bool appendFirstLastLinks = true, string pageNumberAttributeName = "data-page")
        {
            if (itemsCount == 0)
                return new MvcHtmlString("");
            if (pagerAttributes == null)
                pagerAttributes = new { @class = "pager" };
            if (activeLinkClass == null)
                activeLinkClass = "active";
            int pagesCount = (itemsCount + itemsPerPage - 1) / itemsPerPage;
            int firstPage = Math.Max(currentPage - displayedPages / 2, 1);
            int lastPage = Math.Min(firstPage + displayedPages - 1, pagesCount);
            currentPage = Math.Max(currentPage, 1);
            currentPage = Math.Min(lastPage, currentPage);
            var pages = new List<string>();
            MvcHtmlString link = null;
            if (currentPage > 2 && appendFirstLastLinks)
            {
                link = helper.ActionLink("‹‹", actionName, controllerName, routeValuesGenFunction(1),
                    new Dictionary<string, object> { { pageNumberAttributeName, 1 } });
                pages.Add(link + " ");
            }
            if (currentPage != firstPage && appendPrevNextLinks)
            {
                link = helper.ActionLink("‹", actionName, controllerName, routeValuesGenFunction(currentPage - 1),
                    new Dictionary<string, object> { { pageNumberAttributeName, currentPage - 1 } });
                pages.Add(link + " ");
            }
            for (int page = firstPage; page <= lastPage; ++page)
            {
                var attrDictionary = new Dictionary<string, object>();
                if (page == currentPage)
                    attrDictionary["class"] = activeLinkClass;
                attrDictionary[pageNumberAttributeName] = page;
                link = helper.ActionLink(page.ToString(), actionName, controllerName, routeValuesGenFunction(page), attrDictionary);
                pages.Add(link + " ");
            }
            if (currentPage != lastPage && appendPrevNextLinks)
            {
                link = helper.ActionLink("›", actionName, controllerName, routeValuesGenFunction(currentPage + 1),
                    new Dictionary<string, object> { { pageNumberAttributeName, currentPage + 1 } });
                pages.Add(link + " ");
            }
            if (currentPage < pagesCount - 1 && appendFirstLastLinks)
            {
                link = helper.ActionLink("››", actionName, controllerName, routeValuesGenFunction(pagesCount),
                    new Dictionary<string, object> { { pageNumberAttributeName, pagesCount } });
                pages.Add(link + " ");
            }
            var builder = new TagBuilder("div");
            builder.MergeAttributes(new RouteValueDictionary(pagerAttributes));
            builder.InnerHtml = string.Concat(pages);
            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString PagedList(this AjaxHelper helper, int itemsCount, int itemsPerPage, int currentPage,
            int displayedPages, string actionName, string controllerName, Func<int, object> routeValuesGenFunction,
            AjaxOptions ajaxOptions, object pagerAttributes = null, string activeLinkClass = null)
        {
            return PagedList(helper, itemsCount, itemsPerPage, currentPage, displayedPages, actionName, controllerName,
                page => new RouteValueDictionary(routeValuesGenFunction(page)), ajaxOptions, pagerAttributes, activeLinkClass);
        }

        public static MvcHtmlString PagedList(this HtmlHelper helper, int itemsCount, int itemsPerPage, int currentPage,
            int displayedPages, string actionName, string controllerName, Func<int, object> routeValuesGenFunction,
            object pagerAttributes = null, string activeLinkClass = null)
        {
            return PagedList(helper, itemsCount, itemsPerPage, currentPage, displayedPages, actionName, controllerName,
                page => new RouteValueDictionary(routeValuesGenFunction(page)), pagerAttributes, activeLinkClass);
        }
    }
}