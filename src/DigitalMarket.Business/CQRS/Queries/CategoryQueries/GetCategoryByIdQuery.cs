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

namespace DigitalMarket.Business.CQRS.Queries.CategoryQueries
{
    public class GetCategoryByIdQuery : IRequest<ApiResponse<CategoryResponse>>
    {
        public long Id { get; set; }

        public GetCategoryByIdQuery(long id)
        {
            Id = id;
        }
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, ApiResponse<CategoryResponse>>
    {

        private readonly IUnitOfWork<Category> _unitOfWork;
        private readonly IMapper _mapper;

        public GetCategoryByIdQueryHandler(IUnitOfWork<Category> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CategoryResponse>> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository.GetById(query.Id);

            if (result == null)
            {
                return new ApiResponse<CategoryResponse>("Item not found");
            }

            var mappedResult = _mapper.Map<CategoryResponse>(result);

            return new ApiResponse<CategoryResponse>(mappedResult);

        }
    }
}
