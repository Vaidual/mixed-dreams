using MixedDreams.Infrastructure.Features.AuthFeatures;
using MixedDreams.Infrastructure.Features.AuthFeatures.Login;
using MixedDreams.Infrastructure.Features.AuthFeatures.RegisterCompany;
using MixedDreams.Infrastructure.Features.AuthFeatures.RegisterCustomer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Hubs.Clients
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
        public Task<AuthResponse> RegisterCustomerAsync(CustomerRegisterRequest model);
        public Task<AuthResponse> RegisterCompanyAsync(CompanyRegisterRequest model);
        public Task<AuthResponse> LoginUserAsync(LoginRequest model);
        public Task LogoutUserAsync();
    }
}
