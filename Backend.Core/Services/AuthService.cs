using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MixedDreams.Core.Dto;
using MixedDreams.Core.Exceptions;
using MixedDreams.Core.Helpers;
using MixedDreams.Core.RepositoryInterfaces;
using MixedDreams.Core.Responses;
using MixedDreams.Core.Responses.Errors;
using MixedDreams.Core.ServicesInterfaces;
using MixedDreams.Infrastructure.Entities;
using MixedDreams.Infrastructure.StaticTypes;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AuthService(
            IConfiguration configuration,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            _configuration = configuration;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<TokenResponse> LoginUser(LoginDto model)
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

        public async Task LogoutUser()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<TokenResponse> RegisterUser(RegisterDto model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                throw new BadRequestException("Email is already taken");
            }

            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                throw new BadRequestException(result.Errors.First().Description);
            }

            await _userManager.AddToRoleAsync(user, Roles.USER);

            var token = await CreateTokenAsync(user, false);

            return new TokenResponse(new JwtSecurityTokenHandler().WriteToken(token));
        }

        private async Task<JwtSecurityToken> CreateTokenAsync(ApplicationUser user, bool rememberMe)
        {
            var claims = new List<Claim>();
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            Lazy<Claim> a;
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
