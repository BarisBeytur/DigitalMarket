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

namespace DigitalMarket.Business.CQRS.Queries.UserQueries
{
    public class GetUserByIdQuery : IRequest<ApiResponse<UserResponse>>
    {
        public long Id { get; set; }

        public GetUserByIdQuery(long id)
        {
            Id = id;
        }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, ApiResponse<UserResponse>>
    {

        private readonly IUnitOfWork<User> _unitOfWork;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(IUnitOfWork<User> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<UserResponse>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _unitOfWork.Repository.GetById(query.Id);

            if (result == null)
            {
                return new ApiResponse<UserResponse>("Item not found");
            }

            var mappedResult = _mapper.Map<UserResponse>(result);

            return new ApiResponse<UserResponse>(mappedResult);

        }
    }
}
