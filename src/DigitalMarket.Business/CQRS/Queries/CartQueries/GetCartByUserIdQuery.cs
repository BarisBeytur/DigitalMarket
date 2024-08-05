using DigitalMarket.Base.Response;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Globalization;

namespace DigitalMarket.Business.CQRS.Queries.CartQueries;

public class GetCartByUserIdQuery : IRequest<ApiResponse<IEnumerable<CartResponse>>>
{
    public string Key { get; set; }

    public GetCartByUserIdQuery(string key)
    {
        Key = key;
    }
}

public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, ApiResponse<IEnumerable<CartResponse>>>
{
    private readonly IConnectionMultiplexer _redis;

    public GetCartByUserIdQueryHandler(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<ApiResponse<IEnumerable<CartResponse>>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
    {
        var db = _redis.GetDatabase();

        // Check if the key is a hash
        var keyType = await db.KeyTypeAsync(request.Key);
        if (keyType != RedisType.Hash)
        {
            throw new InvalidOperationException("The key type is not a hash.");
        }

        // Retrieve all fields and values from the hash
        var hashEntries = await db.HashGetAllAsync(request.Key);

        // Convert to a list of CartResponse
        var result = hashEntries.Select(x => new CartResponse
        {
            Quantity = int.Parse(x.Value.ToString(), CultureInfo.InvariantCulture),
            ProductId = long.Parse(x.Name.ToString(), CultureInfo.InvariantCulture)
        }).ToList();

        return new ApiResponse<IEnumerable<CartResponse>>(result);
    }
}
