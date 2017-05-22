using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBFirstDAL.Repositories
{
    public class FilterRepository : GenericRepository<PyramidFinalContext, Filters>
    {
        public IEnumerable<EnumValues> GetAllEnumValues(int filterId)
        {
            var filter = FindBy(i => i.Id == filterId).SingleOrDefault();
            if (filter!=null)
            {
                return filter.EnumValues;
            }
            return new List<EnumValues>();
        }
        public void DeleteEnumValue(int id, int enumValueId)
        {
            var filter = FindBy(i => i.Id == id).SingleOrDefault();
            if (filter != null)
            {
                var enumvalue = Context.EnumValues.Find(enumValueId);
                filter.EnumValues.Remove(enumvalue);

            }
        }
        public override void AddOrUpdate(Filters entity)
        {
            var prop = entity.GetType().GetProperty("Id");
            int id = (int)prop.GetValue(entity, null);

            var efEntity = Context.Filters.Find(id);
            if (efEntity == null)
            {
                Context.Filters.Add(entity);
            }
            else
            {
                
                efEntity.EnumValues.Clear();
                foreach (var item in entity.EnumValues)
                {
                    var efEVal = Context.EnumValues.Find(item.Id);
                    if (efEVal!=null)
                    {
                        efEntity.EnumValues.Add(efEVal);
                    }
                    
                }
                Context.Entry(efEntity).CurrentValues.SetValues(entity);
                Context.Entry(efEntity).State = System.Data.Entity.EntityState.Modified;

            }

        }
    }
    
}
