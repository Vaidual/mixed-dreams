using Microsoft.AspNetCore.Identity;
using MixedDreams.Domain.Common;

namespace MixedDreams.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Company? Company { get; set; }
        public Customer? Customer { get; set; }
    }
}
