using MixedDreams.WebAPI.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MixedDreams.Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using MixedDreams.Infrastructure.StaticTypes;
using MixedDreams.Core.Helpers;
using System.IdentityModel.Tokens.Jwt;

namespace Backend.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;

        public AuthController(
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IMapper mapper,
            IConfiguration config) 
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._mapper = mapper;
            this._config = config;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterVM model)
        {
            var userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return BadRequest("Email is already taken");
            }

            var user = _mapper.Map<ApplicationUser>(model);

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await _userManager.AddToRoleAsync(user, Roles.USER);

            var token = JwtHelper.GetJwtToken(
                user.Id,
                _config["JwtToken:SigningKey"],
                _config["JwtToken:Issuer"],
                _config["JwtToken:Audience"],
                TimeSpan.FromHours(2));

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }

        [Authorize]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return Ok(12);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginVM model, string returnUrl = null)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            //var signInResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return BadRequest("Invalid credentials");
            }

            //var user = await _userManager.FindByEmailAsync(model.Email);
            //user.LastLoginDate = DateTime.UtcNow;
            //await _userManager.UpdateAsync(user);

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.NameIdentifier, user.Id),
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim("LastLoginDate", DateTime.UtcNow.ToString())
            //};
            //var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            //var authProperties = new AuthenticationProperties
            //{
            //    IsPersistent = false,
            //    ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(20)
            //};
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            var token = JwtHelper.GetJwtToken(
                user.Id,
                _config["JwtToken:SigningKey"],
                _config["JwtToken:Issuer"],
                _config["JwtToken:Audience"],
                model.RememberMe ? TimeSpan.FromDays(14) : TimeSpan.FromHours(2));

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));

            //return RedirectToLocal(returnUrl);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return Ok();
                //return RedirectToAction(nameof(HomeController.Index), "Home");

        }
    }
}
