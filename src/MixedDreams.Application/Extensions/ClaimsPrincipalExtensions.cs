using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetClaim(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            return claimsPrincipal.Claims.FirstOrDefault(x => x.Type == claimType)?.Value;
        }
    }
}
