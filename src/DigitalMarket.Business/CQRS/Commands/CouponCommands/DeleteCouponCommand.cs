using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.CouponCommands;

public class DeleteCouponCommand : IRequest<ApiResponse>
{
    public long Id { get; set; }
}

public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, ApiResponse>
{

    private readonly IUnitOfWork<Coupon> _unitOfWork;

    public DeleteCouponCommandHandler(IUnitOfWork<Coupon> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository;

        var item = await repo.GetById(request.Id);

        if (item == null)
        {
            return new ApiResponse("Item not found");
        }

        await repo.Delete(request.Id);

        await _unitOfWork.Commit();

        return new ApiResponse(true,"Deleted succesfully");
    }
}