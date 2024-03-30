using System.Net;

namespace aspnetcore6.ntier.API.Responses
{
    public class ApiErrorResponse : ApiBaseResponse
    {
        public ApiErrorResponse(HttpStatusCode statusCode, string message)
            : base(message, false, statusCode)
        {
        }
    }
}
