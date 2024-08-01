using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.DigitalWalletCommands;

public class DeleteDigitalWalletCommand : IRequest<ApiResponse>
{
    public long Id { get; set; }
}

public class DeleteDigitalWalletCommandHandler : IRequestHandler<DeleteDigitalWalletCommand, ApiResponse>
{

    private readonly IUnitOfWork<DigitalWallet> _unitOfWork;

    public DeleteDigitalWalletCommandHandler(IUnitOfWork<DigitalWallet> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse> Handle(DeleteDigitalWalletCommand request, CancellationToken cancellationToken)
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
