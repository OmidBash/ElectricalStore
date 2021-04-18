namespace Contracts.Requests
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PaginationQuery() { }
        
        public PaginationQuery(int pageNumber = 1, int pageSize = 10)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}