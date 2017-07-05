using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class VideoGuide
    {

        public int HomeEntityId { get; set; }
        public string SrcVideoThumbnail { get; set; }
        public string LinkYouTobe { get; set; }

        public Nullable<int> ThumbnailId { get; set; }

        public virtual HomeEntity HomeEntity { get; set; }
        public virtual Image Images { get; set; }
    }
}
