using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Business.CQRS.Queries.OrderQueries
{
    public class GetByOrderNumberQuery : IRequest<ApiResponse<OrderResponse>>
    {
        public string OrderNumber { get; set; }

        public GetByOrderNumberQuery(string orderNumber)
        {
            OrderNumber = orderNumber;
        }
    }

    public class GetByOrderNumberQueryHandler : IRequestHandler<GetByOrderNumberQuery, ApiResponse<OrderResponse>>
    {
        private readonly IUnitOfWork<Order> _orderUnitOfWork;
        private readonly IMapper _mapper;

        public GetByOrderNumberQueryHandler(IUnitOfWork<Order> orderUnitOfWork, IMapper mapper)
        {
            _orderUnitOfWork = orderUnitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<OrderResponse>> Handle(GetByOrderNumberQuery request, CancellationToken cancellationToken)
        {
            var response = await _orderUnitOfWork.Repository.Where(x => x.OrderNumber == request.OrderNumber);
            var order = response.FirstOrDefault();
            if (order == null)
            {
                return new ApiResponse<OrderResponse>("Order not found");
            }
            else
            {
                return new ApiResponse<OrderResponse>(_mapper.Map<OrderResponse>(order));
            }
        }
    }
}
