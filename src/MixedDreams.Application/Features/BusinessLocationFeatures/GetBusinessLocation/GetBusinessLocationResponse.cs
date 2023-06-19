using MixedDreams.Application.Features.Common.Address;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.BusinessLocationFeatures.GetBusinessLocation
{
    public class GetBusinessLocationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressDto Address { get; set; }
    }
}
