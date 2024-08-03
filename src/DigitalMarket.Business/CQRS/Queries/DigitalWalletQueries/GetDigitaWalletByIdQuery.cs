using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Business.CQRS.Queries.DigitalWalletQueries
{
    public class GetDigitalWalletByIdQuery : IRequest<ApiResponse<DigitalWalletResponse>>
    {
        public long Id { get; set; }

        public GetDigitalWalletByIdQuery(long id)
        {
            Id = id;
        }
    }

    public class GetDigitalWalletByIdQueryHandler : IRequestHandler<GetDigitalWalletByIdQuery, ApiResponse<DigitalWalletResponse>>
    {

        private readonly IUnitOfWork<DigitalWallet> _unitOfWork;
        private readonly IMapper _mapper;

        public GetDigitalWalletByIdQueryHandler(IUnitOfWork<DigitalWallet> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<DigitalWalletResponse>> Handle(GetDigitalWalletByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository.GetById(query.Id);

            if (result == null)
            {
                return new ApiResponse<DigitalWalletResponse>("Item not found");
            }

            var mappedResult = _mapper.Map<DigitalWalletResponse>(result);

            return new ApiResponse<DigitalWalletResponse>(mappedResult);

        }
    }
}
