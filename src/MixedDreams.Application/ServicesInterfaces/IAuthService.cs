using MixedDreams.Application.Dto;
using MixedDreams.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.ServicesInterfaces
{
    public interface IAuthService
    {
        public Task<TokenResponse> RegisterUserAsync(RegisterDto model);
        public Task<TokenResponse> LoginUserAsync(LoginDto model);
        public Task LogoutUserAsync();
    }
}
