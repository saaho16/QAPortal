using System.Data.Common;
using System.Data.SqlTypes;
using System.Net;
using System.Security.Authentication;
using System.Text.Json;

namespace ExceptionFiltersDemo.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
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
            _logger.LogError(exception, "Unhandled exception.");

            var statusCode = (int)HttpStatusCode.InternalServerError;
            var error = "Internal Server Error";
            var message = _env.IsDevelopment() ? exception.Message : "An unexpected error occurred.";

            switch (exception)
            {
                case ArgumentException argEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    error = "Bad Request";
                    message = argEx.Message;
                    break;

                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    error = "Not Found";
                    message = "Resource not found.";
                    break;

                case AuthenticationException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    error = "Unauthorized";
                    message = "Authentication failed.";
                    break;

                case UnauthorizedAccessException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    error = "Unauthorized";
                    message = "Unauthorized access.";
                    break;

                case InvalidOperationException:
                    statusCode = (int)HttpStatusCode.Conflict;
                    error = "Conflict";
                    message = exception.Message;
                    break;

                case SqlTypeException:
                case DbException:
                    statusCode = (int)HttpStatusCode.ServiceUnavailable;
                    error = "Database Error";
                    message = "A database error occurred.";
                    break;

                case NotImplementedException:
                    statusCode = (int)HttpStatusCode.NotImplemented;
                    error = "Not Implemented";
                    message = "This functionality is not implemented.";
                    break;

                case TimeoutException:
                    statusCode = (int)HttpStatusCode.RequestTimeout;
                    error = "Request Timeout";
                    message = "The request timed out.";
                    break;
            }

            var errorResponse = new
            {
                Status = statusCode,
                Error = error,
                Message = message
            };

            var jsonResponse = JsonSerializer.Serialize(errorResponse);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(jsonResponse);
        }
    }
}