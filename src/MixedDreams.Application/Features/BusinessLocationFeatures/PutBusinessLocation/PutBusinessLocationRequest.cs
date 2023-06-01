using MixedDreams.Infrastructure.Features.Common.Address;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.BusinessLocationFeatures.PutBusinessLocation
{
    public sealed record PutBusinessLocationRequest
    {
        public string Name { get; init; }
        public AddressDto Address { get; init; }
    }
}
