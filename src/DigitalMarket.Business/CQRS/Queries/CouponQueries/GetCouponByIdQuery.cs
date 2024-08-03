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

namespace DigitalMarket.Business.CQRS.Queries.CouponQueries
{
    public class GetCouponByIdQuery : IRequest<ApiResponse<CouponResponse>>
    {
        public long Id { get; set; }

        public GetCouponByIdQuery(long id)
        {
            Id = id;
        }
    }

    public class GetCouponByIdQueryHandler : IRequestHandler<GetCouponByIdQuery, ApiResponse<CouponResponse>>
    {

        private readonly IUnitOfWork<Coupon> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCouponByIdQueryHandler(IUnitOfWork<Coupon> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CouponResponse>> Handle(GetCouponByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository.GetById(query.Id);

            if (result == null)
            {
                return new ApiResponse<CouponResponse>("Item not found");
            }

            var mappedResult = _mapper.Map<CouponResponse>(result);

            return new ApiResponse<CouponResponse>(mappedResult);

        }
    }
}
