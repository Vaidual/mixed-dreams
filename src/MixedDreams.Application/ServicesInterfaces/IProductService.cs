using Microsoft.AspNetCore.Http;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.ServicesInterfaces
{
    public interface IProductService
    {
        public Task<Product> CreateProductAsync(PostPutProductRequest model);
    }
}
