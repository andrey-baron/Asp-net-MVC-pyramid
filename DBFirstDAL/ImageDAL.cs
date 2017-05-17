using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DBFirstDAL
{
    public class ImageDAL
    {
        public const string pathDirectoryFilesImage = "Content\\UserUpload\\";
        public const string pathServerImg = "/Content/UserUpload/";

        public static ICollection<Pyramid.Entity.Image> GetAll()
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                return dbContext.Images.Select(image => new Pyramid.Entity.Image() {
                Id=image.Id,
                ImgAlt=image.ImgAlt,
                PathInFileSystem=image.PathInFileSystem,
                ServerPathImg=image.ServerPathImg,
                Title=image.Title
                }).ToList();
            }
        }
        public static Pyramid.Entity.Image Get(int id)
        {
            using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {

                var image=dbContext.Images.Find(id);
                if (image != null)
                {
                    return new Pyramid.Entity.Image() {
                        Id = image.Id,
                        ImgAlt = image.ImgAlt,
                        PathInFileSystem = image.PathInFileSystem,
                        ServerPathImg = image.ServerPathImg,
                        Title = image.Title
                    };
                }
                return null;
            }
        }
        public static void AddOrUpdate(Pyramid.Entity.Image image=null, HttpPostedFileBase file=null)
        {
                using (PyramidFinalContext dbContext = new PyramidFinalContext())
                {
                    if (image== null && file!=null)
                    {
                        Images efImg= SaveFile(file);
                        dbContext.Images.Add(efImg);
                    }
                    else
                    {
                    if (image!=null)
                    {
                        var efImage = dbContext.Images.Find(image.Id);
                        if (efImage!=null)
                        {
                            efImage.ImgAlt = image.ImgAlt;
                            efImage.Title = image.Title;
                            
                        }
                    }
                }

                dbContext.SaveChanges();
                }
            
        }
        static Images SaveFile(HttpPostedFileBase file)
        {
            var pathInFileSystem = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryFilesImage, file.FileName);
            var serverPath = Path.Combine(pathServerImg, file.FileName);
            file.SaveAs(pathInFileSystem);
            return new Images()
            {
                PathInFileSystem = pathInFileSystem,
                ServerPathImg= serverPath,
                Title = file.FileName
            };
        }
        static void DeleteFile(string path)
        {
            if (path!=null)
            {
                File.Delete(path);
            }
           
        }
        public static void Delete(int id)
        {
            using (PyramidFinalContext dbContext= new PyramidFinalContext())
            {
                var entity=dbContext.Images.Find(id);
                if (entity!=null)
                {
                    DeleteFile(entity.PathInFileSystem);
                    dbContext.Images.Remove(entity);
                }
                dbContext.SaveChanges();
            }
        }
    }
}
