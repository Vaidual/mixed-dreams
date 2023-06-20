using AutoMapper;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.Exceptions.BadRequest;
using MixedDreams.Application.Exceptions.InternalServerError;
using MixedDreams.Application.Exceptions.NotFound;
using MixedDreams.Application.Extensions;
using MixedDreams.Application.Features.Errors;
using MixedDreams.Application.Features.OrderFeatures.PostOrder;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Application.Hubs.Clients;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Bytewizer.Backblaze.Extensions;
using MixedDreams.Application.Features.OrderFeatures.OrderProduct;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MixedDreams.Application.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Order> PlaceOrderAsync(PostOrderRequest model, ClaimsPrincipal user)
        {
            if (model.Products.Count > 50)
            {
                throw new LargeOrderException();
            }
            string userId = user.GetClaim(ClaimTypes.NameIdentifier) ?? throw new ClaimDoesntExistException(ClaimTypes.NameIdentifier);
            Guid customerId = await _unitOfWork.CustomerRepository.GetCustomerIdByUserIdAsync(userId) ?? throw new RelationCannotBeFoundException(nameof(ApplicationUser), nameof(Customer), userId);
            Order orderToCreate = _mapper.Map<Order>(model);
            orderToCreate.OrderStatus = Domain.Enums.OrderStatus.Accepted;
            orderToCreate.CustomerId = customerId;
            orderToCreate.TenantId = (await _unitOfWork.BusinessLocationRepository.GetAsync(model.BusinessLocationId))?.TenantId ?? throw new EntityNotFoundException(nameof(BusinessLocation), model.BusinessLocationId.ToString());

            List<OrderProductWithPreparationDto> orderProducts = await _unitOfWork.ProductRepository.AddPreparationTimeToOrderProducts(model.Products);
            OrderProduct[] orderProductsToCreate = new OrderProduct[model.Products.Count];
            IReadOnlyList<Cook> cooks = await _unitOfWork.CookRepository.GetAllAsync();
            for (int i = 0; i < orderProducts.Count; i++)
            {
                OrderProductWithPreparationDto orderProduct = orderProducts[i];
                var orderProductToAdd = new OrderProduct()
                {
                    Amount = orderProduct.Amount,
                    Order = orderToCreate,
                    ProductId = orderProduct.ProductId,
                    ProductHistoryId = await _unitOfWork.ProductHistoryRepository.GetLastProductHistoryId(orderProduct.ProductId)
                        ?? throw new ProductHasNoProductHistoryException(orderProduct.ProductId.ToString()),
                };
                orderProductsToCreate[i] = orderProductToAdd;

                for (int j = 0; j < orderProduct.Amount; j++)
                {
                    ProductPreparation newProductPreparation = new ProductPreparation()
                    {
                        OrderProduct = orderProductToAdd,
                    };
                    Cook? cook = cooks.All(x => x.LastEndTime.HasValue) ? cooks.MinBy(x => x.LastEndTime) : cooks.First(x => x.LastEndTime == null);
                    if (cook is not null)
                    {
                        if (cook.LastEndTime == null)
                        {
                            var currentDateTime = DateTimeOffset.Now;
                            var endDateTime = currentDateTime.AddMinutes(orderProduct.PreparationTime);
                            cook.CurrentEndTime = endDateTime;
                            cook.CurrentProductPreparation = newProductPreparation;
                        }
                        else
                        {
                            var endDateTime = cook.LastEndTime?.AddMinutes(orderProduct.PreparationTime);
                            cook.LastEndTime = endDateTime;
                        }
                        _unitOfWork.CookRepository.Update(cook);
                    }
                    _unitOfWork.ProductPreparationRepository.Create(newProductPreparation);
                }
            }
            
            orderToCreate.OrderProducts = orderProductsToCreate.ToList();
            _unitOfWork.OrderRepository.Create(orderToCreate);
            await _unitOfWork.SaveAsync();
            Order createdOrder = await _unitOfWork.OrderRepository.GetAsync(orderToCreate.Id)
                ?? throw new EntityNotFoundException(nameof(Order), orderToCreate.Id.ToString());

            return createdOrder;
        }



        public async Task NotifyAboutLowWater(Guid deviceId)
        {

        }
    }
}
