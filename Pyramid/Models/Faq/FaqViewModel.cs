using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Models.Faq
{
    public class FaqViewModel
    {
        public IEnumerable<Entity.FAQ> AllFaq { get; set; }
        public Entity.FAQ CurrentFaq { get; set; }

        public FaqViewModel()
        {
            AllFaq = new List<Entity.FAQ>();
        }
    }
}