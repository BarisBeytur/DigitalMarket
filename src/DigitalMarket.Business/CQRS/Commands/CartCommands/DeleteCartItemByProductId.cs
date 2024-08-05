using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using MediatR;
using StackExchange.Redis;

namespace DigitalMarket.Business.CQRS.Commands.CartCommands
{
    public class DeleteCartItemByProductId : IRequest<ApiResponse>
    {
        public long UserId { get; set; }
        public long ProductId { get; set; }
    }

    public class DeleteCartItemByProductIdHandler : IRequestHandler<DeleteCartItemByProductId, ApiResponse>
    {
        private readonly IConnectionMultiplexer _redis;

        public DeleteCartItemByProductIdHandler(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<ApiResponse> Handle(DeleteCartItemByProductId request, CancellationToken cancellationToken)
        {
            var db = _redis.GetDatabase();

            var cartKey = $"cart:{request.UserId}";
            var cartItems = await db.HashGetAllAsync(cartKey);

            if (cartItems.Length == 0)
            {
                return new ApiResponse("Cart is empty or does not exist.");
            }

            RedisValue productKey = cartItems.FirstOrDefault(x => x.Name == request.ProductId.ToString()).Name;

            if (!productKey.HasValue)
            {
                return new ApiResponse("Product not found in cart.");
            }

            await db.HashDeleteAsync(cartKey, productKey);

            return new ApiResponse(true, "Product removed from cart");
        }
    }

}
