using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.AuthFeatures
{
    public sealed record AuthResponse
    {
        public required UserDto User { get; set; }
        public required Guid EntityId { get; set; }
        public required TokensDto Tokens { get; set; }
    }
}
