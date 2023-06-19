using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Configurations
{
    public class DeviceConfiguration : IEntityTypeConfiguration<Device>
    {
        public void Configure(EntityTypeBuilder<Device> builder)
        {
            builder
                .HasOne(x => x.Company)
                .WithMany(x => x.Devices).IsRequired(false);

            builder
                .HasIndex(u => u.Identifier)
                .IsUnique();
        }
    }
}
