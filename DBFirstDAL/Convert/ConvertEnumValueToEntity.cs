using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.Convert
{
   public class ConvertEnumValueToEntity
    {
        public static Pyramid.Entity.EnumValue Convert(EnumValues dbEnumValue)
        {
            return new Pyramid.Entity.EnumValue() {
                Id = dbEnumValue.Id,
                Key=dbEnumValue.Key,
                TypeValue=(Common.TypeFromEnumValue) dbEnumValue.TypeValue
            };
        }
        public static IList<Pyramid.Entity.EnumValue> ConvertRange(IEnumerable<EnumValues> dbEnumValueList)
        {
            return dbEnumValueList.Select(s => Convert(s)).ToList();
        }
    }
}
