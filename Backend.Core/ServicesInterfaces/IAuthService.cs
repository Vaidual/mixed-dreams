using MixedDreams.Core.Dto;
using MixedDreams.Core.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Core.ServicesInterfaces
{
    public interface IAuthService
    {
        public Task<TokenResponse> RegisterUser(RegisterDto model);
        public Task<TokenResponse> LoginUser(LoginDto model);
        public Task LogoutUser();
    }
}
