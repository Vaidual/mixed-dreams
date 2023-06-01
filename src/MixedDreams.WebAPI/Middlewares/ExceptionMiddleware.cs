using Microsoft.AspNetCore.Http;
using MixedDreams.Infrastructure.Common;
using MixedDreams.Infrastructure.Exceptions;
using MixedDreams.Infrastructure.Features.Errors;
using Serilog.Events;

namespace MixedDreams.WebAPI.Middlewares
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            //var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>()!;
            context.Response.ContentType = "application/json";
            string response;
            if (exception is BaseHttpException baseException)
            {
                context.Response.StatusCode = baseException.StatusCode;
                response = baseException.GetResponse();
                _logger.Log(baseException.LogLevel, baseException, exception.Message);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                response = new InternalServerErrorResponse().ToString();
                _logger.LogError("An unexpected error happened: {@exception}", exception.Message);
            }
            await context.Response.WriteAsync(response);
        }
    }
}
