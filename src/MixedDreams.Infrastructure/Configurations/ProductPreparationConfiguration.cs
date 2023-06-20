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
    internal class ProductPreparationConfiguration : IEntityTypeConfiguration<ProductPreparation>
    {
        public void Configure(EntityTypeBuilder<ProductPreparation> builder)
        {

        }
    }
}
