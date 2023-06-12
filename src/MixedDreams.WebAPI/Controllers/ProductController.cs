using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Infrastructure.Common;
using MixedDreams.Infrastructure.Exceptions;
using MixedDreams.Infrastructure.Exceptions.NotFound;
using MixedDreams.Infrastructure.Extensions;
using MixedDreams.Infrastructure.Features;
using MixedDreams.Infrastructure.Features.Common;
using MixedDreams.Infrastructure.Features.Errors;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetCategories;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetProduct;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetProductNames;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetProductWithDetails;
using MixedDreams.Infrastructure.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Infrastructure.Features.ProductFeatures.PutProduct;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using MixedDreams.Infrastructure.Hubs.Clients;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Constants;
using Polly;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading;
using ValidationResult = FluentValidation.Results.ValidationResult;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetCompanyProducts;
using MixedDreams.Application.Features.ProductFeatures.Dto;
using MixedDreams.Application.Features.ProductFeatures.GetProducts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
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
        public async Task<IActionResult> GetProducts(
            CancellationToken cancellationToken, 
            [FromQuery][Range(1, 50)] int size = 20, 
            [FromQuery, Range(0, 50)] int page = 0, 
            [FromQuery] string? key = null, 
            [FromQuery] string? category = null, 
            [FromQuery] string? sort = null)
        {
            ProductPages products = 
                await _unitOfWork.ProductRepository.GetPages(
                    page: page, 
                    size: size, 
                    cancellationToken: cancellationToken, 
                    key: key, 
                    category: category,
                    sort: sort);

            return Ok(_mapper.Map<ProductPages, GetProductsResponse>(products));
        }

        [HttpGet("{id}/details")]
        [Authorize(Roles = $"{Roles.Company}, {Roles.Administrator}")]
        public async Task<IActionResult> GetProductWithDetails([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            GetProductWithDetailsResponse product = await _unitOfWork.ProductRepository.GetProductInformation(id, cancellationToken)
                ?? throw new EntityNotFoundException(nameof(Product), id.ToString());

            return Ok(product);
        }

        [HttpGet("~/api/company/products")]
        [Authorize(Roles = $"{Roles.Company}, {Roles.Administrator}")]
        public async Task<IActionResult> GetProductsForCompany(CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.ProductRepository.GetAll(cancellationToken);

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<CompanyProductDto>>(products));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Company)]
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

        [HttpPost("{id}/duplicate")]
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> DuplicateProduct([FromRoute] Guid id)
        {
            Product product = await _unitOfWork.ProductRepository.Get(id) ?? throw new EntityNotFoundException(nameof(Product), id.ToString());
            product.Id = Guid.Empty;
            product.Name = await _unitOfWork.ProductRepository.GenerateUniqueCopyName(product.Name);
            _unitOfWork.ProductRepository.Create(product);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetProductWithDetails), new { id = product.Id }, _mapper.Map<GetProductWithDetailsResponse>(product));
        }


        [HttpPut("{id}")]
        [Authorize(Roles = Roles.Company)]
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
        [Authorize(Roles = Roles.Company)]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
        {
            Product product = await _unitOfWork.ProductRepository.Get(id) ?? throw new EntityNotFoundException(nameof(Product), id.ToString());
            await _productService.DeleteProductAsync(product);

            return NoContent();
        }
    }
}
