using DigitalMarket.Base.Response;
using MediatR;
using StackExchange.Redis;

namespace DigitalMarket.Business.CQRS.Commands.CartCommands;

public class AddProductToCartCommand : IRequest<ApiResponse>
{
    public long UserId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand, ApiResponse>
{
    private readonly IConnectionMultiplexer _redis;

    public AddProductToCartCommandHandler(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<ApiResponse> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var cartKey = $"cart:{request.UserId}";

        // Check if cart exists, if not create it
        var cart = await db.HashGetAllAsync(cartKey);
        if (cart.Length == 0)
        {
            await db.HashSetAsync(cartKey, new HashEntry[]
            {
                new HashEntry(request.ProductId.ToString(), request.Quantity)
            });

            return new ApiResponse("Cart successfully created");
        }
        else
        {
            // Update existing cart
            await db.HashIncrementAsync(cartKey, request.ProductId.ToString(), request.Quantity);
            return new ApiResponse("Cart successfully updated");

        }

    }
}
