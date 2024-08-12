using Azure.Core;
using DigitalMarket.Business.CQRS.Commands.CartCommands;
using DigitalMarket.Business.CQRS.Queries.CartQueries;
using DigitalMarket.Schema.Request;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

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
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> GetCartByUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token");
            }
            long userId = long.Parse(userIdClaim.Value);

            var command = new GetCartByUserIdQuery(userId);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("SetCart")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> SetCart([FromBody]AddProductToCartRequest request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token");
            }
            long userId = long.Parse(userIdClaim.Value);

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
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> DeleteCartByUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token");
            }
            long userId = long.Parse(userIdClaim.Value);

            var command = new DeleteCartCommand(userId);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpDelete("DeleteCartItemByProductId")]
        [Authorize(Roles = "user,admin")]
        public async Task<IActionResult> DeleteCartItemByProductId(long productId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token");
            }
            long userId = long.Parse(userIdClaim.Value);

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
