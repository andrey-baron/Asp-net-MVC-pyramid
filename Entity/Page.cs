using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class Page:BaseEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        
        public string Content { get; set; }
        public int ImageId { get; set; }

        public virtual Image Image { get; set; }
    }
}
