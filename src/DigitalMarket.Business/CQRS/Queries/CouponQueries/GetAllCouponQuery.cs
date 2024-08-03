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
    public class GetAllCouponQuery : IRequest<ApiResponse<IEnumerable<CouponResponse>>>
    {
        public GetAllCouponQuery() { }
    }

    public class GetAllCouponQueryHandler : IRequestHandler<GetAllCouponQuery, ApiResponse<IEnumerable<CouponResponse>>>
    {
        private readonly IUnitOfWork<Coupon> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllCouponQueryHandler(IUnitOfWork<Coupon> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<CouponResponse>>> Handle(GetAllCouponQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository.GetAll();

            var mappedResult = _mapper.Map<IEnumerable<CouponResponse>>(items);

            return new ApiResponse<IEnumerable<CouponResponse>>(mappedResult);
        }
    }

}
