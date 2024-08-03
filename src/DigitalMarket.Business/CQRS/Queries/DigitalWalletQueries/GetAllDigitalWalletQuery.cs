using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Queries.DigitalWalletQueries
{
    public class GetAllDigitalWalletQuery : IRequest<ApiResponse<IEnumerable<DigitalWalletResponse>>>
    {
        public GetAllDigitalWalletQuery() { }
    }

    public class GetAllDigitalWalletQueryHandler : IRequestHandler<GetAllDigitalWalletQuery, ApiResponse<IEnumerable<DigitalWalletResponse>>>
    {
        private readonly IUnitOfWork<DigitalWallet> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllDigitalWalletQueryHandler(IUnitOfWork<DigitalWallet> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<DigitalWalletResponse>>> Handle(GetAllDigitalWalletQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository.GetAll();

            var mappedResult = _mapper.Map<IEnumerable<DigitalWalletResponse>>(items);

            return new ApiResponse<IEnumerable<DigitalWalletResponse>>(mappedResult);
        }
    }
}
