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
    public class GetOrderByIdQuery : IRequest<ApiResponse<OrderResponse>>
    {
        public long Id { get; set; }

        public GetOrderByIdQuery(long id)
        {
            Id = id;
        }
    }

    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiResponse<OrderResponse>>
    {

        private readonly IUnitOfWork<Order> _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderByIdQueryHandler(IUnitOfWork<Order> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<OrderResponse>> Handle(GetOrderByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository.GetById(query.Id);

            if (result == null)
            {
                return new ApiResponse<OrderResponse>("Item not found");
            }

            var mappedResult = _mapper.Map<OrderResponse>(result);

            return new ApiResponse<OrderResponse>(mappedResult);

        }
    }
}
