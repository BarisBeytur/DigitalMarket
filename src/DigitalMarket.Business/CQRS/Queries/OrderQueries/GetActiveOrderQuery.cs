using AutoMapper;
using DigitalMarket.Base.Enums;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Queries.OrderQueries
{
    public class GetActiveOrderQuery : IRequest<ApiResponse<List<OrderResponse>>>
    {

    }


    public class GetActiveOrderQueryHandler : IRequestHandler<GetActiveOrderQuery, ApiResponse<List<OrderResponse>>>
    {

        private readonly IUnitOfWork<Order> _orderUnitOfWork;
        private readonly IMapper _mapper;

        public GetActiveOrderQueryHandler(IUnitOfWork<Order> orderUnitOfWork, IMapper mapper)
        {
            _orderUnitOfWork = orderUnitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<OrderResponse>>> Handle(GetActiveOrderQuery request, CancellationToken cancellationToken)
        {
            var activeOrderList = await _orderUnitOfWork.Repository.Where(x => x.Status != (short)(Enums.OrderStatus.Delivered) 
            && x.Status != (short)(Enums.OrderStatus.Canceled)
            && x.Status != (short)(Enums.OrderStatus.Rejected));

            var mappedList = _mapper.Map<List<OrderResponse>>(activeOrderList);

            return new ApiResponse<List<OrderResponse>>(mappedList);

        }
    }
}
