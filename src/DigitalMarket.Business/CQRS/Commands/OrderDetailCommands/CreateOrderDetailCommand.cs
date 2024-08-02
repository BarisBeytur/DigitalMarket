using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.OrderDetailCommands;

public class CreateOrderDetailCommand : IRequest<ApiResponse<OrderDetailResponse>>
{
    public OrderDetailRequest OrderDetailRequest { get; set; }

    public CreateOrderDetailCommand(OrderDetailRequest orderDetailRequest)
    {
        OrderDetailRequest = orderDetailRequest;
    }
}

public class CreateOrderDetailCommandHandler : IRequestHandler<CreateOrderDetailCommand, ApiResponse<OrderDetailResponse>>
{

    private readonly IUnitOfWork<OrderDetail> _unitOfWork;

    public CreateOrderDetailCommandHandler(IUnitOfWork<OrderDetail> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<OrderDetailResponse>> Handle(CreateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        // mapping
        var item = new OrderDetail
        {
            OrderId = request.OrderDetailRequest.OrderId,
            PointAmount = request.OrderDetailRequest.PointAmount,
            ProductId = request.OrderDetailRequest.ProductId,
            Quantity = request.OrderDetailRequest.Quantity,
            Price = request.OrderDetailRequest.Price,          
        };

        await _unitOfWork.Repository.Insert(item);

        await _unitOfWork.Commit();

        return new ApiResponse<OrderDetailResponse>(
            new OrderDetailResponse
            {
                Id = item.Id,
                OrderId = request.OrderDetailRequest.OrderId,
                PointAmount = request.OrderDetailRequest.PointAmount,
                ProductId = request.OrderDetailRequest.ProductId,
                Quantity = request.OrderDetailRequest.Quantity,
                Price = request.OrderDetailRequest.Price,      
            });
    }
}


