using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;
using StackExchange.Redis;
using System.Globalization;

namespace DigitalMarket.Business.CQRS.Queries.CartQueries;

public class GetCartByUserIdQuery : IRequest<ApiResponse<IEnumerable<CartResponse>>>
{
    public long UserId { get; set; }

    public GetCartByUserIdQuery(long userId)
    {
        UserId = userId;
    }
}

public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, ApiResponse<IEnumerable<CartResponse>>>
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IUnitOfWork<User> _userUnitOfWork;
    private readonly IUnitOfWork<Product> _productUnitOfWork;

    public GetCartByUserIdQueryHandler(IConnectionMultiplexer redis, IUnitOfWork<User> userUnitOfWork, IUnitOfWork<Product> productUnitOfWork)
    {
        _redis = redis;
        _userUnitOfWork = userUnitOfWork;
        _productUnitOfWork = productUnitOfWork;
    }

    public async Task<ApiResponse<IEnumerable<CartResponse>>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userUnitOfWork.Repository.GetById(request.UserId);

        if (user == null)
        {
            return new ApiResponse<IEnumerable<CartResponse>>("User not found");
        }

        var cartKey = $"cart:{request.UserId}";

        var db = _redis.GetDatabase();
        var hashEntries = await db.HashGetAllAsync(cartKey);

        var result = new List<CartResponse>();

        foreach (var entry in hashEntries)
        {
            var productId = long.Parse(entry.Name.ToString(), CultureInfo.InvariantCulture);
            var product = await _productUnitOfWork.Repository.GetById(productId);

            result.Add(new CartResponse
            {
                ProductId = productId,
                ProductName = product?.Name ?? "Unknown",
                Price = product?.Price ?? 0,
                Quantity = int.Parse(entry.Value.ToString(), CultureInfo.InvariantCulture),
            });
        }

        return new ApiResponse<IEnumerable<CartResponse>>(result);
    }
}
