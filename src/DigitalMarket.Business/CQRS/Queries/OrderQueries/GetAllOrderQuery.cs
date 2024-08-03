using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Queries.OrderQueries
{
    public class GetAllOrderQuery : IRequest<ApiResponse<IEnumerable<OrderResponse>>>
    {
        public GetAllOrderQuery() { }
    }

    public class GetAllOrderQueryHandler : IRequestHandler<GetAllOrderQuery, ApiResponse<IEnumerable<OrderResponse>>>
    {
        private readonly IUnitOfWork<Order> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllOrderQueryHandler(IUnitOfWork<Order> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<OrderResponse>>> Handle(GetAllOrderQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository.GetAll();

            var mappedResult = _mapper.Map<IEnumerable<OrderResponse>>(items);

            return new ApiResponse<IEnumerable<OrderResponse>>(mappedResult);
        }
    }
}
