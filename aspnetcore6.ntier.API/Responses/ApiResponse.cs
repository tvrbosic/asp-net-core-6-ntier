using System.Net;

namespace aspnetcore6.ntier.API.Responses
{
    public class ApiResponse<T>
    {
        public ApiResponse()
        {
            Success = true;
        }

        public HttpStatusCode StatusCode;
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
