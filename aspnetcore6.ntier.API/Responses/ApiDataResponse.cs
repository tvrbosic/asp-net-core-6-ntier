using System.Net;

namespace aspnetcore6.ntier.API.Responses
{
    public class ApiDataResponse<T> : ApiBaseResponse
    {
        public T Data { get; }

        public ApiDataResponse(T data, string message, HttpStatusCode statusCode = HttpStatusCode.OK)
            : base(message, true, statusCode)
        {
            Data = data;
        }
    }
}
