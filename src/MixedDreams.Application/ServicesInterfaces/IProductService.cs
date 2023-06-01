using Microsoft.AspNetCore.Http;
using MixedDreams.Infrastructure.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Infrastructure.Features.ProductFeatures.PutProduct;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Hubs.Clients
{
    public interface IProductService
    {
        public Task<Product> CreateProductAsync(PostProductRequest model, ClaimsPrincipal user);
        public Task UpdateProductAsync(Guid id, PutProductRequest updateModel);
        public Task DeleteProductAsync(Product product);
    }
}
