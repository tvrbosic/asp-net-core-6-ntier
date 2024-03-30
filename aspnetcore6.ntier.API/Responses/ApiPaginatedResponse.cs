using System.Net;

namespace aspnetcore6.ntier.API.Responses
{
    public class ApiPagnatedResponse<TEntity> : ApiBaseResponse
    {
        public ApiPagnatedResponse(
            IEnumerable<TEntity> data, 
            int currentPage, 
            int totalPages, 
            int pageSize, 
            int totalCount, 
            bool hasPrevious, 
            bool hasNext, 
            string message = "", 
            bool success = true, 
            HttpStatusCode statusCode = HttpStatusCode.OK) : base(message, success, statusCode)
        {
            CurrentPage = currentPage;
            TotalPages = totalPages;
            PageSize = pageSize;
            TotalCount = totalCount;
            HasPrevious = hasPrevious;
            HasNext = hasNext;
            Data = data;
        }

        public int CurrentPage { get; }
        public int TotalPages { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public bool HasPrevious { get; }
        public bool HasNext { get; }
        public IEnumerable<TEntity> Data { get; }
    }
}
