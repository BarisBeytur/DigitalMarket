using Azure.Core;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
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

    public GetCartByUserIdQueryHandler(IConnectionMultiplexer redis, IUnitOfWork<User> userUnitOfWork)
    {
        _redis = redis;
        _userUnitOfWork = userUnitOfWork;
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

        var result = hashEntries.Select(x => new CartResponse
        {
            Quantity = int.Parse(x.Value.ToString(), CultureInfo.InvariantCulture),
            ProductId = long.Parse(x.Name.ToString(), CultureInfo.InvariantCulture)
        }).ToList();

        return new ApiResponse<IEnumerable<CartResponse>>(result);
    }
}
