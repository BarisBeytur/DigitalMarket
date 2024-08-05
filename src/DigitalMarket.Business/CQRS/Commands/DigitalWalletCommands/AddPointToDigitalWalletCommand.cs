using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.DigitalWalletCommands
{
    public class AddPointToDigitalWalletCommand : IRequest<ApiResponse<AddPointToDigitalWalletCommand>>
    {
        public long UserId { get; set; }
        public decimal Point { get; set; }

        public AddPointToDigitalWalletCommand(long userId, decimal point)
        {
            UserId = userId;
            Point = point;
        }
    }

    public class AddPointToDigitalWalletCommandHandler : IRequestHandler<AddPointToDigitalWalletCommand, ApiResponse<AddPointToDigitalWalletCommand>>
    {
        private readonly IUnitOfWork<DigitalWallet> _digitalWalletUnitOfWork;

        public AddPointToDigitalWalletCommandHandler(IUnitOfWork<DigitalWallet> digitalWalletUnitOfWork)
        {
            _digitalWalletUnitOfWork = digitalWalletUnitOfWork;
        }

        public async Task<ApiResponse<AddPointToDigitalWalletCommand>> Handle(AddPointToDigitalWalletCommand request, CancellationToken cancellationToken)
        {
            var response = await _digitalWalletUnitOfWork.Repository.Where(x => x.UserId == request.UserId);
            var wallet = response.FirstOrDefault();

            if (wallet == null)
            {
                return new ApiResponse<AddPointToDigitalWalletCommand>("Wallet not found");
            }
                
            wallet.PointBalance += request.Point;
            _digitalWalletUnitOfWork.Repository.Update(wallet);
            await _digitalWalletUnitOfWork.Commit();

            return new ApiResponse<AddPointToDigitalWalletCommand>(true);
        }
    }

}
