using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Models.JsonModels
{
   public class FilterJsonModel
    {
        public int Id { get; set; }

        public IEnumerable<int> EnumValueIds { get; set; }

        public FilterJsonModel() {
            EnumValueIds = new List<int>();
        }
    }
}
