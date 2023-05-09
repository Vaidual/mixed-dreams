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
    internal class BusinessLocationConfiguration : IEntityTypeConfiguration<BusinessLocation>
    {
        public void Configure(EntityTypeBuilder<BusinessLocation> builder)
        {
            builder.OwnsOne(bl => bl.Address);
            builder.HasQueryFilter(bl => !bl.Company.IsDeleted);

            builder.Property(x => x.Name)
                .HasColumnType("nvarchar(50)");
        }
    }
}
