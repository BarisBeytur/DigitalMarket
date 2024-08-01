using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.OrderCommands;

public class DeleteOrderCommand : IRequest<ApiResponse>
{
    public long Id { get; set; }
}

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, ApiResponse>
{

    private readonly IUnitOfWork<Order> _unitOfWork;

    public DeleteOrderCommandHandler(IUnitOfWork<Order> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
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
