using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Configurations
{
    internal class CookConfiguration : IEntityTypeConfiguration<Cook>
    {
        public void Configure(EntityTypeBuilder<Cook> builder)
        {
            builder.HasQueryFilter(p => !p.Company.IsDeleted);
        }
    }
}
