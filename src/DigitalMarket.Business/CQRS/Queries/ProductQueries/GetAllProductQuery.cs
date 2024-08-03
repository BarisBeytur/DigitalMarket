using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Queries.ProductQueries
{
    public class GetAllProductQuery : IRequest<ApiResponse<IEnumerable<ProductResponse>>>
    {
        public GetAllProductQuery() { }
    }

    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, ApiResponse<IEnumerable<ProductResponse>>>
    {
        private readonly IUnitOfWork<Product> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductQueryHandler(IUnitOfWork<Product> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<ProductResponse>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository.GetAll();

            var mappedResult = _mapper.Map<IEnumerable<ProductResponse>>(items);

            return new ApiResponse<IEnumerable<ProductResponse>>(mappedResult);
        }
    }
}
