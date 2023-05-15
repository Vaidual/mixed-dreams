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
        /// <summary>
        /// Determines how long the access will be valid for remember me option.
        /// </summary>
        public TimeSpan RememberMeAccessTokenDuration { get; }

        /// <summary>
        /// Determines how long the access will be valid by default.
        /// </summary>
        public TimeSpan StandardAccessTokenDuration { get; }
        public Task<TokenResponse> RegisterCustomerAsync(CustomerRegisterRequest model);
        public Task<TokenResponse> RegisterCompanyAsync(CompanyRegisterRequest model);
        public Task<TokenResponse> LoginUserAsync(LoginRequest model);
        public Task LogoutUserAsync();
    }
}
