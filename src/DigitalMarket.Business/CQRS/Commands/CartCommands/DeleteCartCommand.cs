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

        private readonly IConnectionMultiplexer _redis;

        public DeleteCartCommandHandler(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<ApiResponse> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {

            _redis.GetDatabase().KeyDelete($"cart:{request.UserId}");

            return new ApiResponse(true, "Cart deleted successfuly");
        }
    }
}
