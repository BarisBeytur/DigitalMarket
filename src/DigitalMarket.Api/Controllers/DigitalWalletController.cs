using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.DigitalWalletCommands;
using DigitalMarket.Business.CQRS.Queries.DigitalWalletQueries;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DigitalMarket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DigitalWalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DigitalWalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<IEnumerable<DigitalWalletResponse>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllDigitalWalletQuery());
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<DigitalWalletResponse>> Get(long id)
        {
            var response = await _mediator.Send(new GetDigitalWalletByIdQuery(id));
            return response;
        }

        [HttpGet("pointbalance")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<PointBalanceResponse>> GetPointBalance()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return new ApiResponse<PointBalanceResponse>("User Id claim not found");
            }
            long userId = long.Parse(userIdClaim.Value);
            var response = await _mediator.Send(new GetPointBalanceQuery { UserId = userId });
            return response;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<DigitalWalletResponse>> Put(long id, [FromBody] decimal pointBalance)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return new ApiResponse<DigitalWalletResponse>("User not found");
            }
            long userId = long.Parse(userIdClaim.Value);

            var digitalWalletRequest = new DigitalWalletRequest
            {
                UserId = userId,
                PointBalance = pointBalance
            };

            var response = await _mediator.Send(new UpdateDigitalWalletCommand(id, digitalWalletRequest));
            return response;
        }
    }
}
