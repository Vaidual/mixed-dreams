using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Tenants
{
    public class TenantService : ITenantService
    {
        public TenantService(IHttpContextAccessor httpContext)
        {
            Claim? tenantClaim = httpContext.HttpContext?.User.Claims.FirstOrDefault(c => c.Type == "TenantId");
            if (tenantClaim is not null)
                TenantId = Guid.Parse(tenantClaim.Value);
        }

        public Guid? TenantId { get; private set; } = null;
        public void SetTenant(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }

    public interface ITenantService : ITenantGetter, ITenantSetter { }
    public interface ITenantGetter
    {
        Guid? TenantId { get; }
    }
    public interface ITenantSetter
    {
        void SetTenant(Guid tenantId);
    }
}
