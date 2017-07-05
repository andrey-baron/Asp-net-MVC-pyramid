using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Entity
{
    public class EnumValue
    {
        public int Id { get; set; }
        public string Key { get; set; }
        public Common.TypeFromEnumValue TypeValue { get; set; }
    }
}
