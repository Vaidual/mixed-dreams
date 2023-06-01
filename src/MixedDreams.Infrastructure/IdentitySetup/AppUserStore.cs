using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Data;

namespace MixedDreams.Infrastructure.IdentitySetup
{
    internal class AppUserStore : UserStore<ApplicationUser>
    {
        public AppUserStore(AppDbContext context)
        : base(context)
        {
            AutoSaveChanges = false;
        }
    }
}
