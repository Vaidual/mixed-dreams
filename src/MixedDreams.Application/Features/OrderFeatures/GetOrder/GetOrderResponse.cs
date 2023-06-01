using MixedDreams.Infrastructure.Features.Common.Address;
using MixedDreams.Domain.Entities;
using MixedDreams.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.OrderFeatures.GetOrder
{
    public class GetOrderResponse
    {
        public Guid Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public AddressDto Address { get; set; }
    }
}
