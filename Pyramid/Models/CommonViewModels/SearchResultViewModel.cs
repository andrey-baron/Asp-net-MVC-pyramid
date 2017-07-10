using Common.SearchClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pyramid.Models.CommonViewModels
{
    public sealed class SearchResultViewModel<TModel>
    {
        public IList<TModel> Objects { get; set; }
        public PagesInfoModel PagesInfo { get; set; }

        public static SearchResultViewModel<TModel> CreateFromSearchResult<TEntity>(SearchResult<TEntity> searchResult, Func<TEntity, TModel> transformToModel, int displayedPages)
        {
            var objectsCount = searchResult.RequestedObjectsCount ?? searchResult.Total - searchResult.RequestedStartIndex;
            return new SearchResultViewModel<TModel>
            {
                Objects = searchResult.Objects.Select(transformToModel).ToList(),
                PagesInfo = new PagesInfoModel(searchResult.Total, objectsCount, searchResult.RequestedStartIndex / objectsCount + 1, displayedPages)
            };
        }
    }
}