using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Pyramid.Global
{
    public static class Config
    {
        public static int PageSize
        {
            get
            {
                string pageSizeString = ConfigurationManager.AppSettings["PageSize"] as string;
                int match = 0;
                int.TryParse(pageSizeString, out match);
                return match > 0 ? match : 20;
            }
        }

        public static string PathNotFilledImage {
            get
            {
                string pathImage = ConfigurationManager.AppSettings["PathNotFilledImage"] as string;

                return pathImage!=null? pathImage: "/Content/img/pypamid-no-photo.png";
            }
        }

        public static bool IsProduction
        {
            get
            {
                string stringIsProd = ConfigurationManager.AppSettings["IsProd"] as string;
                bool IsProdFlag = false;
                bool.TryParse(stringIsProd, out IsProdFlag);

                return IsProdFlag;                
            }
        }

        public static string PathFtpOneCServerUpload
        {
            get
            {
                string path = ConfigurationManager.AppSettings["PathOneCServerUpload"] as string;

                return path != null ? path : "/Content/img/pypamid-no-photo.png";
            }
        }

    }
}