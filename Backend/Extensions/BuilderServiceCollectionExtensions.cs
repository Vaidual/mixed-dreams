using Microsoft.AspNetCore.Identity;
using MixedDreams.Infrastructure.Models;
using MixedDreams.WebAPI.IdentitySetup;
using MixedDreams.WebAPI.Middleware;

namespace MixedDreams.WebAPI.Extensions
{
    public static class BuilderServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessServices(
            this IServiceCollection services)
        {
            return services;
        }

        public static IServiceCollection AddIdentityServices(
            this IServiceCollection services)
        {
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, MixedDreamsClaimsFactory>();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<SignInManager<ApplicationUser>>();
            return services;
        }

        public static IServiceCollection AddMiddlewareServices(
            this IServiceCollection services)
        {
            services.AddTransient<ExceptionMiddleware>();
            return services;
        }
    }
}
