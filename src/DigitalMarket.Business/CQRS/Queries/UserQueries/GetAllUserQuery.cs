using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Queries.UserQueries
{
    public class GetAllUserQuery : IRequest<ApiResponse<IEnumerable<UserResponse>>>
    {
        public GetAllUserQuery() { }
    }

    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, ApiResponse<IEnumerable<UserResponse>>>
    {
        private readonly IUnitOfWork<User> _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllUserQueryHandler(IUnitOfWork<User> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IEnumerable<UserResponse>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var items = await _unitOfWork.Repository.GetAll();

            var mappedResult = _mapper.Map<IEnumerable<UserResponse>>(items);

            return new ApiResponse<IEnumerable<UserResponse>>(mappedResult);
        }
    }
}
