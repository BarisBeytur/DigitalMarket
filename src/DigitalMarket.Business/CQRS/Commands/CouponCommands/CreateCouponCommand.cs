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

    public CreateCouponCommandHandler(IUnitOfWork<Coupon> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<CouponResponse>> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        // mapping
        var item = new Coupon
        {
            Code = request.CouponRequest.Code,
            Discount = request.CouponRequest.Discount,
            InsertDate = DateTime.Now,
            InsertUser = "SystemAdmin",
            IsActive = true,
        };

        await _unitOfWork.Repository.Insert(item);

        await _unitOfWork.Commit();

        return new ApiResponse<CouponResponse>(
            new CouponResponse
            {
                Id = item.Id,
                Code = request.CouponRequest.Code,
                Discount = request.CouponRequest.Discount,
            });
    }
}

