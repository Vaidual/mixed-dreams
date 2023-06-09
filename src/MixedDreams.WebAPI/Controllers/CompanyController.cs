﻿using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.Exceptions.InternalServerError;
using MixedDreams.Application.Features.CompanyFeatures.GetSettings;
using MixedDreams.Application.Features.CompanyFeatures.PutCompanySettings;
using MixedDreams.Domain.Entities;
using MixedDreams.Application.Common;
using MixedDreams.Application.Constants;
using MixedDreams.Application.Exceptions.NotFound;
using MixedDreams.Application.Features.IngredientFeatures.GetIngredient;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Application.RepositoryInterfaces;
using System.Linq;
using System.Security.Claims;
using MixedDreams.Application.ServicesInterfaces;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/companies")]
    [ApiController]
    [Authorize(Roles = Roles.Company)]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("~/api/company/settings")]
        [Authorize(Roles = $"{Roles.Company}")]
        public async Task<IActionResult> GetSettings(CancellationToken cancellationToken)
        {
            Guid companyId = Guid.Parse((this.User.Claims.FirstOrDefault(x => x.Type == AppClaimTypes.EntityId) ?? throw new MissingClaimException(AppClaimTypes.EntityId)).Value);
            GetCompanySettings settings = await _unitOfWork.CompanyRepository.GetSettings(companyId) ?? throw new EntityNotFoundException(nameof(Company), companyId.ToString());

            return Ok(settings);
        }

        [HttpPut("~/api/company/settings")]
        [Authorize(Roles = $"{Roles.Company}")]
        public async Task<IActionResult> PutSettings([FromBody] PutCompanySettingsRequest model, [FromServices] IValidator<PutCompanySettingsRequest> validator, [FromServices] ICooksService cooksService)
        {
            ValidationResult validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }

            Guid companyId = Guid.Parse((this.User.Claims.First(x => x.Type == AppClaimTypes.EntityId) ?? throw new MissingClaimException(AppClaimTypes.EntityId)).Value);
            await _unitOfWork.CompanyRepository.UpdateSettings(companyId, model);
            await cooksService.UpdateCooksNumber(companyId, model.CooksNumber ?? 0);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
