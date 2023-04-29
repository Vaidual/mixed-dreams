using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MixedDreams.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Newtonsoft.Json.Serialization;
using MixedDreams.Infrastructure.StaticTypes;
using MixedDreams.Core.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using MixedDreams.Infrastructure.Data;
using MixedDreams.Core.Responses;
using MixedDreams.WebAPI.Filters;
using MixedDreams.Core.Responses.Errors;
using MixedDreams.Core.Dto;
using MixedDreams.Core.Repositories;
using MixedDreams.Core.ServicesInterfaces;

namespace Backend.Controllers
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
        [Route("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var response = await _authService.RegisterUser(model);

            return Ok(response);
        }

        //[Authorize]
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(12);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutUser();
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model, string returnUrl = null)
        {
            var response = _authService.LoginUser(model);

            return Ok(response);
        }
    }
}
