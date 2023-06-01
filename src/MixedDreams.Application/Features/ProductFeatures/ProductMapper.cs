using AutoMapper;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetCategories;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetCompanyProducts;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetProduct;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetProductNames;
using MixedDreams.Infrastructure.Features.ProductFeatures.GetProductWithDetails;
using MixedDreams.Infrastructure.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Infrastructure.Features.ProductFeatures.PutProduct;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Features.ProductFeatures
{
    public class ProductMapper : Profile
    {
        public ProductMapper()
        {
            CreateMap<Product, GetProductResponse>()
                .ForMember(dest => dest.PrimaryImage, opt => opt
                .MapFrom(src => src.Image != null ? src.Image.Path : null));
            CreateMap<Product, GetProductWithDetailsResponse>()
                .ForMember(dest => dest.PrimaryImage, opt => opt
                .MapFrom(src => src.Image != null ? src.Image.Path : null))
                .ForMember(dest => dest.ProductCategory, opt => opt
                .MapFrom(src => src.ProductCategoryId));
            CreateMap<PostProductRequest, Product>();
            CreateMap<PutProductRequest, Product>();
            CreateMap<ProductCategory, GetCategoryResponse>();
            CreateMap<Product, GetProductNamesResponse>();
            CreateMap<Product, CompanyProductDto>();
        }
    }
}
