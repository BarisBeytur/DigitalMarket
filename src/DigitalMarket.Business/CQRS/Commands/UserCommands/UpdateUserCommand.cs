using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.UserCommands;

public class UpdateUserCommand : IRequest<ApiResponse<UserResponse>>
{
    public long Id { get; set; }
    public UserRequest Request { get; set; }

    public UpdateUserCommand(long id, UserRequest request)
    {
        Request = request;
        Id = id;
    }
}

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ApiResponse<UserResponse>>
{

    private readonly IUnitOfWork<User> _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUnitOfWork<User> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<ApiResponse<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
            return new ApiResponse<UserResponse>("Item not found");

        _mapper.Map(request.Request, item);

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<UserResponse>(_mapper.Map<UserResponse>(item));
    }
}

