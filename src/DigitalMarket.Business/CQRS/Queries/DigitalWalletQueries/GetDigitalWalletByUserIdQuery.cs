using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Queries.DigitalWalletQueries
{
    public class GetDigitalWalletByUserIdQuery : IRequest<ApiResponse<DigitalWalletResponse>>
    {
        public long userId { get; set; }

        public GetDigitalWalletByUserIdQuery(long userId)
        {
            this.userId = userId;
        }
    }

    public class GetDigitalWalletByUserIdQueryHandler : IRequestHandler<GetDigitalWalletByUserIdQuery, ApiResponse<DigitalWalletResponse>>
    {
        private readonly IUnitOfWork<DigitalWallet> _digitalWalletUnitOfWork;

        public GetDigitalWalletByUserIdQueryHandler(IUnitOfWork<DigitalWallet> digitalWalletUnitOfWork)
        {
            _digitalWalletUnitOfWork = digitalWalletUnitOfWork;
        }

        public async Task<ApiResponse<DigitalWalletResponse>> Handle(GetDigitalWalletByUserIdQuery request, CancellationToken cancellationToken)
        {

            var wallet = await _digitalWalletUnitOfWork.Repository.Where(x => x.UserId == request.userId);
            return wallet.FirstOrDefault() != null ? new ApiResponse<DigitalWalletResponse>(new DigitalWalletResponse
            {
                PointBalance = wallet.FirstOrDefault().PointBalance,
                UserId = wallet.FirstOrDefault().UserId,
                Id = wallet.FirstOrDefault().Id
            }) : new ApiResponse<DigitalWalletResponse>("User not found");

            throw new NotImplementedException();
        }
    }
}
