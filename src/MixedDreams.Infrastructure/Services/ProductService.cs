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

        public ProductService(IBackblazeService backblazeService, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _backblazeService = backblazeService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Product> CreateProductAsync(PostPutProductRequest model)
        {
            Product productToCreate = _mapper.Map<Product>(model);
            if (model.PrimaryImage is not null)
            {
                string imageUrl = await _backblazeService.UploadImage(model.PrimaryImage);
                productToCreate.PrimaryImage = imageUrl;
            }
            Product result = await _unitOfWork.ProductRepository.CreateAsync(productToCreate); ;
            await _unitOfWork.SaveAsync();
            return result;
        }
    }
}
