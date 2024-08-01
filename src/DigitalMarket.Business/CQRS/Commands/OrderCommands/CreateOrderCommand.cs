using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.OrderCommands;

public class CreateOrderCommand : IRequest<ApiResponse<OrderResponse>>
{
    public OrderRequest OrderRequest { get; set; }

    public CreateOrderCommand(OrderRequest orderRequest)
    {
        OrderRequest = orderRequest;
    }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse<OrderResponse>>
{

    private readonly IUnitOfWork<Order> _unitOfWork;

    public CreateOrderCommandHandler(IUnitOfWork<Order> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // mapping
        var item = new Order
        {
            BasketAmount = request.OrderRequest.BasketAmount,
            CouponAmount = request.OrderRequest.CouponAmount,
            CouponCode = request.OrderRequest.CouponCode,
            PointAmount = request.OrderRequest.PointAmount,
            TotalAmount = request.OrderRequest.TotalAmount,
            UserId = request.OrderRequest.UserId,
            InsertDate = DateTime.Now,
            InsertUser = "SystemAdmin",
            IsActive = true,
        };

        await _unitOfWork.Repository.Insert(item);

        await _unitOfWork.Commit();

        return new ApiResponse<OrderResponse>(
            new OrderResponse
            {
                Id = item.Id,
                BasketAmount = request.OrderRequest.BasketAmount,
                CouponAmount = request.OrderRequest.CouponAmount,
                CouponCode = request.OrderRequest.CouponCode,
                PointAmount = request.OrderRequest.PointAmount,
                TotalAmount = request.OrderRequest.TotalAmount,
                UserId = request.OrderRequest.UserId,
            });
    }
}
