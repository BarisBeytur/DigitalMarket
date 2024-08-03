using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;

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

        public GetAllCategoryQueryHandler(IUnitOfWork<Category> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<CategoryResponse>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository.GetAll();

            var mappedResult = _mapper.Map<IEnumerable<CategoryResponse>>(items);

            return new ApiResponse<IEnumerable<CategoryResponse>>(mappedResult);
        }
    }
}
