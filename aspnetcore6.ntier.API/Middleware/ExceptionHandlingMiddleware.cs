using aspnetcore6.ntier.API.Responses;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security;

namespace aspnetcore6.ntier.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ExceptionResponse response;
            _logger.LogError(exception, "An unexpected error occurred.");


            switch (exception)
            {
                // Bad request (400)
                case ArgumentNullException:
                    response = new ExceptionResponse(HttpStatusCode.BadRequest, "The request is missing required parameters.");
                    break;
                case ArgumentException:
                    response = new ExceptionResponse(HttpStatusCode.BadRequest, "The request contains invalid arguments.");
                    break;
                case ValidationException:
                    response = new ExceptionResponse(HttpStatusCode.BadRequest, "Validation failed for the request data.");
                    break;
                case FormatException:
                    response = new ExceptionResponse(HttpStatusCode.BadRequest, "The request data is not in the expected format.");
                    break;
                // Unauthorized (401)
                case UnauthorizedAccessException:
                    response = new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized access. Please authenticate.");
                    break;
                // Forbidden (403)
                case SecurityException:
                    response = new ExceptionResponse(HttpStatusCode.Forbidden, "Access denied. You don't have permission to access this resource.");
                    break;
                // Internal server error (500)
                default:
                    response = new ExceptionResponse(HttpStatusCode.InternalServerError, "Internal server error. Please try again later or contact the administrator for support.");
                    break;

            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
