using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.Common;
using MixedDreams.Application.Constants;
using MixedDreams.Application.Exceptions;
using MixedDreams.Application.Extensions;
using MixedDreams.Application.Features.Errors;
using MixedDreams.Application.Features.OrderFeatures.GetOrder;
using MixedDreams.Application.Features.OrderFeatures.PostOrder;
using MixedDreams.Application.Features.OrderFeatures.UpdateOrderStatus;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Constants;
using System.Security.Claims;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize(Roles = Roles.Customer + "," + Roles.Company)]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<PostOrderRequest> _postOrderValidator;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<PostOrderRequest> postOrderValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _postOrderValidator = postOrderValidator;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrder(Guid id, CancellationToken cancellationToken)
        {
            Order? order = await _unitOfWork.OrderRepository.Get(id);
            if (order == null)
            {
                return NotFound(new EntityNotFoundResponse(nameof(Order), id.ToString()));
            }

            return Ok(_mapper.Map<GetOrderResponse>(order));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
        {
            IReadOnlyList<Order> Orders = await _unitOfWork.OrderRepository.GetAll(cancellationToken);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<GetOrderResponse>>(Orders));
        }

        [HttpPost]
        [Authorize(Roles = Roles.Customer)]
        public async Task<IActionResult> PostOrder([FromBody] PostOrderRequest model)
        {
            ValidationResult validationResult = await _postOrderValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }
            if (model.Products.Count > 50)
            {
                return BadRequest(new BadRequestResponse("Order is failed. Contact the seller for details", new List<string> { "You can't but more than 50 products. Contact the seller, If you wish to make a large order" }));
            }
            string userId = User.GetClaim(ClaimTypes.NameIdentifier) ?? throw new ClaimDoesntExistException(ClaimTypes.NameIdentifier);
            Guid customerId = await _unitOfWork.CustomerRepository.GetCustomerIdByUserIdAsync(userId) ?? throw new RelationCannotBeFoundException(nameof(ApplicationUser), nameof(Customer), userId);
            Order orderToCreate = _mapper.Map<Order>(model);
            orderToCreate.OrderStatus = Domain.Enums.OrderStatus.Accepted;
            orderToCreate.CustomerId = customerId;
            orderToCreate.OrderProducts = model.Products.Select(x => new OrderProduct
            {
                Amount = x.Amount,
                Order = orderToCreate,
                ProductId = x.ProductId,
                ProductHistoryId = _unitOfWork.OrderRepository.GetLastProductHistoryId(x.ProductId) ?? throw new InternalServerErrorException($"Product with id '{x.ProductId}' have no producthistory records")
            }).ToList();
            _unitOfWork.OrderRepository.Create(orderToCreate);
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = orderToCreate.Id }, _mapper.Map<GetOrderResponse>(orderToCreate));
        }

        [HttpPost("{id}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateOrderStatusRequest model)
        {
            Order? order = await _unitOfWork.OrderRepository.Get(id);
            if (order == null)
            {
                return NotFound(new EntityNotFoundResponse(nameof(Order), id.ToString()));
            }

            _unitOfWork.OrderRepository.Update(_mapper.Map(model, order));
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
