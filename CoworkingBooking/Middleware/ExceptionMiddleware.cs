using System.Net;
using System.Text.Json;

namespace CoworkingBooking.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }
        public static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                error = "Internal Server Error",
                message = exception.Message,
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
