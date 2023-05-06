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
    internal class OrderProductConfiguration : IEntityTypeConfiguration<OrderProduct>
    {
        public void Configure(EntityTypeBuilder<OrderProduct> builder)
        {
            builder.HasOne(op => op.Order)
                .WithMany(o => o.OrderProducts)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(op => op.Product)
                .WithMany(p => p.OrderProducts)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(op => op.ProductHistory)
               .WithMany(ph => ph.OrderProducts)
               .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(op => !op.Order.IsDeleted);
        }
    }
}
