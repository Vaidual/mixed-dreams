using AutoMapper;
using Microsoft.AspNetCore.Http;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Application.Features.ProductFeatures.PutProduct;
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

        public async Task<Product> CreateProductAsync(PostProductRequest model)
        {
            Product productToCreate = _mapper.Map<Product>(model);
            if (model.PrimaryImage is not null)
            {
                var image = await _backblazeService.UploadImage(model.PrimaryImage);
                productToCreate.Image = image;
            }
            Product result = await _unitOfWork.ProductRepository.CreateAsync(productToCreate); ;
            await _unitOfWork.SaveAsync();
            return result;
        }
        public async Task UpdateProductAsync(Product productToUpdate, PutProductRequest updateModel)
        {
            Product productToCreate = _mapper.Map(updateModel, productToUpdate);
            if (updateModel.ChangeImage)
            {
                if (productToUpdate.Image != null)
                {
                    await _backblazeService.DeleteImage(productToUpdate.Image.Id, productToUpdate.Image.FileName);
                    productToUpdate.Image = null;
                }
                if (updateModel.PrimaryImage != null)
                {
                    var image = await _backblazeService.UploadImage(updateModel.PrimaryImage);
                    productToCreate.Image = image;
                }
            }
            _unitOfWork.ProductRepository.Update(productToUpdate); ;
            await _unitOfWork.SaveAsync();
        }


        public async Task DeleteProductAsync(Product product)
        {
            if (product.Image != null)
            {
                await _backblazeService.DeleteImage(product.Image.Id, product.Image.FileName);
            }
            _unitOfWork.ProductRepository.Delete(product); ;
            await _unitOfWork.SaveAsync();
        }
    }
}
