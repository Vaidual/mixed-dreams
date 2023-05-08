using MixedDreams.Application.Common;
using MixedDreams.Application.Exceptions;

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
            BaseException responseException;
            if (exception is BaseException baseException)
            {
                responseException = baseException;
                _logger.LogWarning("Something went wrong: {@exception}", baseException.Title);
            }
            else
            {
                responseException = new InternalServerErrorException();
                _logger.LogError("Internal error happened: {@exception}", exception);
            }
            context.Response.StatusCode = responseException.StatusCode;
            await context.Response.WriteAsync(responseException.GetErrorResponse().ToString());
        }
    }
}
