using AutoMapper;
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
    private readonly IMapper _mapper;

    public CreateDigitalWalletCommandHandler(IUnitOfWork<DigitalWallet> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<DigitalWalletResponse>> Handle(CreateDigitalWalletCommand request, CancellationToken cancellationToken)
    {
        var digitalWallet = _mapper.Map<DigitalWallet>(request.DigitalWalletRequest);

        await _unitOfWork.Repository.Insert(digitalWallet);
        await _unitOfWork.Commit();

        return new ApiResponse<DigitalWalletResponse>(_mapper.Map<DigitalWalletResponse>(digitalWallet));
    }
}

