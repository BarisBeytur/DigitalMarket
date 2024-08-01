using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.UserCommands;
using DigitalMarket.Business.CQRS.Commands.UserCommands;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ApiResponse<UserResponse>> Post(UserRequest userRequest)
        {
            var response = await _mediator.Send(new CreateUserCommand(userRequest));
            return response;
        }

        [HttpDelete]
        public async Task<ApiResponse> Delete(long id)
        {
            var response = await _mediator.Send(new DeleteUserCommand { Id = id });
            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<UserResponse>> Put(long id, [FromBody] UserRequest couponRequest)
        {
            var response = await _mediator.Send(new UpdateUserCommand(id, couponRequest));
            return response;
        }
    }
}
