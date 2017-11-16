using Common;
using Entity;
using Pyramid.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Pyramid
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var admin=DBFirstDAL.UserDAL.GetByLogin("admin");
            if (admin==null)
            {
                DBFirstDAL.UserDAL.AddOrUpdate(
                    DBFirstDAL.UserDAL.EntityToDAL(
                        Pyramid.Tools.Cryptography.GetDefaultAdmin()
                        ));
            }

            DBFirstDAL.Repositories.GlobalOptionRepository globRepo = new DBFirstDAL.Repositories.GlobalOptionRepository();
           
        
            if (!globRepo.isExist(Constant.KeyShipping))
            {
                var newEntityObj = new GlobalOptionEntity()
                {
                    StringKey = Constant.KeyShipping,
                    IsHtml = true,
                    Description = "Доставка",
                    OptionContent = ""
                };
                globRepo.AddOrUpdate(newEntityObj);
            }
            if (!globRepo.isExist(Constant.KeyFootnote))
            {
                var newEntityObj = new GlobalOptionEntity()
                {
                    StringKey = Constant.KeyFootnote,
                    IsHtml = true,
                    Description = "Текст сноски когда товар доступен к заказу",
                    OptionContent = ""
                };
                globRepo.AddOrUpdate(newEntityObj);
            }
            if (!globRepo.isExist(Constant.KeyEvent))
            {
                var newEntityObj = new GlobalOptionEntity()
                {
                    StringKey = Constant.KeyEvent,
                    IsHtml = false,
                    Description = "Url родительской страницы акций",
                    OptionContent = "akcii"
                };
                globRepo.AddOrUpdate(newEntityObj);
            }
            if (!globRepo.isExist(Constant.KeyFaq))
            {
                var newEntityObj = new GlobalOptionEntity()
                {
                    StringKey = Constant.KeyFaq,
                    IsHtml = false,
                    Description = "Url родительской страницы Актуальных вопросов",
                    OptionContent = "faq"
                };
                globRepo.AddOrUpdate(newEntityObj);
            }
            if (!globRepo.isExist(Constant.KeyRecommendation))
            {
                var newEntityObj = new GlobalOptionEntity()
                {
                    StringKey = Constant.KeyRecommendation,
                    IsHtml = false,
                    Description = "Url родительской страницы советов",
                    OptionContent = "sovety"
                };
                globRepo.AddOrUpdate(newEntityObj);
            }
            //var repoRoute = new DBFirstDAL.Repositories.RouteItemRepository();
            //var _recommendRepository = new DBFirstDAL.Repositories.RecommendationRepository();
            //var recommendAll = _recommendRepository.Get(new Common.SearchClasses.SearchParamsBase() );

            //foreach (var item in recommendAll.Objects)
            //{
            //    _recommendRepository.InitSeo(item.Id);
            //}

            //foreach (var item in recommendAll.Objects)
            //{
            //    repoRoute.AddOrUpdate(new RouteItem()
            //    {
            //        ActionName = "Get",
            //        ControllerName = "Recommendation",
            //        ContentId = item.Id,
            //        Type = Common.TypeEntityFromRouteEnum.RecommendationType,

            //    });
            //}


            ////category
            //var _categoryRepository = new DBFirstDAL.Repositories.CategoryRepository();
            //var catAll = _categoryRepository.Get(new Common.SearchClasses.SearchParamsCategory() { IsShowCategoryOnSite = true });

            //foreach (var item in catAll.Objects)
            //{
            //    _categoryRepository.InitSeo(item.Id);
            //}

            //var repoProducts = new DBFirstDAL.Repositories.ProductRepository();
            //var products = repoProducts.Get(new Common.SearchClasses.SearchParamsProduct() { IsSearchOnlyPublicProduct = true });
            //foreach (var item in products.Objects)
            //{
            //    repoProducts.InitSeo(item.Id);
            //}

            //foreach (var item in catAll.Objects)
            //{
            //    try
            //    {
            //        repoRoute.AddOrUpdate(new RouteItem()
            //        {
            //            ActionName = "Index",
            //            ControllerName = "Category",
            //            ContentId = item.Id,
            //            Type = Common.TypeEntityFromRouteEnum.CategoryType,

            //        });
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //foreach (var item in products.Objects)
            //{
            //    try
            //    {
            //        repoRoute.AddOrUpdate(new RouteItem()
            //        {
            //            ActionName = "Index",
            //            ControllerName = "Product",
            //            ContentId = item.Id,
            //            Type = Common.TypeEntityFromRouteEnum.ProductType,

            //        });
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}

            //var pageRepo = new DBFirstDAL.Repositories.PageRepository();
            //var allPages = pageRepo.GetAll();
            //foreach (var item in allPages)
            //{
            //    pageRepo.InitSeo(item.Id);
            //}
            //foreach (var item in allPages)
            //{
            //    try
            //    {
            //        repoRoute.AddOrUpdate(new RouteItem()
            //        {
            //            ActionName = "Index",
            //            ControllerName = "Page",
            //            ContentId = item.Id,
            //            Type = Common.TypeEntityFromRouteEnum.PageType,

            //        });
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}

            //var faqRepo = new DBFirstDAL.Repositories.FaqRepository();
            //var allFaq = faqRepo.GetAll();
            //foreach (var item in allFaq)
            //{
            //    faqRepo.InitSeo(item.Id);
            //}
            //foreach (var item in allFaq)
            //{
            //    try
            //    {
            //        repoRoute.AddOrUpdate(new RouteItem()
            //        {
            //            ActionName = "Get",
            //            ControllerName = "Faq",
            //            ContentId = item.Id,
            //            Type = Common.TypeEntityFromRouteEnum.Faq,

            //        });
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}

            //var eventRepo = new DBFirstDAL.Repositories.EventRepository();
            //var eventAll = eventRepo.GetAll();
            //foreach (var item in eventAll)
            //{
            //    eventRepo.InitSeo(item.Id);
            //}
            //foreach (var item in eventAll)
            //{
            //    repoRoute.AddOrUpdate(new RouteItem()
            //    {
            //        ActionName = "Get",
            //        ControllerName = "Event",
            //        ContentId = item.Id,
            //        Type = Common.TypeEntityFromRouteEnum.Event,

            //    });
            //}
        }

    }
}
