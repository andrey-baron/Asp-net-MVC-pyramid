using Common.SearchClasses;
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
    public class ImageRepository : GenericRepository<Images, PyramidFinalContext, Pyramid.Entity.Image, SearchParamsImage, int>
    {
        public ImageRepository(PyramidFinalContext context) : base(context) { }
        public ImageRepository() { }
        public override Image ConvertDbObjectToEntity(PyramidFinalContext context, Images dbObject)
        {
            return new Image(dbObject.Id,dbObject.PathInFileSystem,dbObject.ServerPathImg,dbObject.ImgAlt,dbObject.Title);
        }

        public override void UpdateBeforeSaving(PyramidFinalContext dbContext, Images dbEntity, Image entity, bool exists)
        {
            dbEntity.Id = entity.Id;
            dbEntity.ImgAlt = entity.ImgAlt;
            dbEntity.Title = entity.Title;
            dbEntity.ServerPathImg = entity.ServerPathImg;
            dbEntity.PathInFileSystem = entity.PathInFileSystem;
        }
        protected override Image ConvertDbObjectToEntityShort(PyramidFinalContext context, Images dbObject)
        {
            return base.ConvertDbObjectToEntityShort(context, dbObject);
        }

        protected override IQueryable<Images> BuildDbObjectsList(PyramidFinalContext context, IQueryable<Images> dbObjects, SearchParamsImage searchParams)
        {
            if (searchParams.ProductId.HasValue && searchParams.TypeImage.HasValue)
            {
                dbObjects = dbObjects.Where(w => w.ProductImages.Any(a => a.TypeImage == searchParams.TypeImage.Value && a.ProductId == searchParams.ProductId.Value));
            }
            return dbObjects = dbObjects.OrderByDescending(o => o.Id);
        }

        protected override Images GetDbObjectByEntity(DbSet<Images> objects, Image entity)
        {
            return objects.Find(entity.Id);
        }

        protected override Expression<Func<Images, int>> GetIdByDbObjectExpression()
        {
            return i => i.Id;
        }

        public  Image Get(int ProductId,int TypeImage)
        {
            var data = _entities ?? new PyramidFinalContext();
            try
            {
                var dbObject = data.ProductImages.FirstOrDefault(i => i.TypeImage == TypeImage && i.ProductId == ProductId);

                return dbObject != null ? ConvertDbObjectToEntity(data, dbObject.Images) : new Image();
            }
            finally
            {
                if (_entities == null)
                    data.Dispose();
            }
        }

    }
}
