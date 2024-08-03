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

namespace DigitalMarket.Business.CQRS.Queries.ProductQueries
{
    public class GetProductByIdQuery : IRequest<ApiResponse<ProductResponse>>
    {
        public long Id { get; set; }

        public GetProductByIdQuery(long id)
        {
            Id = id;
        }
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ApiResponse<ProductResponse>>
    {

        private readonly IUnitOfWork<Product> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductByIdQueryHandler(IUnitOfWork<Product> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ProductResponse>> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository.GetById(query.Id);

            if (result == null)
            {
                return new ApiResponse<ProductResponse>("Item not found");
            }

            var mappedResult = _mapper.Map<ProductResponse>(result);

            return new ApiResponse<ProductResponse>(mappedResult);

        }
    }
}
