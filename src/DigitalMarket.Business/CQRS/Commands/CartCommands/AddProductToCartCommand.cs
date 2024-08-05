using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
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
    private readonly IUnitOfWork<Product> _productUnitOfWork;

    public AddProductToCartCommandHandler(IConnectionMultiplexer redis, IUnitOfWork<Product> productUnitOfWork)
    {
        _redis = redis;
        _productUnitOfWork = productUnitOfWork;
    }

    public async Task<ApiResponse> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();
        var cartKey = $"cart:{request.UserId}";

        var product = await _productUnitOfWork.Repository.GetById(request.ProductId);

        if (product == null)
        {
            return new ApiResponse("Product not found");
        }

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
            await db.HashIncrementAsync(cartKey, request.ProductId.ToString(), request.Quantity);
            return new ApiResponse("Cart successfully updated");
        }

    }
}
