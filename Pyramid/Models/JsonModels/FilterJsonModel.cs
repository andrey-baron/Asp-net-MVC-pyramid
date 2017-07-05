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

        public IEnumerable<EnumValueJsonModel> EnumValues { get; set; }

        public FilterJsonModel() {
            EnumValues = new List<EnumValueJsonModel>();
        }
    }
}
