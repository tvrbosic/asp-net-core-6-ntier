using System.Net;

namespace aspnetcore6.ntier.API.Responses
{
    public record ExceptionResponse(HttpStatusCode StatusCode, string Message);
}
