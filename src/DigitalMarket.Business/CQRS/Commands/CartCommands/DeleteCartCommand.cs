using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using MediatR;
using StackExchange.Redis;

namespace DigitalMarket.Business.CQRS.Commands.CartCommands
{
    public class DeleteCartCommand : IRequest<ApiResponse>
    {
        public long UserId { get; set; }

        public DeleteCartCommand(long userId)
        {
            UserId = userId;
        }
    }

    public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, ApiResponse>
    {

        private readonly IUnitOfWork<User> _userUnitOfWork;
        private readonly IConnectionMultiplexer _redis;

        public DeleteCartCommandHandler(IUnitOfWork<User> userUnitOfWork, IConnectionMultiplexer redis)
        {
            _userUnitOfWork = userUnitOfWork;
            _redis = redis;
        }

        public async Task<ApiResponse> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            var user = await _userUnitOfWork.Repository.GetById(request.UserId);

            if (user is null)
            {
                return new ApiResponse("User not found");
            }

            _redis.GetDatabase().KeyDelete($"cart:{request.UserId}");

            return new ApiResponse(true, "Cart deleted successfuly");
        }
    }
}
