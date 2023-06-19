using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Configurations
{
    internal class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasOne(c => c.ApplicationUser)
                .WithOne(a => a.Customer)
                .HasForeignKey<Customer>(c => c.ApplicationUserId)
                .IsRequired();
            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
