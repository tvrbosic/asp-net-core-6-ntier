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
            // Logger
            LogLevel logLevel;
            string logErrorMessage;

            // Response
            HttpStatusCode statusCode;
            string returnErrorMessage = "";
            ExceptionResponse response;


            // Determine response and log message based on exception type
            switch (exception)
            {
                // Bad request (400)
                case HttpListenerException:

                case ArgumentNullException:
                    logLevel = LogLevel.Warning;
                    logErrorMessage = exception.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    returnErrorMessage = "The request is missing required parameters.";
                    break;
                case ArgumentException:
                    logLevel = LogLevel.Warning;
                    logErrorMessage = exception.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    returnErrorMessage = "The request contains invalid arguments.";
                    break;
                case ValidationException:
                    logLevel = LogLevel.Warning;
                    logErrorMessage = exception.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    returnErrorMessage = "Validation failed for the request data.";
                    break;
                case FormatException:
                    logLevel = LogLevel.Warning;
                    logErrorMessage = exception.Message;
                    statusCode = HttpStatusCode.BadRequest;
                    returnErrorMessage = "The request data is not in the expected format.";
                    break;
                // Unauthorized (401)
                case UnauthorizedAccessException:
                    logLevel = LogLevel.Warning;
                    logErrorMessage = exception.Message;
                    statusCode = HttpStatusCode.Unauthorized;
                    response = new ExceptionResponse(HttpStatusCode.Unauthorized, "Unauthorized access. Please authenticate.");
                    break;
                // Forbidden (403)
                case SecurityException:
                    logLevel = LogLevel.Warning;
                    logErrorMessage = exception.Message;
                    statusCode = HttpStatusCode.Forbidden;
                    returnErrorMessage = "Access denied. You don't have permission to access this resource.";
                    break;
                // Internal server error (500)
                default:
                    logLevel = LogLevel.Error;
                    logErrorMessage = exception.Message;
                    statusCode = HttpStatusCode.InternalServerError;
                    returnErrorMessage = "Internal server error. Please try again later or contact the administrator for support.";
                    break;

            }

            // Log error message based on log level
            if (logLevel == LogLevel.Warning)
            {
                _logger.LogWarning(exception, logErrorMessage);
            }
            else
            {
                _logger.LogError(exception, logErrorMessage);
            }


            // Construct response
            response = new ExceptionResponse(statusCode, returnErrorMessage);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }
    }
}