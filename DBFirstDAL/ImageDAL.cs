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
                 
                    var t=dbContext.Images.Select(image => new Pyramid.Entity.Image() {
                Id=image.Id,
                ImgAlt=image.ImgAlt,
                PathInFileSystem=image.PathInFileSystem,
                ServerPathImg=image.ServerPathImg,
                Title=image.Title
                }).ToList();
                t.Reverse();
                return t;
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
        public static void AddOrUpdate(Pyramid.Entity.Image image=null, HttpPostedFileBase files =null)
        {
                using (PyramidFinalContext dbContext = new PyramidFinalContext())
            {
                if (files!=null)
                {
                    
                        if (image == null && files != null)
                        {
                            Images efImg = SaveFile(files);
                            dbContext.Images.Add(efImg);
                        }
                        else
                        {
                            if (image != null)
                            {
                                var efImage = dbContext.Images.Find(image.Id);
                                if (efImage != null)
                                {
                                    efImage.ImgAlt = image.ImgAlt;
                                    efImage.Title = image.Title;

                                }
                            }
                        }

                    
                }

                dbContext.SaveChanges();
                }
            
        }
        static Images SaveFile(HttpPostedFileBase file)
        {
            Random r = new Random();
            var salt = r.Next(10000000).ToString();
            var filename = DateTime.Now.ToString("d") + "_"+ salt + "_" + Path.GetFileName(file.FileName);
            var title = Path.GetFileNameWithoutExtension(file.FileName);
            var pathInFileSystem = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryFilesImage,filename);
            var serverPath = Path.Combine(pathServerImg, filename);
            file.SaveAs(pathInFileSystem);
            return new Images()
            {
                PathInFileSystem = pathInFileSystem,
                ServerPathImg= serverPath,
                Title = title
            };
        }
        static void DeleteFile(string path)
        {
            if (path!=null&& File.Exists(path))
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
                    entity.HomeEntity.Clear();
                    entity.BannerWithPoints.Clear();
                    entity.CategoryImages.Clear();
                    entity.EventBanners.Clear();
                    entity.EventImages.Clear();
                    entity.Recommendations.Clear();
                    dbContext.SaveChanges();

                    DeleteFile(entity.PathInFileSystem);
                    dbContext.Images.Remove(entity);
                }
                dbContext.SaveChanges();
            }
        }
    }
}
