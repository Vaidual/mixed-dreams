using Microsoft.AspNetCore.Identity;

namespace MixedDreams.Infrastructure.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime LastLoginDate { get; set; }
    }
}
