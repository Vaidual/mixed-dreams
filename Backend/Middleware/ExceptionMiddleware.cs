using Microsoft.AspNetCore.Http;
using System.Net;
using Serilog;
using Microsoft.AspNetCore.Http.HttpResults;
using MixedDreams.Core.Responses;

namespace MixedDreams.WebAPI.Middleware
{
    public class ExceptionMiddleware : IMiddleware
    {
        private readonly Serilog.ILogger _logger;

        public ExceptionMiddleware(Serilog.ILogger logger)
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
                _logger.Error($"Something went wrong: {ex}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorResponse();
            switch (exception)
            {
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Title = "An internal server error occured.";
                    break;
            };
            await context.Response.WriteAsync(response.ToString());
        }
    }
}
