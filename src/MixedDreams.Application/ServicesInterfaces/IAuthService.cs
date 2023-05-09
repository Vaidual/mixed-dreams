using MixedDreams.Application.Features.AuthFeatures;
using MixedDreams.Application.Features.AuthFeatures.Login;
using MixedDreams.Application.Features.AuthFeatures.RegisterCompany;
using MixedDreams.Application.Features.AuthFeatures.RegisterCustomer;
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
