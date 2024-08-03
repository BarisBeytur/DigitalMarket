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

namespace DigitalMarket.Business.CQRS.Queries.ProductCategoryQueries
{
    public class GetAllProductCategoryQuery : IRequest<ApiResponse<IEnumerable<ProductCategoryResponse>>>
    {
        public GetAllProductCategoryQuery() { }
    }

    public class GetAllProductCategoryQueryHandler : IRequestHandler<GetAllProductCategoryQuery, ApiResponse<IEnumerable<ProductCategoryResponse>>>
    {
        private readonly IUnitOfWork<ProductCategory> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllProductCategoryQueryHandler(IUnitOfWork<ProductCategory> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<ProductCategoryResponse>>> Handle(GetAllProductCategoryQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository.GetAll();

            var mappedResult = _mapper.Map<IEnumerable<ProductCategoryResponse>>(items);

            return new ApiResponse<IEnumerable<ProductCategoryResponse>>(mappedResult);
        }
    }
}
