using AutoMapper;
using Microsoft.AspNetCore.Http;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.Exceptions.BadRequest;
using MixedDreams.Application.Exceptions.InternalServerError;
using MixedDreams.Application.Exceptions.NotFound;
using MixedDreams.Application.Features.ProductFeatures.PostPutProduct;
using MixedDreams.Application.Features.ProductFeatures.PutProduct;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.Hubs.Clients;
using MixedDreams.Domain.Entities;
using MixedDreams.Application.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Services
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

        public async Task<Product> CreateProductAsync(PostProductRequest model, ClaimsPrincipal user)
        {
            if (await _unitOfWork.ProductRepository.IsNameTaken(model.Name!))
            {
                throw new PropertyIsTakenException(nameof(model.Name), model.Name);
            }
            Product productToCreate = _mapper.Map<Product>(model);
            string userId = user.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            productToCreate.CompanyId = await _unitOfWork.CompanyRepository.GetCompanyIdByUserIdAsync(userId)
                ?? throw new InternalServerErrorException($"User with id {userId} doesn't connected to company however has company role ");
            if (model.PrimaryImage is not null)
            {
                var image = await _backblazeService.UploadImage(model.PrimaryImage);
                productToCreate.Image = image;
            }
            if (model.Ingredients is not null)
            {
                productToCreate.ProductIngredients = model.Ingredients
                .Select(x => new ProductIngredient
                {
                    HasAmount = x.HasAmount,
                    Amount = x.HasAmount ? x.Amount : null,
                    Unit = x.HasAmount ? x.Unit : null,
                    Product = productToCreate,
                    IngredientId = x.Id
                }).ToList();
            }
            
            Product result = _unitOfWork.ProductRepository.Create(productToCreate);
            await _unitOfWork.SaveAsync();
            return result;
        }
        public async Task UpdateProductAsync(Guid id, PutProductRequest updateModel)
        {
            Product productToUpdate = await _unitOfWork.ProductRepository.GetAsync(id) 
                ?? throw new EntityNotFoundException(nameof(Product), id.ToString());
            if (updateModel.Name != productToUpdate.Name && await _unitOfWork.ProductRepository.IsNameTaken(updateModel.Name!))
            {
                throw new PropertyIsTakenException(nameof(updateModel.Name), updateModel.Name);
            }

            bool shouldCreateProductHistory = ShouldCreateProductHistory(productToUpdate, updateModel);
            if (updateModel.ChangeImage && productToUpdate.Image != null  && (await _unitOfWork.ProductRepository.CountImageOccurrencesAsync(productToUpdate.Image.Path)) <= 1)
            {
                await _backblazeService.DeleteImage(productToUpdate.Image.Id, productToUpdate.Image.FileName);
                productToUpdate.Image = null;
            }

            _mapper.Map(updateModel, productToUpdate);
            if (updateModel.PrimaryImage != null && updateModel.ChangeImage)
            {
                productToUpdate.Image = await _backblazeService.UploadImage(updateModel.PrimaryImage);
            }
            _unitOfWork.ProductRepository.ClearProductIngredients(productToUpdate.Id);
            if (updateModel.Ingredients is not null)
            {
                productToUpdate.ProductIngredients = updateModel.Ingredients
                .Select(x => new ProductIngredient
                {
                    HasAmount = x.HasAmount,
                    Amount = x.HasAmount ? x.Amount : null,
                    Unit = x.HasAmount ? x.Unit : null,
                    Product = productToUpdate,
                    IngredientId = x.Id
                }).ToList();
            }
            _unitOfWork.ProductRepository.Update(productToUpdate);

            if (shouldCreateProductHistory)
            {
                _unitOfWork.ProductRepository.CreateProductHistory(productToUpdate);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            if (product.Image != null && (await _unitOfWork.ProductRepository.CountImageOccurrencesAsync(product.Image.Path)) <= 1)
            {
                await _backblazeService.DeleteImage(product.Image.Id, product.Image.FileName);
            }
            _unitOfWork.ProductRepository.Delete(product); ;
            await _unitOfWork.SaveAsync();
        }

        private bool ShouldCreateProductHistory(Product oldProduct, PutProductRequest newProduct)
        {
            return oldProduct.Name != newProduct.Name || oldProduct.Price != newProduct.Price;
        }
    }
}
