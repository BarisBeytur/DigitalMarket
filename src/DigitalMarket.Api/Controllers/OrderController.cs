using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.OrderCommands;
using DigitalMarket.Business.CQRS.Queries.OrderQueries;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DigitalMarket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<IEnumerable<OrderResponse>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllOrderQuery());
            return response;
        }

        [HttpGet("GetByOrderNumber")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<OrderResponse>> GetByOrderNumber([FromQuery]string orderNumber)
        {
            var response = await _mediator.Send(new GetByOrderNumberQuery(orderNumber));
            return response;
        }

        [HttpGet("GetActiveOrders")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<List<OrderResponse>>> GetActiveOrders()
        {
            var response = await _mediator.Send(new GetActiveOrderQuery());
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<OrderResponse>> Get(long id)
        {
            var response = await _mediator.Send(new GetOrderByIdQuery(id));
            return response;
        }

        [HttpPost]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<OrderResponse>> Post([FromHeader] string NameSurname, [FromHeader] string CardNo, [FromHeader] string ExpirationDate, [FromHeader] string CVV, string couponCode)
        {
            var paymentRequest = new PaymentRequest
            {
                NameSurname = NameSurname,
                CardNo = CardNo,
                ExpirationDate = ExpirationDate,
                CVV = CVV
            };

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return new ApiResponse<OrderResponse>("User not found");
            }

            long userId = long.Parse(userIdClaim.Value);
            var orderRequest = new OrderRequest { UserId = userId, CouponCode = couponCode};

            var response = await _mediator.Send(new CreateOrderCommand(orderRequest, paymentRequest));
            return response;
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> Delete(long id)
        {
            var response = await _mediator.Send(new DeleteOrderCommand { Id = id });
            return response;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<OrderResponse>> Put(long id, [FromBody] OrderRequest orderRequest)
        {
            var response = await _mediator.Send(new UpdateOrderCommand(id, orderRequest));
            return response;
        }
    }
}
