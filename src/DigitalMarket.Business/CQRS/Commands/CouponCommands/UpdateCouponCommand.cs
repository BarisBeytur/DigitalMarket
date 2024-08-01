using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.CouponCommands;

public class UpdateCouponCommand : IRequest<ApiResponse<CouponResponse>>
{
    public long Id { get; set; }
    public CouponRequest Request { get; set; }

    public UpdateCouponCommand(long id, CouponRequest request)
    {
        Request = request;
        Id = id;
    }
}

public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommand, ApiResponse<CouponResponse>>
{

    private readonly IUnitOfWork<Coupon> _unitOfWork;

    public UpdateCouponCommandHandler(IUnitOfWork<Coupon> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<ApiResponse<CouponResponse>> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
        {
            return new ApiResponse<CouponResponse>("Item not found");
        }

        // mapping
        item.Code = request.Request.Code;
        item.Discount = request.Request.Discount;

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<CouponResponse>(
            new CouponResponse
            {
                Id = item.Id,
                Code = item.Code,
                Discount = item.Discount
            });
    }
}
