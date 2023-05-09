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
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(p => p.Price).HasPrecision(18, 2);
            builder.HasQueryFilter(p => !p.Company.IsDeleted);

            builder.Property(x => x.Name)
                .HasColumnType("nvarchar(50)");
            builder.Property(x => x.Description)
                .HasColumnType("nvarchar(4000)");
            builder.Property(x => x.PrimaryImage)
                .HasColumnType("varchar(2100)");

            //builder.HasOne(x => x.ProductCategory)
            //    .WithMany(x => x.Products)
            //    .HasForeignKey(x => x.ProductCategoryId);
        }
    }
}
