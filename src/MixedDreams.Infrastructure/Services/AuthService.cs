using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Helpers;
using MixedDreams.Infrastructure.Constants;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using MixedDreams.Infrastructure.Data;
using MixedDreams.Application.Features.AuthFeatures;
using MixedDreams.Application.Features.AuthFeatures.RegisterCustomer;
using MixedDreams.Application.Features.AuthFeatures.Login;
using MixedDreams.Application.Features.AuthFeatures.RegisterCompany;
using MixedDreams.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using MixedDreams.Application.Constants;
using MixedDreams.Application.Exceptions.BadRequest;

namespace MixedDreams.Infrastructure.Services
{
    internal class AuthService : IAuthService
    {
        private readonly JwtOptions _jwtOptions;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppDbContext _context;

        public TimeSpan RememberMeAccessTokenDuration { get; private set; } = TimeSpan.FromDays(14);
        public TimeSpan StandardAccessTokenDuration { get; private set; } = TimeSpan.FromHours(2);

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            IUnitOfWork unitOfWork,
            AppDbContext context,
            IOptions<JwtOptions> jwtOptions)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            this._unitOfWork = unitOfWork;
            _context = context;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<TokenResponse> LoginUserAsync(LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            //var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new BadRequestException("Authentification failed.", new List<string> { "Invalid credentials" });
            }

            //await _userManager.UpdateAsync(user);

            List<Claim> claims = new();
            if ((await _userManager.GetRolesAsync(user)).Any(r => r == Roles.Company))
            {
                claims.Add(new Claim(AppClaimTypes.TenantId, user.Id.ToString()));
            }

            var token = await CreateTokenAsync(user, model.RememberMe, claims);

            return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<TokenResponse> RegisterCustomerAsync(CustomerRegisterRequest model)
        {
            var user = await CreateUserAsync(model);
            Customer customer;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await RegisterUserAsync(model, user, Roles.Customer);

                    customer = _mapper.Map<Customer>(model);
                    customer.ApplicationUser = user;
                    _unitOfWork.CustomerRepository.Create(customer);
                    await _unitOfWork.SaveAsync(CancellationToken.None);

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            var token = await CreateTokenAsync(user, false);

            return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<TokenResponse> RegisterCompanyAsync(CompanyRegisterRequest model)
        {
            var user = await CreateUserAsync(model);
            Company company;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await RegisterUserAsync(model, user, Roles.Company);

                    company = _mapper.Map<Company>(model);
                    company.ApplicationUser = user;
                    company = _unitOfWork.CompanyRepository.Create(company);
                    await _unitOfWork.SaveAsync(CancellationToken.None);

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            var claims = new List<Claim>
            {
                new Claim(AppClaimTypes.TenantId, user.Id.ToString()),
            };
            var token = await CreateTokenAsync(user, false, claims);

            return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token));
        }

        private async Task<ApplicationUser> CreateUserAsync(RegisterDto registerModel)
        {
            var userExists = await _userManager.FindByEmailAsync(registerModel.Email);
            if (userExists != null)
            {
                throw new BadRequestException("Authentification failed.", new List<string> { "Email is already taken" });
            }

            var user = _mapper.Map<ApplicationUser>(registerModel);

            return user;
        }

        private async Task RegisterUserAsync(RegisterDto registerModel, ApplicationUser user, string role)
        {
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException("Authentification failed.", result.Errors.Select(e => e.Description));
            }
            await _unitOfWork.SaveAsync(CancellationToken.None);
            await _userManager.AddToRoleAsync(user, role);
        }

        private async Task<JwtSecurityToken> CreateTokenAsync(ApplicationUser user, bool rememberMe, List<Claim>? additionalClaims = null)
        {
            List<Claim> claims = additionalClaims?? new List<Claim>();
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim("Name", user.FirstName));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            var token = JwtHelper.GetJwtToken(
                user.Id,
                _jwtOptions.SigningKey,
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                rememberMe ? RememberMeAccessTokenDuration : StandardAccessTokenDuration,
                claims);

            return token;
        }
    }
}
