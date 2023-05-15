using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.Common;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.Exceptions.BadRequest;
using MixedDreams.Application.Exceptions.InternalServerError;
using MixedDreams.Application.Exceptions.NotFound;
using MixedDreams.Application.Features.BusinessLocationFeatures.GetBusinessLocation;
using MixedDreams.Application.Features.BusinessLocationFeatures.PostBusinessLocation;
using MixedDreams.Application.Features.BusinessLocationFeatures.PutBusinessLocation;
using MixedDreams.Application.Features.Errors;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Constants;
using System.Security.Claims;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/business-locations")]
    [ApiController]
    public class BusinessLocationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<PostBusinessLocationRequest> _postBusinessLocationValidator;
        private readonly IValidator<PutBusinessLocationRequest> _putBusinessLocationValidator;

        public BusinessLocationController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<PutBusinessLocationRequest> putBusinessLocationValidator, IValidator<PostBusinessLocationRequest> postBusinessLocationValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _putBusinessLocationValidator = putBusinessLocationValidator;
            _postBusinessLocationValidator = postBusinessLocationValidator;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetBusinessLocation(Guid id, CancellationToken cancellationToken)
        {
            BusinessLocation businessLocation = await _unitOfWork.BusinessLocationRepository.Get(id) ?? throw new EntityNotFoundException(nameof(BusinessLocation), id.ToString());

            return Ok(_mapper.Map<GetBusinessLocationResponse>(businessLocation));
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.Company}, {Roles.Administrator}")]
        public async Task<IActionResult> GetBusinessLocations(CancellationToken cancellationToken)
        {
            IReadOnlyList<BusinessLocation> businessLocations = await _unitOfWork.BusinessLocationRepository.GetAll(cancellationToken);

            return Ok(_mapper.Map<IReadOnlyList<BusinessLocation>, IReadOnlyList<GetBusinessLocationResponse>>(businessLocations));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> PostBusinessLocation([FromBody] PostBusinessLocationRequest model)
        {
            ValidationResult validationResult = await _postBusinessLocationValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }
            if (await _unitOfWork.BusinessLocationRepository.IsNameTaken(model.Name))
            {
                throw new PropertyIsTakenException(nameof(model.Name), model.Name);
            }

            BusinessLocation businessLocation = _mapper.Map<BusinessLocation>(model);
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            businessLocation.CompanyId = await _unitOfWork.CompanyRepository.GetCompanyIdByUserIdAsync(userId)
                ?? throw new InternalServerErrorException($"User with id {userId} doesn't connected to company however has company role ");
            businessLocation = _unitOfWork.BusinessLocationRepository.Create(businessLocation);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetBusinessLocation), new { id = businessLocation.Id }, _mapper.Map<GetBusinessLocationResponse>(businessLocation));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> PutBusinessLocation([FromRoute] Guid id, [FromBody] PutBusinessLocationRequest model)
        {
            ValidationResult validationResult = await _putBusinessLocationValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }

            BusinessLocation businessLocation = await _unitOfWork.BusinessLocationRepository.Get(id) ?? throw new PutNotFoundException();
            if (businessLocation.Name != model.Name && await _unitOfWork.BusinessLocationRepository.IsNameTaken(model.Name))
            {
                throw new PropertyIsTakenException(nameof(model.Name), model.Name);
            }

            _unitOfWork.BusinessLocationRepository.Update(_mapper.Map(model, businessLocation));
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> DeleteBusinessLocation([FromRoute] Guid id)
        {
            bool exist = await _unitOfWork.BusinessLocationRepository.EntityExistsAsync(id);
            if (!exist)
            {
                throw new EntityNotFoundException(nameof(BusinessLocation), id.ToString());
            }
            await _unitOfWork.BusinessLocationRepository.ExecuteDeleteAsync(x => x.Id == id);

            return NoContent();
        }
    }
}
