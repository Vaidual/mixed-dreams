using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.Features;
using MixedDreams.Application.Features.Common;
using MixedDreams.Application.Features.Errors;
using MixedDreams.Application.Features.ProductFeatures.GetProduct;
using MixedDreams.Application.Features.ProductFeatures.GetProductWithDetails;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Constants;
using Polly;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize(Roles = $"{Roles.Administrator}, {Roles.Company}")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IValidator<PostPutProductRequest> _postPutProductValidator;

        public ProductController(
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IValidator<PostPutProductRequest> postPutProductValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _postPutProductValidator = postPutProductValidator;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            Product? product = await _unitOfWork.ProductRepository.Get(id, cancellationToken);
            if (product == null)
            {
                return NotFound(new EntityNotFoundResponse(nameof(Product), id.ToString()));
            }

            return Ok(_mapper.Map<GetProductResponse>(product));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken)
        {
            List<Product> products = await _unitOfWork.ProductRepository.GetAll(cancellationToken);

            return Ok(_mapper.Map<List<Product>, IReadOnlyList<GetProductResponse>>(products));
        }

        [HttpGet("pages")]
        [Authorize]
        public async Task<IActionResult> GetProductsPages(CancellationToken cancellationToken, [FromQuery][Range(1, 50)] int size = 50, [FromQuery][Range(0, 50)] int page = 0)
        {
            //List<Product> products = await _unitOfWork.ProductRepository.GetAll(cancellationToken);
            List<Product> products = await _unitOfWork.ProductRepository.GetPagedData(page, size, cancellationToken);
            return Ok(_mapper.Map<List<Product>, IReadOnlyList<GetProductResponse>>(products));
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetProductWithDetails([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            Product? product = await _unitOfWork.ProductRepository.Get(id, cancellationToken);
            if (product == null)
            {
                return NotFound(new EntityNotFoundResponse(nameof(Product), id.ToString()));
            }

            return Ok(_mapper.Map<GetProductWithDetailsResponse>(product));
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromForm] PostPutProductRequest model)
        {
            ValidationResult validationResult = await _postPutProductValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }

            Product product = await _productService.CreateProductAsync(model, model.PrimaryImage);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetProductWithDetails), new { id = product.Id}, _mapper.Map<GetProductWithDetailsResponse>(product));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] Guid id, [FromBody] PostPutProductRequest model)
        {
            ValidationResult validationResult = await _postPutProductValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }

            Product? product = await _unitOfWork.ProductRepository.Get(id);
            if (product is null)
            {
                return BadRequest(new PutNotFoundResponse());
            }
            Product productToUpdate = _mapper.Map<Product>(model);
            productToUpdate.Id = id;
            _unitOfWork.ProductRepository.Update(productToUpdate);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            Product? product = await _unitOfWork.ProductRepository.Get(id, cancellationToken);
            if (product is null)
            {
                return BadRequest(new EntityNotFoundResponse(nameof(Product), id.ToString()));
            }
            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.SaveAsync();

            return NoContent();
        }
    }
}
