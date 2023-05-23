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
using MixedDreams.Application.Features.AuthFeatures;
using MixedDreams.Application.Common;
using MixedDreams.Application.Features.IngredientFeatures.PostIngredient;
using FluentValidation.Results;
using FluentValidation;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<CustomerRegisterRequest> _registerCustomerValidator;
        private readonly IValidator<CompanyRegisterRequest> _registerCompanyValidator;
        private readonly IValidator<LoginRequest> _loginValidator;

        public AuthController(IAuthService authService, IValidator<CustomerRegisterRequest> registerCustomerValidator, IValidator<CompanyRegisterRequest> registerCompanyValidator, IValidator<LoginRequest> loginValidator)
        {
            _authService = authService;
            _registerCustomerValidator = registerCustomerValidator;
            _registerCompanyValidator = registerCompanyValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost]
        [Route("register/customer")]
        public async Task<IActionResult> RegisterCustomer(CustomerRegisterRequest model)
        {
            ValidationResult validationResult = await _registerCustomerValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }
            AuthResponse response = await _authService.RegisterCustomerAsync(model);

            return Ok(response);
        }

        [HttpPost]
        [Route("register/company")]
        public async Task<IActionResult> RegisterCompany([FromBody] CompanyRegisterRequest model)
        {
            ValidationResult validationResult = await _registerCompanyValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }
            AuthResponse response = await _authService.RegisterCompanyAsync(model);

            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            ValidationResult validationResult = await _loginValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }
            AuthResponse response = await _authService.LoginUserAsync(model);

            return Ok(response);
        }
    }
}
