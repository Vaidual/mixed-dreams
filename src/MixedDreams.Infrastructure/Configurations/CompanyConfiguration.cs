﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Configurations
{
    internal class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.OwnsOne(c => c.Address);
            builder.HasOne(c => c.ApplicationUser)
                .WithOne(a => a.Company)
                .HasForeignKey<Company>(c => c.ApplicationUserId)
                .IsRequired();
            builder.HasQueryFilter(c => !c.IsDeleted);

            builder.Property(x => x.Birthday)
                .HasColumnType("date");
            builder.Property(x => x.CompanyName)
                .HasColumnType("nvarchar(50)");

            builder.Property(x => x.CooksNumber)
                .HasColumnType("smallint");

            builder.Property(x => x.MaxSimultaneousOrders)
                .HasColumnType("smallint");


            builder.Property(x => x.AcceptOrders)
                .HasDefaultValue(true);
        }
    }
}
