using System.Net;

namespace aspnetcore6.ntier.API.Responses
{
    public class ApiBaseResponse
    {
        public ApiBaseResponse(string message, bool success = true, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            Success = success;
            StatusCode = statusCode;
            Message = message;
        }

        public bool Success { get; }
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }
    }
}
