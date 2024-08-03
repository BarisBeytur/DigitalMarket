using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.UserCommands;
using DigitalMarket.Business.CQRS.Commands.UserCommands;
using DigitalMarket.Business.CQRS.Queries.UserQueries;
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

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<UserResponse>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllUserQuery());
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<UserResponse>> Get(long id)
        {
            var response = await _mediator.Send(new GetUserByIdQuery(id));
            return response;
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
