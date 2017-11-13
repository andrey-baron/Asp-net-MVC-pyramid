using Common.SearchClasses;
using DBFirstDAL.Intercaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pyramid.Entity;
using System.Data.Entity;
using System.Linq.Expressions;

namespace DBFirstDAL.Repositories
{
    public class PageRepository : GenericRepository<Pages, PyramidFinalContext, Pyramid.Entity.Page, SearchParamsBase, int>
    {
        public PageRepository(PyramidFinalContext context):base(context) { }
        public PageRepository() { }
        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Pages dbEntity, Page entity, bool exists)
        {
            dbEntity.Content = entity.Content;
            dbEntity.Title = entity.Title;

        }

        public override void UpdateAfterSaving(PyramidFinalContext dbContext, Pages dbEntity, Page entity, bool exists)
        {
            if (entity.Seo != null)
            {
                if (dbEntity.Seo == null)
                {
                    dbEntity.Seo = new Seo();
                }
                dbEntity.Seo.Alias = entity.Seo.Alias;
                dbEntity.Seo.MetaTitle = entity.Seo.MetaTitle;
                dbEntity.Seo.MetaKeywords = entity.Seo.MetaKeywords;
                dbEntity.Seo.MetaDescription = entity.Seo.MetaDescription;

            }
        }

       
        protected override IQueryable<Pages> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Pages> dbObjects, SearchParamsBase searchParams)
        {
            throw new NotImplementedException();
        }

        public override Page ConvertDbObjectToEntity(PyramidFinalContext context, Pages dbObject)
        {
            
            var seo = new SeoRepository(context).Get(dbObject.SeoId.HasValue?dbObject.SeoId.Value:0);
            var friendly = new RouteItemRepository(context).GetFriendlyUrl(dbObject.Id, Common.TypeEntityFromRouteEnum.PageType);
            var page = new Page() {
                Content = dbObject.Content,
                Title = dbObject.Title,
                Id = dbObject.Id,
                SeoId = dbObject.SeoId,
                Seo = seo
            };
            page.FriendlyUrl = friendly;
            return page;
        }

        protected override Pages GetDbObjectByEntity(DbSet<Pages> objects, Page entity)
        {
            return objects.FirstOrDefault(f => f.Id == entity.Id);
        }

        protected override Expression<Func<Pages, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }
        public override bool Delete(int id)
        {
            var data = _entities ?? new PyramidFinalContext();
            try
            {
                var objects = data.Set<Pages>();
                var dbObject = GetDbObjectById(objects, id);
                if (dbObject == null)
                    return false;
                var seo = data.Seo.Find(dbObject.SeoId);
                if (seo != null)
                {
                    data.Seo.Remove(seo);
                }
                objects.Remove(dbObject);
                data.SaveChanges();
                return true;
            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
        }

        public void InitSeo(int pageId)
        {
            using (PyramidFinalContext context = new PyramidFinalContext())
            {
                var pageDb = context.Pages.Find(pageId);
                if (pageDb != null)
                {
                    if (pageDb.Seo == null)
                    {
                        pageDb.Seo = new Seo();

                    }
                    pageDb.Seo.Alias = Tools.Transliteration.Translit(pageDb.Title);
                    pageDb.Seo.MetaTitle = pageDb.Title;
                    context.SaveChanges();

                }
            }

        }
    }
}
