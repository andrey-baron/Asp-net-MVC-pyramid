using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Pyramid.Tools
{
    public class UploadXmlToFileSystem
    {
        public const string pathDirectoryFiles = "Content\\UserUploadXml\\";

        public static bool Upload(HttpPostedFileBase uploadxml)
        {
            bool error = false;
            try
            {
                
                var pathInFileSystem = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryFiles, uploadxml.FileName);
                var pathDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pathDirectoryFiles);
                DirectoryInfo dirInfo = new DirectoryInfo(pathDirectory);
                if (!dirInfo.Exists)
                {
                    dirInfo.Create();
                }
                uploadxml.SaveAs(pathInFileSystem);

            }
            catch (Exception)
            {

                error=true;
            }
            return error;

        }
    }
}