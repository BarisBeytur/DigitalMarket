using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace DigitalMarket.Business.CQRS.Queries.CategoryQueries
{
    public class GetAllCategoryQuery : IRequest<ApiResponse<IEnumerable<CategoryResponse>>>
    {
        public GetAllCategoryQuery() { }
    }

    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, ApiResponse<IEnumerable<CategoryResponse>>>
    {
        private readonly IUnitOfWork<Category> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IDistributedCache distributedCache;


        public GetAllCategoryQueryHandler(IUnitOfWork<Category> unitOfWork, IMapper mapper, IDistributedCache distributedCache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            this.distributedCache = distributedCache;
        }

        public async Task<ApiResponse<IEnumerable<CategoryResponse>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var checkResult = await distributedCache.GetAsync("categoryList");
            if (checkResult != null)
            {
                string json = Encoding.UTF8.GetString(checkResult);
                var responseObj = JsonConvert.DeserializeObject<List<CategoryResponse>>(json);
                return new ApiResponse<IEnumerable<CategoryResponse>>(responseObj);
            }

            var items = await _unitOfWork.Repository.GetAll();

            var mappedResult = _mapper.Map<IEnumerable<CategoryResponse>>(items);

            var response = new ApiResponse<IEnumerable<CategoryResponse>>(mappedResult);

            if (items.Any())
            {
                string responseStr = JsonConvert.SerializeObject(response.Data);
                byte[] responseArr = Encoding.UTF8.GetBytes(responseStr);
                var cacheOptions = new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    SlidingExpiration = TimeSpan.FromHours(1)
                };
                await distributedCache.SetAsync("categoryList", responseArr, cacheOptions);
            }

            return response;
        }
    }
}
