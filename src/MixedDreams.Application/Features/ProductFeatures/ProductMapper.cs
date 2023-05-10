using AutoMapper;
using MixedDreams.Application.Features.ProductFeatures.GetProduct;
using MixedDreams.Application.Features.ProductFeatures.GetProductWithDetails;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.ProductFeatures
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, GetProductResponse>();
            CreateMap<Product, GetProductWithDetailsResponse>();
            CreateMap<PostPutProductRequest, Product>();
        }
    }
}
