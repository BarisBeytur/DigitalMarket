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

namespace DigitalMarket.Business.CQRS.Queries.ProductProductCategoryQueries
{
    public class GetProductCategoryByIdQuery : IRequest<ApiResponse<ProductCategoryResponse>>
    {
        public long Id { get; set; }

        public GetProductCategoryByIdQuery(long id)
        {
            Id = id;
        }
    }

    public class GetProductCategoryByIdQueryHandler : IRequestHandler<GetProductCategoryByIdQuery, ApiResponse<ProductCategoryResponse>>
    {

        private readonly IUnitOfWork<ProductCategory> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductCategoryByIdQueryHandler(IUnitOfWork<ProductCategory> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ProductCategoryResponse>> Handle(GetProductCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository.GetById(query.Id);

            if (result == null)
            {
                return new ApiResponse<ProductCategoryResponse>("Item not found");
            }

            var mappedResult = _mapper.Map<ProductCategoryResponse>(result);

            return new ApiResponse<ProductCategoryResponse>(mappedResult);

        }
    }
}
