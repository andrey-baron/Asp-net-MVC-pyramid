using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ShortContent { get; set; }
        
        public System.DateTime DateEventStart { get; set; }
        public System.DateTime DateEventEnd { get; set; }
        public bool isActive { get; set; }

        public  Image Image { get; set; }
        public IEnumerable<Product>Products { get; set; }

        public Event()
        {
            Products = new List<Product>();
        }

    }
}
