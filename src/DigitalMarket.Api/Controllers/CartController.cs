using Azure.Core;
using DigitalMarket.Business.CQRS.Commands.CartCommands;
using DigitalMarket.Business.CQRS.Queries.CartQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCartByUserId(long userId)
        {
            var cartKey = $"cart:{userId}";
            var command = new GetCartByUserIdQuery(cartKey);
            var response = await _mediator.Send(command);  // Use await here
            return Ok(response);
        }

        [HttpPost]
        public IActionResult AddProductToCart(AddProductToCartCommand command)
        {
            _mediator.Send(command);
            return Ok();
        }

    }
}
