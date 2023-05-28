using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixedDreams.Domain.Entities;
using MixedDreams.Application.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Configurations
{
    internal class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasQueryFilter(c => !c.IsDeleted);

            builder.Property(x => x.Name)
                .HasColumnType("nvarchar(50)");

            builder.HasData(
                new ProductCategory
                {
                    Id = new Guid("a9572488-e307-4d70-ad4c-64dfe31819a1"),
                    Name = "Salad"
                },
                new ProductCategory
                {
                    Id = new Guid("a9572488-e307-4d70-ad4c-64dfe31819a2"),
                    Name = "Soup"
                },
                new ProductCategory
                {
                    Id = new Guid("a9572488-e307-4d70-ad4c-64dfe31819a3"),
                    Name = "Snacks"
                },
                new ProductCategory
                {
                    Id = new Guid("a9572488-e307-4d70-ad4c-64dfe31819a4"),
                    Name = "Garnish"
                },
                new ProductCategory
                {
                    Id = new Guid("a9572488-e307-4d70-ad4c-64dfe31819a5"),
                    Name = "Meat"
                },
                new ProductCategory
                {
                    Id = new Guid("a9572488-e307-4d70-ad4c-64dfe31819a6"),
                    Name = "Fish"
                },
                new ProductCategory
                {
                    Id = new Guid("a9572488-e307-4d70-ad4c-64dfe31819a7"),
                    Name = "Dessert"
                },
                new ProductCategory
                {
                    Id = new Guid("a9572488-e307-4d70-ad4c-64dfe31819a8"),
                    Name = "Full meal"
                },
                new ProductCategory
                {
                    Id = new Guid("a9572488-e307-4d70-ad4c-64dfe31819a9"),
                    Name = "Other"
                });
        }
    }
}
