using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MixedDreams.Application.ServicesInterfaces;
using System.Threading;
using MixedDreams.Application.Features.AuthFeatures.RegisterCustomer;
using MixedDreams.Application.Features.AuthFeatures.RegisterCompany;
using MixedDreams.Application.Features.AuthFeatures.Login;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("register/customer")]
        public async Task<IActionResult> RegisterCustomer(CustomerRegisterRequest model)
        {
            var response = await _authService.RegisterCustomerAsync(model);

            return Ok(response);
        }

        [HttpPost]
        [Route("register/company")]
        public async Task<IActionResult> RegisterCompany([FromBody] CompanyRegisterRequest model)
        {
            var response = await _authService.RegisterCompanyAsync(model);

            return Ok(response);
        }

        //[HttpPost("logout")]
        //public async Task<IActionResult> Logout()
        //{
        //    await _authService.LogoutUserAsync();
        //    return Ok();
        //}

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            var response = await _authService.LoginUserAsync(model);

            return Ok(response);
        }
    }
}
