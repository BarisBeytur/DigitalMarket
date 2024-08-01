using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.DigitalWalletCommands;

public class CreateDigitalWalletCommand : IRequest<ApiResponse<DigitalWalletResponse>>
{
    public DigitalWalletRequest DigitalWalletRequest { get; set; }

    public CreateDigitalWalletCommand(DigitalWalletRequest digitalWalletRequest)
    {
        DigitalWalletRequest = digitalWalletRequest;
    }
}

public class CreateDigitalWalletCommandHandler : IRequestHandler<CreateDigitalWalletCommand, ApiResponse<DigitalWalletResponse>>
{

    private readonly IUnitOfWork<DigitalWallet> _unitOfWork;

    public CreateDigitalWalletCommandHandler(IUnitOfWork<DigitalWallet> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<DigitalWalletResponse>> Handle(CreateDigitalWalletCommand request, CancellationToken cancellationToken)
    {
        // mapping
        var item = new DigitalWallet
        {
            Balance = request.DigitalWalletRequest.Balance,
            PointBalance = request.DigitalWalletRequest.PointBalance,
            UserId = request.DigitalWalletRequest.UserId,
            InsertDate = DateTime.Now,
            InsertUser = "SystemAdmin",
            IsActive = true,
        };

        await _unitOfWork.Repository.Insert(item);

        await _unitOfWork.Commit();

        return new ApiResponse<DigitalWalletResponse>(
            new DigitalWalletResponse
            {
                Id = item.Id,
                Balance = request.DigitalWalletRequest.Balance,
                PointBalance = request.DigitalWalletRequest.PointBalance,
                UserId = request.DigitalWalletRequest.UserId,
            });
    }
}

