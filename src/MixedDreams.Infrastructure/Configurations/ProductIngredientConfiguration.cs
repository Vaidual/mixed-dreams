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
    internal class ProductIngredientConfiguration : IEntityTypeConfiguration<ProductIngredient>
    {
        public void Configure(EntityTypeBuilder<ProductIngredient> builder)
        {
            builder.HasOne(pi => pi.Product)
                .WithMany(p => p.ProductIngredients)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(pi => pi.Ingredient)
                .WithMany(i => i.ProductIngredients)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(pi => !pi.Product.Company.IsDeleted);
        }
    }
}
