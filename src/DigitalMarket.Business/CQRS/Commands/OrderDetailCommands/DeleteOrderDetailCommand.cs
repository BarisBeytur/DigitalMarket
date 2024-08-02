using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.OrderDetailCommands;

public class DeleteOrderDetailCommand : IRequest<ApiResponse>
{
    public long Id { get; set; }
}

public class DeleteOrderDetailCommandHandler : IRequestHandler<DeleteOrderDetailCommand, ApiResponse>
{

    private readonly IUnitOfWork<OrderDetail> _unitOfWork;

    public DeleteOrderDetailCommandHandler(IUnitOfWork<OrderDetail> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse> Handle(DeleteOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository;

        var item = await repo.GetById(request.Id);

        if (item == null)
        {
            return new ApiResponse("Item not found");
        }

        await repo.Delete(request.Id);

        await _unitOfWork.Commit();

        return new ApiResponse(true, "Deleted succesfully");
    }
}
