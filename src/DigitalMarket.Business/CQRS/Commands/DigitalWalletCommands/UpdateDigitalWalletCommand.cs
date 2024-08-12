using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.DigitalWalletCommands;

public class UpdateDigitalWalletCommand : IRequest<ApiResponse<DigitalWalletResponse>>
{
    public long Id { get; set; }
    public DigitalWalletRequest Request { get; set; }

    public UpdateDigitalWalletCommand(long id, DigitalWalletRequest request)
    {
        Request = request;
        Id = id;
    }
}

public class UpdateDigitalWalletCommandHandler : IRequestHandler<UpdateDigitalWalletCommand, ApiResponse<DigitalWalletResponse>>
{

    private readonly IUnitOfWork<DigitalWallet> _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateDigitalWalletCommandHandler(IUnitOfWork<DigitalWallet> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<ApiResponse<DigitalWalletResponse>> Handle(UpdateDigitalWalletCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
            return new ApiResponse<DigitalWalletResponse>("Item not found");
     
        
        item.PointBalance = request.Request.PointBalance;
        item.UserId = request.Request.UserId;
        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<DigitalWalletResponse>(_mapper.Map<DigitalWalletResponse>(item));
    }
}
