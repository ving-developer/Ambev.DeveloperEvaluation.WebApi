using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Middleware
{
    /// <summary>
    /// Middleware for handling all unhandled exceptions in a consistent way.
    /// Validation exceptions should be handled separately by ValidationExceptionMiddleware.
    /// </summary>
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
                if (!(ex is ValidationException || ex is BadHttpRequestException))
                {
                    _logger.LogError(ex, "[ERR] An unhandled exception occurred.");
                }

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode status;
            ApiResponse response;

            switch (exception)
            {
                case KeyNotFoundException:
                    status = HttpStatusCode.NotFound;
                    response = new ApiResponse
                    {
                        Success = false,
                        Message = exception.Message
                    };
                    break;

                case UnauthorizedAccessException:
                    status = HttpStatusCode.Unauthorized;
                    response = new ApiResponse
                    {
                        Success = false,
                        Message = exception.Message
                    };
                    break;

                case ValidationException:
                    status = HttpStatusCode.BadRequest;
                    response = new ApiResponse
                    {
                        Success = false,
                        Message = "Validation failed"
                    };
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;
                    response = new ApiResponse
                    {
                        Success = false,
                        Message = "An unexpected error occurred"
                    };
                    break;
            }

            context.Response.StatusCode = (int)status;
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

            return context.Response.WriteAsync(json);
        }
    }
}
