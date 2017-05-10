using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class Image
    {
        public int Id { get; set; }
        public string PathInFileSystem { get; set; }
        public string ImgAlt { get; set; }
        public string Title { get; set; }
    }
}
