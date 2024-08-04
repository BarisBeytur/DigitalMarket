using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Queries.DigitalWalletQueries
{
    public class GetPointBalanceQuery : IRequest<ApiResponse<PointBalanceResponse>>
    {
        public long UserId { get; set; }
    }

    public class GetPointBalanceQueryHandler : IRequestHandler<GetPointBalanceQuery, ApiResponse<PointBalanceResponse>>
    {
        private readonly IUnitOfWork<DigitalWallet> _digitalWalletUnitOfWork;
        private readonly IMapper _mapper;

        public GetPointBalanceQueryHandler(IUnitOfWork<DigitalWallet> digitalWalletUnitOfWork, IMapper mapper)
        {
            _digitalWalletUnitOfWork = digitalWalletUnitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<PointBalanceResponse>> Handle(GetPointBalanceQuery request, CancellationToken cancellationToken)
        {
            var result = await _digitalWalletUnitOfWork.Repository.Where(x => x.UserId == request.UserId, "User");

            var digitalWallet = result.FirstOrDefault();

            if (digitalWallet == null)
            {
                return new ApiResponse<PointBalanceResponse>("Digital wallet and user not found");
            }

            return new ApiResponse<PointBalanceResponse>(_mapper.Map<DigitalWallet, PointBalanceResponse>(digitalWallet));
        }
    }
}
