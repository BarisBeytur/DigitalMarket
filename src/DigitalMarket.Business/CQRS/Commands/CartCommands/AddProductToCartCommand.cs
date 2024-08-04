using DigitalMarket.Base.Response;
using MediatR;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace DigitalMarket.Business.CQRS.Commands.CartCommands
{
    public class AddProductToCartCommand : IRequest<ApiResponse>
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public int Quantity { get; set; }

        public AddProductToCartCommand(long userId, long productId, int quantity)
        {
            UserId = userId;
            ProductId = productId;
            Quantity = quantity;
        }
    }

    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand, ApiResponse>
    {

        private readonly IConfiguration _configuration;
        private readonly IConnectionMultiplexer _redis;


        public AddProductToCartCommandHandler(IConfiguration configuration, IConnectionMultiplexer redis)
        {
            _configuration = configuration;
            _redis = redis;
        }

        public async Task<ApiResponse> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {

            var cartKey = $"cart:{request.UserId}";

            var db = _redis.GetDatabase();

            await db.HashSetAsync(cartKey, request.ProductId.ToString(), request.Quantity);

            return new ApiResponse();
        }
    }
}
