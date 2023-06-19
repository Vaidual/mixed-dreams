using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.Common;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.Exceptions.BadRequest;
using MixedDreams.Application.Exceptions.NotFound;
using MixedDreams.Application.Features.Errors;
using MixedDreams.Application.Features.IngredientFeatures.GetIngredient;
using MixedDreams.Application.Features.IngredientFeatures.PostIngredient;
using MixedDreams.Application.Features.IngredientFeatures.PutIngredient;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Application.Constants;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/ingredients")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<PostIngredientRequest> _postIngredientValidator;
        private readonly IValidator<PutIngredientRequest> _putIngredientValidator;

        public IngredientController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<PutIngredientRequest> putIngredientValidator, IValidator<PostIngredientRequest> postIngredientValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _putIngredientValidator = putIngredientValidator;
            _postIngredientValidator = postIngredientValidator;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = $"{Roles.Company}, {Roles.Administrator}")]
        public async Task<IActionResult> GetIngredient(Guid id, CancellationToken cancellationToken)
        {
            Ingredient ingredient = await _unitOfWork.IngredientRepository.GetAsync(id) ?? throw new EntityNotFoundException(nameof(Ingredient), id.ToString()); ;

            return Ok(_mapper.Map<GetIngredientResponse>(ingredient));
        }

        [HttpGet]
        [Authorize(Roles = $"{Roles.Company}, {Roles.Administrator}")]
        public async Task<IActionResult> GetIngredients(CancellationToken cancellationToken)
        {
            IReadOnlyList<Ingredient> ingredients = await _unitOfWork.IngredientRepository.GetAllNoTrackingAsync(cancellationToken);

            return Ok(_mapper.Map<IReadOnlyList<Ingredient>, IReadOnlyList<GetIngredientResponse>>(ingredients));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> PostIngredient([FromBody] PostIngredientRequest model)
        {
            ValidationResult validationResult = await _postIngredientValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }
            if (await _unitOfWork.IngredientRepository.IsNameTaken(model.Name))
            {
                throw new PropertyIsTakenException(nameof(model.Name), model.Name);
            }

            Ingredient Ingredient = _unitOfWork.IngredientRepository.Create(_mapper.Map<Ingredient>(model));
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetIngredient), new { id = Ingredient.Id }, _mapper.Map<GetIngredientResponse>(Ingredient));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> PutIngredient([FromRoute] Guid id, [FromBody] PutIngredientRequest model)
        {
            ValidationResult validationResult = await _putIngredientValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }

            Ingredient ingredient = await _unitOfWork.IngredientRepository.GetAsync(id) ?? throw new EntityNotFoundException(nameof(Ingredient), id.ToString()); ;
            if (ingredient.Name != model.Name && await _unitOfWork.IngredientRepository.IsNameTaken(model.Name))
            {
                throw new PropertyIsTakenException(nameof(model.Name), model.Name);
            }

            _unitOfWork.IngredientRepository.Update(_mapper.Map(model, ingredient));
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> DeleteIngredient([FromRoute] Guid id)
        {
            bool exist = await _unitOfWork.IngredientRepository.EntityExistsAsync(id);
            if (!exist)
            {
                throw new EntityNotFoundException(nameof(Ingredient), id.ToString());
            }
            await _unitOfWork.IngredientRepository.ExecuteDeleteAsync(x => x.Id == id);

            return NoContent();
        }
    }
}
