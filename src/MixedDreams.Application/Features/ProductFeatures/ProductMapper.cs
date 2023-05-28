using AutoMapper;
using MixedDreams.Application.Features.ProductFeatures.GetCategories;
using MixedDreams.Application.Features.ProductFeatures.GetCompanyProducts;
using MixedDreams.Application.Features.ProductFeatures.GetProduct;
using MixedDreams.Application.Features.ProductFeatures.GetProductNames;
using MixedDreams.Application.Features.ProductFeatures.GetProductWithDetails;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Application.Features.ProductFeatures.PutProduct;
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
            CreateMap<Product, GetProductResponse>()
                .ForMember(dest => dest.PrimaryImage, opt => opt
                .MapFrom(src => src.Image != null ? src.Image.Path : null));
            CreateMap<Product, GetProductWithDetailsResponse>()
                .ForMember(dest => dest.PrimaryImage, opt => opt
                .MapFrom(src => src.Image != null ? src.Image.Path : null));
            CreateMap<PostProductRequest, Product>();
            CreateMap<PutProductRequest, Product>();
            CreateMap<ProductCategory, GetCategoryResponse>();
            CreateMap<Product, GetProductNamesResponse>();
            CreateMap<Product, CompanyProductDto>();
        }
    }
}
