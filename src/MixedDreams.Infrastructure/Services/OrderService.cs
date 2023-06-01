using AutoMapper;
using MixedDreams.Infrastructure.Exceptions;
using MixedDreams.Infrastructure.Exceptions.BadRequest;
using MixedDreams.Infrastructure.Exceptions.InternalServerError;
using MixedDreams.Infrastructure.Exceptions.NotFound;
using MixedDreams.Infrastructure.Extensions;
using MixedDreams.Infrastructure.Features.Errors;
using MixedDreams.Infrastructure.Features.OrderFeatures.PostOrder;
using MixedDreams.Infrastructure.RepositoryInterfaces;
using MixedDreams.Infrastructure.Hubs.Clients;
using MixedDreams.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Infrastructure.Services
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
            orderToCreate.TenantId = Guid.Parse(_unitOfWork.CompanyRepository
                .GettenantIdByBusinessLocationIdAsync(model.BusinessLocationId) 
                ?? throw new EntityNotFoundException(
                    nameof(BusinessLocation), 
                    model.BusinessLocationId.ToString()));
            var orderProducts = await Task.WhenAll(model.Products.Select(async x => new OrderProduct
            {
                Amount = x.Amount,
                Order = orderToCreate,
                ProductId = x.ProductId,
                ProductHistoryId = await _unitOfWork.ProductHistoryRepository.GetLastProductHistoryId(x.ProductId) ?? throw new ProductHasNoProductHistoryException(x.ProductId.ToString())
            }));
            orderToCreate.OrderProducts = orderProducts.ToList();
            _unitOfWork.OrderRepository.Create(orderToCreate);
            await _unitOfWork.SaveAsync();
            Order createdOrder = await _unitOfWork.OrderRepository.Get(orderToCreate.Id)
                ?? throw new EntityNotFoundException(nameof(Order), orderToCreate.Id.ToString());

            return createdOrder;
        }

        public async Task NotifyAboutLowWater(Guid deviceId)
        {

        }
    }
}
