using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.CouponCommands;

public class CreateCouponCommand : IRequest<ApiResponse<CouponResponse>>
{
    public CouponRequest CouponRequest { get; set; }

    public CreateCouponCommand(CouponRequest couponRequest)
    {
        CouponRequest = couponRequest;
    }
}

public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, ApiResponse<CouponResponse>>
{

    private readonly IUnitOfWork<Coupon> _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCouponCommandHandler(IUnitOfWork<Coupon> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CouponResponse>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Coupon>(request.CouponRequest);

        await _unitOfWork.Repository.Insert(item);

        await _unitOfWork.Commit();

        return new ApiResponse<CouponResponse>(_mapper.Map<CouponResponse>(item));
    }
}

