using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.Features.Errors;
using MixedDreams.Application.RepositoryInterfaces;
using MixedDreams.Domain.Entities;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/orders")]
    [ApiController]
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
            Order? Order = await _unitOfWork.OrderRepository.Get(id);
            if (Order == null)
            {
                return NotFound(new EntityNotFoundResponse(nameof(Order), id.ToString()));
            }

            return Ok(_mapper.Map<GetOrderResponse>(Order));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetOrders(CancellationToken cancellationToken)
        {
            IReadOnlyList<Order> Orders = await _unitOfWork.OrderRepository.GetAll(cancellationToken);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<GetOrderResponse>>(Orders));
        }

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] PostOrderRequest model)
        {
            ValidationResult validationResult = await _postOrderValidator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                ErrorsMaker.ProcessValidationErrors(validationResult.Errors);
            }
            if (await _unitOfWork.OrderRepository.IsNameTaken(model.Name))
            {
                return BadRequest(new PropertyIsTakenBadRequestResponse(nameof(model.Name), model.Name));
            }

            Order Order = _unitOfWork.OrderRepository.Create(_mapper.Map<Order>(model));
            await _unitOfWork.SaveAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = Order.Id }, _mapper.Map<GetOrderResponse>(Order));
        }
    }
}
