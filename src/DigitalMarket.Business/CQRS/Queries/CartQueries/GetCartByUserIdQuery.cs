using DigitalMarket.Base.Response;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using StackExchange.Redis;
using System.Globalization;

namespace DigitalMarket.Business.CQRS.Queries.CartQueries
{
    public class GetCartByUserIdQuery : IRequest<Dictionary<string, string>>
    {
        public string Key { get; set; }

        public GetCartByUserIdQuery(string key)
        {
            Key = key;
        }
    }

    public class GetCartByUserIdQueryHandler : IRequestHandler<GetCartByUserIdQuery, Dictionary<string, string>>
    {
        private readonly IConnectionMultiplexer _redis;

        public GetCartByUserIdQueryHandler(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<Dictionary<string, string>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
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

            // Convert to a dictionary
            var result = hashEntries.ToDictionary(
                entry => entry.Name.ToString(),
                entry => entry.Value.ToString()
            );

            return result;
        }
    }
}
