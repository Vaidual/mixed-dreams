using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Hosting;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Configurations
{
    internal class CookConfiguration : IEntityTypeConfiguration<Cook>
    {
        public void Configure(EntityTypeBuilder<Cook> builder)
        {
            builder.HasQueryFilter(p => !p.Company.IsDeleted);

            builder.HasOne(x => x.CurrentProductPreparation)
                .WithOne(x => x.Cook)
                .OnDelete(DeleteBehavior.NoAction)
                .HasForeignKey<Cook>(x => x.CurrentProductPreparationId);

            //builder
            //    .HasOne(c => c.CurrentProductPreparation)
            //    .WithOne()
            //    .HasForeignKey<ProductPreparation>(p => p.CookId);

            //builder
            //    .HasOne(c => c.LastProductPreparation)
            //    .WithOne()
            //    .HasForeignKey<Cook>(c => c.LastProductPreparationId);

            //builder
            //    .HasMany(x => x.ProductPreparations)
            //    .WithOne(p => p.Cook)
            //    .HasForeignKey(pv => pv.CookId);
        }
    }
}
