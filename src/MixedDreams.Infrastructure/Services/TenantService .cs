using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MixedDreams.Infrastructure.Exceptions;
using MixedDreams.Infrastructure.Exceptions.InternalServerError;
using MixedDreams.Infrastructure.Hubs.Clients;
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
        public Guid? TenantId { get; private set; } = null;
        public void SetTenant(Guid tenantId)
        {
            TenantId = tenantId;
        }
    }
}
