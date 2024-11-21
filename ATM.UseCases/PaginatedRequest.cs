namespace ATM.UseCases
{
    public class PaginatedRequest(int pageIndex, int pageSize)
    {
        public int PageIndex { get; set; } = pageIndex;
        public int PageSize { get; set; } = pageSize;
    }
}
