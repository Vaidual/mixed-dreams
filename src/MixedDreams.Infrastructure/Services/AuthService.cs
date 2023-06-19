using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.Hubs.Clients;
using MixedDreams.Domain.Entities;
using MixedDreams.Application.Helpers;
using MixedDreams.Application.Constants;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using MixedDreams.Application.Data;
using MixedDreams.Application.Features.AuthFeatures;
using MixedDreams.Application.Features.AuthFeatures.RegisterCustomer;
using MixedDreams.Application.Features.AuthFeatures.Login;
using MixedDreams.Application.Features.AuthFeatures.RegisterCompany;
using MixedDreams.Application.Options;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using MixedDreams.Application.Constants;
using MixedDreams.Application.Exceptions.BadRequest;
using MixedDreams.Application.Enums;
using MixedDreams.Application.Exceptions.InternalServerError;
using Stripe;
using Customer = MixedDreams.Domain.Entities.Customer;

namespace MixedDreams.Application.Services
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

        public async Task<AuthResponse> LoginUserAsync(LoginRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new BadRequestException("Authentification failed.", ErrorCodes.Invalidcredentials, new List<string> { "Invalid credentials" });
            }

            List<Claim> claims = new();
            if ((await _userManager.GetRolesAsync(user)).Any(r => r == Roles.Company))
            {
                claims.Add(new Claim(AppClaimTypes.TenantId, user.EntityId.ToString()));
            }

            var token = await CreateTokenAsync(user, model.RememberMe, claims);

            AuthResponse result = new()
            {
                EntityId = user.EntityId,
                Tokens = new TokensDto(new JwtSecurityTokenHandler().WriteToken(token)),
                User = _mapper.Map<UserDto>(user)
            };
            result.User.Roles = await _userManager.GetRolesAsync(user);
            return result;
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AuthResponse> RegisterCustomerAsync(CustomerRegisterRequest model)
        {
            var user = await GetUserToCreateAsync(model);
            Customer customer;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await CreateUserAsync(model, user, Roles.Customer);

                    customer = _mapper.Map<Customer>(model);
                    customer.ApplicationUser = user;
                    _unitOfWork.CustomerRepository.Create(customer);
                    await _unitOfWork.SaveAsync(CancellationToken.None);

                    await SetUserEntityIdAsync(user, customer.Id);

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
            var token = await CreateTokenAsync(user, false);

            AuthResponse result = new ()
            {
                EntityId = customer.Id,
                Tokens = new TokensDto(new JwtSecurityTokenHandler().WriteToken(token)),
                User = _mapper.Map<UserDto>(user)
            };
            result.User.Roles = await _userManager.GetRolesAsync(user);
            return result;
        }

        public async Task<AuthResponse> RegisterCompanyAsync(CompanyRegisterRequest model)
        {
            var user = await GetUserToCreateAsync(model);
            Company company;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await CreateUserAsync(model, user, Roles.Company);

                    company = _mapper.Map<Company>(model);
                    company.ApplicationUser = user;
                    company = _unitOfWork.CompanyRepository.Create(company);
                    await _unitOfWork.SaveAsync(CancellationToken.None);

                    await SetUserEntityIdAsync(user, company.Id);

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
                new Claim(AppClaimTypes.TenantId, user.EntityId.ToString()),
            };
            var token = await CreateTokenAsync(user, false, claims);

            AuthResponse result = new()
            {
                EntityId = company.Id,
                Tokens = new TokensDto(new JwtSecurityTokenHandler().WriteToken(token)),
                User = _mapper.Map<UserDto>(user)
            };
            result.User.Roles = await _userManager.GetRolesAsync(user);
            return result;
        }

        private async Task<ApplicationUser> GetUserToCreateAsync(RegisterDto registerModel)
        {
            var userExists = await _userManager.FindByEmailAsync(registerModel.Email);
            if (userExists != null)
            {
                throw new BadRequestException("Email is already taken.", ErrorCodes.EmailIsTaken, new List<string> { "Email is already taken" });
            }

            var user = _mapper.Map<ApplicationUser>(registerModel);

            return user;
        }

        private async Task CreateUserAsync(RegisterDto registerModel, ApplicationUser user, string role)
        {
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException("Authentification failed.", ErrorCodes.InternalError, result.Errors.Select(e => e.Description));
            }
            await _unitOfWork.SaveAsync(CancellationToken.None);
            await _userManager.AddToRoleAsync(user, role);
        }

        private async Task SetUserEntityIdAsync(ApplicationUser user, Guid entityId)
        {
            user.EntityId = entityId;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new InternalServerErrorException("User entity update process failded.");
            }
            await _unitOfWork.SaveAsync();
        }

        private async Task<JwtSecurityToken> CreateTokenAsync(ApplicationUser user, bool rememberMe, List<Claim>? additionalClaims = null)
        {
            List<Claim> claims = additionalClaims?? new List<Claim>();
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            claims.Add(new Claim(AppClaimTypes.Name, user.FirstName));
            claims.Add(new Claim(AppClaimTypes.UserId, user.Id));
            claims.Add(new Claim(AppClaimTypes.EntityId, user.EntityId.ToString()));
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
