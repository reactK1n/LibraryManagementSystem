using System.Net;
using LibraryManagementSystem.Api.Extensions;
using Newtonsoft.Json;

namespace LibraryManagementSystem.Extensions
{
    public class GlobalExceptionHandler : IMiddleware
    {
        private readonly LoggerHandler _logger;

        public GlobalExceptionHandler(LoggerHandler logger) => _logger = logger;

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                _logger.TraceId = Guid.NewGuid().ToString();

                _logger.Info("---------request has been made---------");
                await next(context);
            }
            catch (ArgumentNullException ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                _logger.Error($"StackTrace:---------{ex.StackTrace}------------");

                _logger.Error($"Exception Message:---------{ex.InnerException?.Message ?? ex.Message}------------");
                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { Message = ex.InnerException?.Message ?? ex.Message, Succeeded = false, StatusCode = ((int)HttpStatusCode.NotFound).ToString() }));
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                _logger.Error($"StackTrace:---------{ex.StackTrace}------------");

                _logger.Error($"Exception Message:---------{ex.InnerException?.Message ?? ex.Message}------------");

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new { Message = ex.InnerException?.Message ?? ex.Message, Succeeded = false, StatusCode = ((int)HttpStatusCode.InternalServerError).ToString() }));
            }
        }
    }
}