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
    internal class BackblazeFileConfiguration : IEntityTypeConfiguration<BackblazeFile>
    {
        public void Configure(EntityTypeBuilder<BackblazeFile> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .HasColumnType("varchar(200)").ValueGeneratedNever();

            builder.Property(x => x.FileName)
                .HasColumnType("varchar(50)");

            builder.Property(x => x.Path)
                .HasColumnType("varchar(200)");

            builder.HasQueryFilter(x => !x.Product.Company.IsDeleted);
        }
    }
}
