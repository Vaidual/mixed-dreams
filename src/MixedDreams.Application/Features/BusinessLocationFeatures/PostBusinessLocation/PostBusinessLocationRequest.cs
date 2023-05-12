using MixedDreams.Application.Features.Common.Address;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.BusinessLocationFeatures.PostBusinessLocation
{
    public sealed record PostBusinessLocationRequest
    {
        public string Name { get; init; }
        public AddressDto Address { get; init; }
    }
}
