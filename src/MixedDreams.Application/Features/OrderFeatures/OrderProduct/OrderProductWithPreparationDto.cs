using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.OrderFeatures.OrderProduct
{
    public sealed record OrderProductWithPreparationDto
    {
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
        public short PreparationTime { get; set; }
    }
}
