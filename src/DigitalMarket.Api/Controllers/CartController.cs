using Azure.Core;
using DigitalMarket.Business.CQRS.Commands.CartCommands;
using DigitalMarket.Business.CQRS.Queries.CartQueries;
using DigitalMarket.Schema.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DigitalMarket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetCartByUserId([FromHeader] long userId)
        {
            var command = new GetCartByUserIdQuery(userId);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("SetCart")]
        public async Task<IActionResult> UpdateCart([FromHeader] long userId, [FromBody]AddProductToCartRequest request)
        {
            var command = new AddProductToCartCommand()
            {
                UserId = userId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("DeleteCart")]
        public async Task<IActionResult> DeleteCartByUserId([FromHeader] long userId)
        {
            var command = new DeleteCartCommand(userId);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("DeleteCartItemByProductId")]
        public async Task<IActionResult> DeleteCartItemByProductId([FromHeader] long userId, long productId)
        {
            var command = new DeleteCartItemByProductId()
            {
                UserId = userId,
                ProductId = productId
            };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

    }
}
