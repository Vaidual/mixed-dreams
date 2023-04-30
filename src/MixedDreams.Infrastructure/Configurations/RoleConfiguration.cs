using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixedDreams.Infrastructure.StaticTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Configurations
{
    internal class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
            new IdentityRole
            {
                Name = Roles.USER,
                NormalizedName = nameof(Roles.USER),
            },
            new IdentityRole
            {
                Name = Roles.ADMINISTATOR,
                NormalizedName = nameof(Roles.ADMINISTATOR),
            },
            new IdentityRole
            {
                Name = Roles.COMPANY,
                NormalizedName = nameof(Roles.COMPANY),
            });
        }
    }
}
