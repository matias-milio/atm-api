namespace ATM.Core
{
    public class PaginatedList<T> : List<T>
    {
        public int CurrentPage { get; private set;}
        public int TotalPages { get; private set;}
        public int PageSize { get; private set;}
        public int TotalCount {get; private set;}

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
    }
}
