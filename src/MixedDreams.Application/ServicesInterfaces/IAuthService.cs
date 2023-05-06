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
        public Task<TokenResponse> RegisterCustomerAsync(CustomerRegisterDto model);
        public Task<TokenResponse> RegisterCompanyAsync(CompanyRegisterDto model);
        public Task<TokenResponse> LoginUserAsync(LoginDto model);
        public Task LogoutUserAsync();
    }
}
