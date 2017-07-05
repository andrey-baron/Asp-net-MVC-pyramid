using Pyramid.Models.CategoryModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pyramid.Models.JsonModels
{
    class CategoryFiltersJsonModel
    {
        public int CategoryId { get; set; }

        public IEnumerable<FilterJsonModel> Filters { get; set; }

        public double MaxPrice { get; set; }

        public double MinPrice { get; set; }

        public CategoryFiltersJsonModel()
        {
            Filters = new List<FilterJsonModel>();
        }

        public static CategoryFiltersJsonModel ConvertToJsonModel(CategoryViewModel viewModel)
        {
            var model = new CategoryFiltersJsonModel()
            {

                CategoryId = viewModel.Id,
                Filters = viewModel.Filters.Select(s => new FilterJsonModel()
                {
                    Id = s.Id,
                    EnumValues = s.EnumValues.Where(w => w.Checked == true).Select(e => new EnumValueJsonModel() { Id = e.Id }).ToList()
                }),
                MaxPrice = viewModel.CurrentMaxPrice,
                MinPrice = viewModel.CurrentMinPrice
            };
            return model;

            
        }
    }
}
