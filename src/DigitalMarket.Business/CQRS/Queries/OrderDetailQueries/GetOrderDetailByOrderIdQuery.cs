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

namespace DigitalMarket.Business.CQRS.Queries.OrderDetailQueries
{
    public class GetOrderDetailByOrderIdQuery : IRequest<ApiResponse<List<OrderDetailResponse>>>
    {
        public long OrderId { get; set; }
    }

    public class GetOrderDetailByOrderIdQueryHandler : IRequestHandler<GetOrderDetailByOrderIdQuery, ApiResponse<List<OrderDetailResponse>>>
    {

        private readonly IUnitOfWork<OrderDetail> _orderDetailUnitOfWork;
        private readonly IMapper _mapper;

        public GetOrderDetailByOrderIdQueryHandler(IUnitOfWork<OrderDetail> orderDetailUnitOfWork, IMapper mapper)
        {
            _orderDetailUnitOfWork = orderDetailUnitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<OrderDetailResponse>>> Handle(GetOrderDetailByOrderIdQuery request, CancellationToken cancellationToken)
        {
            var response = await _orderDetailUnitOfWork.Repository.Where(x => x.OrderId == request.OrderId, "Product", "Product.ProductCategories");

            var mappedResponse = _mapper.Map<List<OrderDetailResponse>>(response);

            return new ApiResponse<List<OrderDetailResponse>>(mappedResponse);
        }
    }
}
