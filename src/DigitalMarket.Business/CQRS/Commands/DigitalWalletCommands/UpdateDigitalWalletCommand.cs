using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public UpdateDigitalWalletCommandHandler(IUnitOfWork<DigitalWallet> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<ApiResponse<DigitalWalletResponse>> Handle(UpdateDigitalWalletCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
        {
            return new ApiResponse<DigitalWalletResponse>("Item not found");
        }

        // mapping
        item.Balance = request.Request.Balance;
        item.PointBalance = request.Request.PointBalance;
        item.UserId = request.Request.UserId;

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<DigitalWalletResponse>(
            new DigitalWalletResponse
            {
                Id = item.Id,
                UserId = item.UserId,
                PointBalance = item.PointBalance,
                Balance = item.Balance
            });
    }
}
