using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Services
{
    public class TenantService : ITenantService
    {
        public TenantService(IHttpContextAccessor httpContext)
        {
            if (httpContext.HttpContext == null)
                return;
            bool isCompany = httpContext.HttpContext.User.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Any(r => r.Value == Roles.Company);
            if (isCompany)
            {
                Claim tenantClaim = httpContext.HttpContext?.User.Claims
                    .FirstOrDefault(c => c.Type == "TenantId") ?? 
                    throw new InternalServerErrorException("User has a company role, however, TenantId isn't specified");
                    TenantId = Guid.Parse(tenantClaim.Value);
            }
        }

        public Guid? TenantId { get; private set; } = null;
        public void SetTenant(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }
}
