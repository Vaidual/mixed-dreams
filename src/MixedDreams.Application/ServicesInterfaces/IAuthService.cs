using MixedDreams.Application.Features.Auth;
using MixedDreams.Application.Features.Auth.Login;
using MixedDreams.Application.Features.Auth.RegisterCompany;
using MixedDreams.Application.Features.Auth.RegisterCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.ServicesInterfaces
{
    public interface IAuthService
    {
        public Task<TokenResponse> RegisterCustomerAsync(CustomerRegisterRequest model);
        public Task<TokenResponse> RegisterCompanyAsync(CompanyRegisterRequest model);
        public Task<TokenResponse> LoginUserAsync(LoginRequest model);
        public Task LogoutUserAsync();
    }
}
