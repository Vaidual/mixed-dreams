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
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Domain.Entities;
using MixedDreams.Infrastructure.Constants;
using System.Security.Claims;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<PostOrderRequest> _postOrderValidator;
        private readonly IOrderService _orderService;

        public OrderController(IUnitOfWork unitOfWork, IMapper mapper, IValidator<PostOrderRequest> postOrderValidator, IOrderService orderService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _postOrderValidator = postOrderValidator;
            _orderService = orderService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetOrder(Guid id, CancellationToken cancellationToken)
        {
            Order order = await _unitOfWork.OrderRepository.Get(id, cancellationToken) ?? throw new EntityNotFoundException(nameof(Order), id.ToString());
            
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
            Order order = await _orderService.PlaceOrderAsync(model, User);

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, _mapper.Map<GetOrderResponse>(order));
        }

        [HttpPost("{id}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] Guid id, [FromBody] UpdateOrderStatusRequest model)
        {
            Order order = await _unitOfWork.OrderRepository.Get(id) ?? throw new EntityNotFoundException(nameof(Order), id.ToString());
            _unitOfWork.OrderRepository.Update(_mapper.Map(model, order));
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
