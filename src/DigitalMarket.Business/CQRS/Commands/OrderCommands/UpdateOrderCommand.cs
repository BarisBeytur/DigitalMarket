using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.OrderCommands;

public class UpdateOrderCommand : IRequest<ApiResponse<OrderResponse>>
{
    public long Id { get; set; }
    public OrderRequest Request { get; set; }

    public UpdateOrderCommand(long id, OrderRequest request)
    {
        Request = request;
        Id = id;
    }
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResponse<OrderResponse>>
{

    private readonly IUnitOfWork<Order> _unitOfWork;

    public UpdateOrderCommandHandler(IUnitOfWork<Order> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<ApiResponse<OrderResponse>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
        {
            return new ApiResponse<OrderResponse>("Item not found");
        }

        // mapping
        item.UserId = request.Request.UserId;
        item.BasketAmount = request.Request.BasketAmount;
        item.CouponAmount = request.Request.CouponAmount;
        item.CouponCode = request.Request.CouponCode;
        item.PointAmount = request.Request.PointAmount;
        item.TotalAmount = request.Request.TotalAmount;

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<OrderResponse>(
            new OrderResponse
            {
                Id = item.Id,
                TotalAmount = item.TotalAmount,
                BasketAmount = item.BasketAmount,
                CouponAmount = item.CouponAmount,
                CouponCode = item.CouponCode,
                PointAmount = item.PointAmount,
                UserId = item.UserId
            });
    }
}
