using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MixedDreams.Infrastructure.Entities;
using System.Security.Claims;

namespace MixedDreams.WebAPI.IdentitySetup
{
    public class MixedDreamsClaimsFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public MixedDreamsClaimsFactory(UserManager<ApplicationUser> userManager, IOptions<IdentityOptions> optionsAccessor)
            : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            var roles = await UserManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role));
            }
            return identity;
        }
    }
}
