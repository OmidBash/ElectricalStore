namespace Contracts.Responses
{
    public class PaginationResponse<T>
    {
        public T Data { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        
        public PaginationResponse() { }

        public PaginationResponse(T response)
        {
            Data = response;
        }
    }
}