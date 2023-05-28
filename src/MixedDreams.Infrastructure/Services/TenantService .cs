using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.Exceptions.InternalServerError;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Services
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
