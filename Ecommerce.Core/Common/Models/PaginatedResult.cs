namespace Ecommerce.Core.Common.Models
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Data { get; }
        public int TotalCount { get; }
        public int Page { get; }
        public int PageSize { get; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;

        public PaginatedResult(IEnumerable<T> data, int totalCount, int page, int pageSize)
        {
            Data = data;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
        }


    }
}
