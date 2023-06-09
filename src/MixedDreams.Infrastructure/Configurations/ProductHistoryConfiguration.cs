﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Configurations
{
    internal class ProductHistoryConfiguration : IEntityTypeConfiguration<ProductHistory>
    {
        public void Configure(EntityTypeBuilder<ProductHistory> builder)
        {
            builder.Property(p => p.Price).HasPrecision(18, 2);
            builder.HasQueryFilter(p => !p.Product.Company.IsDeleted);

            builder.Property(x => x.Name)
                .HasColumnType("nvarchar(50)");
        }
    }
}
