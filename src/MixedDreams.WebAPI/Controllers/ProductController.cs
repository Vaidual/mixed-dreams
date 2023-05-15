using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.Common;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.Exceptions.NotFound;
using MixedDreams.Application.Extensions;
using MixedDreams.Application.Features;
using MixedDreams.Application.Features.Common;
using MixedDreams.Application.Features.Errors;
using MixedDreams.Application.Features.ProductFeatures.GetCategories;
using MixedDreams.Application.Features.ProductFeatures.GetProduct;
using MixedDreams.Application.Features.ProductFeatures.GetProductNames;
using MixedDreams.Application.Features.ProductFeatures.GetProductWithDetails;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Application.Features.ProductFeatures.PutProduct;
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
        private readonly IValidator<PostProductRequest> _postProductValidator;
        private readonly IValidator<PutProductRequest> _putProductValidator;

        public ProductController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<PostProductRequest> postProductValidator,
            IProductService productService,
            IValidator<PutProductRequest> putProductValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _postProductValidator = postProductValidator;
            _productService = productService;
            _putProductValidator = putProductValidator;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            Product product = await _unitOfWork.ProductRepository.Get(id, cancellationToken) ?? throw new EntityNotFoundException(nameof(Product), id.ToString());

            return Ok(_mapper.Map<GetProductResponse>(product));
        }

        [HttpGet]
        [Route("~/api/product-names")]
        [Authorize]
        public async Task<IActionResult> GetProductNames([FromQuery] string key = "", [FromQuery][Range(1, 20)] int count = 10, CancellationToken cancellationToken = default)
        {
            List<Product> products = await _unitOfWork.ProductRepository.GetProductNamesAsync(key.Trim(), count, cancellationToken);

            return Ok(_mapper.Map<List<Product>, IReadOnlyList<GetProductNamesResponse>>(products));
        }

        [HttpGet("categories")]
        [Authorize]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
        {
            IReadOnlyList<ProductCategory> categories = await _unitOfWork.ProductCategoryRepository.GetAll(cancellationToken);

            return Ok(_mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<GetCategoryResponse>>(categories));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetProducts(CancellationToken cancellationToken, [FromQuery][Range(1, 50)] int size = 20, [FromQuery][Range(0, 50)] int page = 0, [FromQuery] string key = "", [FromQuery] string category = "")
        {
            IReadOnlyList<Product> products = 
                await _unitOfWork.ProductRepository.GetPages(
                    page: page, 
                    size: size, 
                    cancellationToken: cancellationToken, 
                    key: key != string.Empty ? key : null, 
                    category: category != string.Empty ? category : null);

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<GetProductResponse>>(products));
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetProductWithDetails([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            Product product = await _unitOfWork.ProductRepository.Get(id, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(Product), id.ToString());

            return Ok(_mapper.Map<GetProductWithDetailsResponse>(product));
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromForm] PostProductRequest model)
        {
            ValidationResult validationResult = await _postProductValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }

            Product product = await _productService.CreateProductAsync(model, User);

            return CreatedAtAction(nameof(GetProductWithDetails), new { id = product.Id}, _mapper.Map<GetProductWithDetailsResponse>(product));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromRoute] Guid id, [FromForm] PutProductRequest model)
        {
            ValidationResult validationResult = await _putProductValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }

            await _productService.UpdateProductAsync(id, model);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            Product product = await _unitOfWork.ProductRepository.Get(id) ?? throw new EntityNotFoundException(nameof(Product), id.ToString());
            await _productService.DeleteProductAsync(product);

            return NoContent();
        }
    }
}
