using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.OrderFeatures.OrderProduct
{
    public sealed record PostOrderProductDto
    {
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
    }
}
