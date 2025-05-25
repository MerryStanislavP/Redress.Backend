using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Redress.Backend.Application.Common.Exceptions;

namespace Redress.Backend.API.Middleware
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An unhandled exception occurred");
                await HandleExceptionAsync(context, exception);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var result = string.Empty;

            switch (exception)
            {
                case ValidationException validationException:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        error = "Validation failed",
                        details = validationException.Errors.Select(e => new
                        {
                            property = e.PropertyName,
                            message = e.ErrorMessage
                        })
                    });
                    break;

                case NotFoundException notFoundException:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new
                    {
                        error = "Resource not found",
                        message = notFoundException.Message
                    });
                    break;

                case UnauthorizedAccessException _:
                    code = HttpStatusCode.Unauthorized;
                    result = JsonSerializer.Serialize(new
                    {
                        error = "Unauthorized access",
                        message = "You don't have permission to access this resource"
                    });
                    break;

                case DbUpdateConcurrencyException _:
                    code = HttpStatusCode.Conflict;
                    result = JsonSerializer.Serialize(new
                    {
                        error = "Concurrency conflict",
                        message = "The resource was modified by another user"
                    });
                    break;

                case DbUpdateException _:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        error = "Database error",
                        message = "An error occurred while updating the database"
                    });
                    break;

                case InvalidOperationException _:
                    code = HttpStatusCode.BadRequest;
                    result = JsonSerializer.Serialize(new
                    {
                        error = "Invalid operation",
                        message = exception.Message
                    });
                    break;

                case KeyNotFoundException _:
                    code = HttpStatusCode.NotFound;
                    result = JsonSerializer.Serialize(new
                    {
                        error = "Resource not found",
                        message = exception.Message
                    });
                    break;

                default:
                    result = JsonSerializer.Serialize(new
                    {
                        error = "Internal server error",
                        message = "An unexpected error occurred"
                    });
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}