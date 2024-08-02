using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using System.Diagnostics;

namespace DigitalMarket.Business.CQRS.Commands.OrderDetailCommands;

public class UpdateOrderDetailCommand : IRequest<ApiResponse<OrderDetailResponse>>
{
    public long Id { get; set; }
    public OrderDetailRequest Request { get; set; }

    public UpdateOrderDetailCommand(long id, OrderDetailRequest request)
    {
        Request = request;
        Id = id;
    }
}

public class UpdateOrderDetailCommandHandler : IRequestHandler<UpdateOrderDetailCommand, ApiResponse<OrderDetailResponse>>
{

    private readonly IUnitOfWork<OrderDetail> _unitOfWork;

    public UpdateOrderDetailCommandHandler(IUnitOfWork<OrderDetail> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<ApiResponse<OrderDetailResponse>> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
        {
            return new ApiResponse<OrderDetailResponse>("Item not found");
        }

        // mapping
        item.ProductId = request.Request.ProductId;
        item.Quantity = request.Request.Quantity;
        item.Price = request.Request.Price; 
        item.PointAmount = request.Request.PointAmount;

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<OrderDetailResponse>(
            new OrderDetailResponse
            {
                Id = item.Id,
                OrderId = item.OrderId,
                PointAmount = item.PointAmount,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Price = item.Price
            });
    }
}

