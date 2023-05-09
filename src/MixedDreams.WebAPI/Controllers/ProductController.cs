using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.Features;
using MixedDreams.Application.Features.Errors;
using MixedDreams.Application.Features.ProductFeatures.GetProduct;
using MixedDreams.Application.Features.ProductFeatures.GetProductWithDetails;
using MixedDreams.Application.Features.ProductFeatures.PostProduct;
using MixedDreams.Application.Features.ProductFeatures.PutProduct;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Constants;
using System.Threading;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    [Authorize(Roles = $"{Roles.Administrator}, {Roles.Company}")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.Get(id, cancellationToken);
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
            var products = await _unitOfWork.ProductRepository.GetAll(cancellationToken);

            return Ok(_mapper.Map<IReadOnlyList<GetProductResponse>>(products));
        }

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetProductWithDetails([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.Get(id, cancellationToken);
            if (product == null)
            {
                return NotFound(new EntityNotFoundResponse(nameof(Product), id.ToString()));
            }

            return Ok(_mapper.Map<GetProductWithDetailsResponse>(product));
        }

        [HttpPost]
        public async Task<IActionResult> PostProduct([FromBody] PostProductRequest model)
        {
            var product = await _unitOfWork.ProductRepository.CreateAsync(_mapper.Map<Product>(model));
            await _unitOfWork.Save();

            return CreatedAtAction(nameof(GetProductWithDetails), new { id = product.Id}, _mapper.Map<GetProductWithDetailsResponse>(product));
        }

        [HttpPut("id")]
        public async Task<IActionResult> PutProduct([FromRoute] Guid id, [FromBody] PutProductRequest model, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.Get(id, cancellationToken);
            if (product is null)
            {
                return BadRequest(new PutNotFoundResponse());
            }
            var productToUpdate = _mapper.Map<Product>(model);
            productToUpdate.Id = id;
            _unitOfWork.ProductRepository.Update(productToUpdate);
            await _unitOfWork.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.ProductRepository.Get(id, cancellationToken);
            if (product is null)
            {
                return BadRequest(new EntityNotFoundResponse(nameof(Product), id.ToString()));
            }
            _unitOfWork.ProductRepository.Delete(product);
            await _unitOfWork.Save();

            return NoContent();
        }
    }
}
