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
using MixedDreams.Application.Common;
using MixedDreams.Application.Dto.Auth;

namespace MixedDreams.Infrastructure.Services
{
    internal class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            this._unitOfWork = unitOfWork;
        }

        public async Task<TokenResponse> LoginUserAsync(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            //var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                throw new BadRequestException("Invalid credentials");
            }

            await _userManager.UpdateAsync(user);

            var token = await CreateTokenAsync(user, model.RememberMe);

            return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task LogoutUserAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<TokenResponse> RegisterCustomerAsync(CustomerRegisterDto model)
        {
            var user = await CreateUserAsync(model);
            await RegisterUserAsync(model, user, Roles.Customer);
            await _unitOfWork.Save(CancellationToken.None);
            var token = await CreateTokenAsync(user, false);

            return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token));
        }

        public async Task<TokenResponse> RegisterCompanyAsync(CompanyRegisterDto model)
        {
            var user = await CreateUserAsync(model);
            await RegisterUserAsync(model, user, Roles.Company);

            var company = await CreateCompanyAsync(model);
            var claims = new List<Claim>()
            {
                new Claim("TenantId", company.Id.ToString())
            };
            var token = await CreateTokenAsync(user, false, claims);

            return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token));
        }

        private async Task<ApplicationUser> CreateUserAsync(RegisterDto registerModel)
        {
            var userExists = await _userManager.FindByEmailAsync(registerModel.Email);
            if (userExists != null)
            {
                throw new BadRequestException("Email is already taken");
            }

            var user = _mapper.Map<ApplicationUser>(registerModel);

            return user;
        }

        private async Task<Company> CreateCompanyAsync(CompanyRegisterDto model)
        {
            var company = _mapper.Map<Company>(model);

            return await _unitOfWork.CompanyRepository.CreateAsync(company);
        }

        private async Task RegisterUserAsync(RegisterDto registerModel, ApplicationUser user, string role)
        {
            var result = await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException(result.Errors.First().Description);
            }
            await _userManager.AddToRoleAsync(user, role);
        }

        private async Task<JwtSecurityToken> CreateTokenAsync(ApplicationUser user, bool rememberMe, List<Claim>? additionalClaims = null)
        {
            var claims = additionalClaims?? new List<Claim>();
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var token = JwtHelper.GetJwtToken(
                user.Id,
                _configuration["JwtToken:SigningKey"],
                _configuration["JwtToken:Issuer"],
                _configuration["JwtToken:Audience"],
                rememberMe ? TimeSpan.FromDays(14) : TimeSpan.FromHours(2),
                claims);

            return token;
        }
    }
}
