using Microsoft.AspNetCore.Http;
using MixedDreams.Application.Exceptions.InternalServerError;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Application.Constants;
using System.Security.Claims;

namespace MixedDreams.WebAPI.Middlewares
{
    public class TenantMiddleware : IMiddleware
    {
        private readonly ITenantSetter _tenantSetter;
        public TenantMiddleware(ITenantSetter tenantSetter)
        {
            this._tenantSetter = tenantSetter;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
            bool isCompany = httpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Any(r => r.Value == Roles.Company);
            if (isCompany)
            {
                Claim tenantClaim = httpContext.User.Claims
                    .FirstOrDefault(c => c.Type == "TenantId") ??
                    throw new InternalServerErrorException("User has a company role, however, TenantId isn't specified");
                _tenantSetter.SetTenant(Guid.Parse(tenantClaim.Value));
            }
            await next(httpContext);
        }
    }
}
