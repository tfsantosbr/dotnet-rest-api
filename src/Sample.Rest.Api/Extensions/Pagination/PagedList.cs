using System;
using System.Collections.Generic;

namespace Sample.Rest.Api.Extensions.Pagination
{
    public class PagedList<TItem> : List<TItem>, IPagedList<TItem>
    {
        public PagedList(IEnumerable<TItem> items, long count, int pageNumber, int pageSize)
        {
            TotalRecords = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (long)Math.Ceiling(count / (double)pageSize);

            if (items != null)
            {
                AddRange(items);
            }
        }

        public int CurrentPage { get; }
        public int PageSize { get; }
        public long TotalPages { get; }
        public long TotalRecords { get; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
    }
}