using MixedDreams.WebAPI.Middleware;

namespace MixedDreams.WebAPI.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(
            this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionMiddleware>();
    }
}
