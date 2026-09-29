using System.Net;
using System.Text.Json;

namespace EmployeeManagement.API.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger)
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
                _logger.LogError(
                    ex,
                    "Unhandled exception occurred. Path: {Path}",
                    context.Request.Path);

                context.Response.StatusCode = 500;

                await context.Response.WriteAsJsonAsync(new
                {
                    success = false,
                    message = "Something went wrong."
                });
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = "Something went wrong. Please try again.",
                statusCode = 500
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}