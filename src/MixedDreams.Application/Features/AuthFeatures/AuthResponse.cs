using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.AuthFeatures
{
    public class AuthResponse
    {
        public UserDto User { get; set; }
        public TokensDto Tokens { get; set; }
    }
}
