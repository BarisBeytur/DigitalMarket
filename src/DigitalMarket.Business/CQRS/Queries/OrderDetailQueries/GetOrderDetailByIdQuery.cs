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
    public class GetOrderDetailByIdQuery : IRequest<ApiResponse<OrderDetailResponse>>
    {
        public long Id { get; set; }

        public GetOrderDetailByIdQuery(long id)
        {
            Id = id;
        }
    }

    public class GetOrderDetailByIdQueryHandler : IRequestHandler<GetOrderDetailByIdQuery, ApiResponse<OrderDetailResponse>>
    {

        private readonly IUnitOfWork<OrderDetail> _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderDetailByIdQueryHandler(IUnitOfWork<OrderDetail> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<OrderDetailResponse>> Handle(GetOrderDetailByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository.GetById(query.Id);

            if (result == null)
            {
                return new ApiResponse<OrderDetailResponse>("Item not found");
            }

            var mappedResult = _mapper.Map<OrderDetailResponse>(result);

            return new ApiResponse<OrderDetailResponse>(mappedResult);

        }
    }
}
