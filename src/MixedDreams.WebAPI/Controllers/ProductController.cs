using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.Dto;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.ServicesInterfaces;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProduct()
        {
            //var response = await _authService.RegisterCustomerAsync(model);

            return Ok();
        }
    }
}
