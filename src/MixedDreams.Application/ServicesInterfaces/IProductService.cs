using Microsoft.AspNetCore.Http;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Application.Features.ProductFeatures.PutProduct;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.ServicesInterfaces
{
    public interface IProductService
    {
        public Task<Product> CreateProductAsync(PostProductRequest model, ClaimsPrincipal user);
        public Task UpdateProductAsync(Product productToUpdate, PutProductRequest updateModel);
        public Task DeleteProductAsync(Product product);
    }
}
