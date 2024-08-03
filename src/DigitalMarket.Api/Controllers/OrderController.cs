using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.OrderCommands;
using DigitalMarket.Business.CQRS.Queries.OrderQueries;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<OrderResponse>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllOrderQuery());
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<OrderResponse>> Get(long id)
        {
            var response = await _mediator.Send(new GetOrderByIdQuery(id));
            return response;
        }

        [HttpPost]
        public async Task<ApiResponse<OrderResponse>> Post(OrderRequest orderRequest)
        {
            var response = await _mediator.Send(new CreateOrderCommand(orderRequest));
            return response;
        }

        [HttpDelete]
        public async Task<ApiResponse> Delete(long id)
        {
            var response = await _mediator.Send(new DeleteOrderCommand { Id = id });
            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<OrderResponse>> Put(long id, [FromBody] OrderRequest orderRequest)
        {
            var response = await _mediator.Send(new UpdateOrderCommand(id, orderRequest));
            return response;
        }
    }
}
