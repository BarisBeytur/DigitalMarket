using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Queries.OrderDetailQueries
{
    public class GetAllOrderDetailQuery : IRequest<ApiResponse<IEnumerable<OrderDetailResponse>>>
    {
        public GetAllOrderDetailQuery() { }
    }

    public class GetAllOrderDetailQueryHandler : IRequestHandler<GetAllOrderDetailQuery, ApiResponse<IEnumerable<OrderDetailResponse>>>
    {
        private readonly IUnitOfWork<OrderDetail> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOrderDetailQueryHandler(IUnitOfWork<OrderDetail> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<OrderDetailResponse>>> Handle(GetAllOrderDetailQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository.GetAll("Product", "Product.ProductCategories");

            var mappedResult = _mapper.Map<IEnumerable<OrderDetailResponse>>(items);

            return new ApiResponse<IEnumerable<OrderDetailResponse>>(mappedResult);
        }
    }
}
