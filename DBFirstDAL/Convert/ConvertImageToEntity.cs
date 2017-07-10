using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.Convert
{
   public class ConvertImageToEntity
    {
        public static Pyramid.Entity.Image Convert(Images dbImage)
        {
            if (dbImage != null)
            {


                return new Pyramid.Entity.Image()
                {
                    Id = dbImage.Id,
                    ImgAlt = dbImage.ImgAlt,
                    PathInFileSystem = dbImage.PathInFileSystem,
                    ServerPathImg = dbImage.ServerPathImg,
                    Title = dbImage.Title
                };
            }
            else {
                return new Pyramid.Entity.Image();
            }

        }
    }
}
