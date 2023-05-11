using Microsoft.AspNetCore.Http;
using MixedDreams.Application.Common;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.Features.Errors;

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
            ErrorResponse response;
            if (exception is BaseException baseException)
            {
                response = baseException.GetErrorResponse();
                _logger.LogWarning("Something went wrong: {@exception}", baseException.Title);
            }
            else
            {
                response = new InternalServerErrorResponse();
                _logger.LogError("Internal error happened: {@exception}", exception.Message);
            }
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsync(response.ToString());
        }
    }
}
