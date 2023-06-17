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
using Bytewizer.Backblaze.Extensions;
using MixedDreams.Infrastructure.Features.OrderFeatures.OrderProduct;

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
                ?? throw new EntityNotFoundException(nameof(BusinessLocation), model.BusinessLocationId.ToString()));

            OrderProduct[] orderProducts = new OrderProduct[model.Products.Count];
            for (int i = 0; i < model.Products.Count; i++)
            {
                PostOrderProductDto orderProduct = model.Products[i];
                orderProducts[i] = new OrderProduct()
                {
                    Amount = orderProduct.Amount,
                    Order = orderToCreate,
                    ProductId = orderProduct.ProductId,
                    ProductHistoryId = await _unitOfWork.ProductHistoryRepository.GetLastProductHistoryId(orderProduct.ProductId) 
                        ?? throw new ProductHasNoProductHistoryException(orderProduct.ProductId.ToString())
                };
            }
            
            orderToCreate.OrderProducts = orderProducts.ToList();
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
