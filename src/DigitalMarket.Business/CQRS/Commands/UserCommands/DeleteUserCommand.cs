using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.UserCommands;

public class DeleteUserCommand : IRequest<ApiResponse>
{
    public long Id { get; set; }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ApiResponse>
{

    private readonly IUnitOfWork<User> _unitOfWork;

    public DeleteUserCommandHandler(IUnitOfWork<User> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
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