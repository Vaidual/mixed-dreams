using AutoMapper;
using Microsoft.AspNetCore.Http;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Services
{
    internal class ProductService : IProductService
    {
        private readonly IBackblazeService _backblazeService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IBackblazeService backblazeService, IUnitOfWork unitOfWork)
        {
            _backblazeService = backblazeService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Product> CreateProductAsync(PostPutProductRequest model, IFormFile? image)
        {
            Product productToCreate = _mapper.Map<Product>(model);
            if (image is not null)
            {
                string imageUrl = await _backblazeService.UploadImage(image);
                productToCreate.PrimaryImage = imageUrl;
            }
            Product result = await _unitOfWork.ProductRepository.CreateAsync(productToCreate); ;
            await _unitOfWork.SaveAsync();
            return result;
        }
    }
}
