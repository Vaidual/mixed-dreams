using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.Features.Errors;
using MixedDreams.Application.Features.IngredientFeatures.GetIngredient;
using MixedDreams.Application.Features.IngredientFeatures.PostIngredient;
using MixedDreams.Application.Features.IngredientFeatures.PutIngredient;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Constants;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/ingredients")]
    [ApiController]
    [Authorize(Roles = Roles.Company)]
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
        [Authorize]
        public async Task<IActionResult> GetIngredient(Guid id, CancellationToken cancellationToken)
        {
            Ingredient? ingredient = await _unitOfWork.IngredientRepository.Get(id);
            if (ingredient == null)
            {
                return NotFound(new EntityNotFoundResponse(nameof(Ingredient), id.ToString()));
            }

            return Ok(_mapper.Map<GetIngredientResponse>(ingredient));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetIngredients(CancellationToken cancellationToken)
        {
            List<Ingredient> ingredients = await _unitOfWork.IngredientRepository.GetAll(cancellationToken);

            return Ok(_mapper.Map<List<Ingredient>, IReadOnlyList<GetIngredientResponse>>(ingredients));
        }

        [HttpPost]
        public async Task<IActionResult> PostIngredient([FromBody] PostIngredientRequest model)
        {
            ValidationResult validationResult = await _postIngredientValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }
            if (await _unitOfWork.IngredientRepository.IsNameTaken(model.Name))
            {
                return BadRequest(new PropertyIsTakenBadRequestResponse(nameof(model.Name), model.Name));
            }

            Ingredient Ingredient = _unitOfWork.IngredientRepository.Create(_mapper.Map<Ingredient>(model));
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetIngredient), new { id = Ingredient.Id }, _mapper.Map<GetIngredientResponse>(Ingredient));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutIngredient([FromRoute] Guid id, [FromBody] PutIngredientRequest model)
        {
            ValidationResult validationResult = await _putIngredientValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }

            Ingredient? ingredient = await _unitOfWork.IngredientRepository.Get(id);
            if (ingredient is null)
            {
                return BadRequest(new PutNotFoundResponse());
            }
            if (ingredient.Name != model.Name && await _unitOfWork.IngredientRepository.IsNameTaken(model.Name))
            {
                return BadRequest(new PropertyIsTakenBadRequestResponse(nameof(model.Name), model.Name));
            }

            _unitOfWork.IngredientRepository.Update(_mapper.Map(model, ingredient));
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient([FromRoute] Guid id)
        {
            bool exist = await _unitOfWork.IngredientRepository.EntityExistsAsync(id);
            if (!exist)
            {
                return BadRequest(new EntityNotFoundResponse(nameof(Ingredient), id.ToString()));
            }
            await _unitOfWork.IngredientRepository.ExecuteDeleteAsync(x => x.Id == id);

            return NoContent();
        }
    }
}
