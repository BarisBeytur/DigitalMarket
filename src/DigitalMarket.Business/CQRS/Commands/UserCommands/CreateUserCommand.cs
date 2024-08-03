using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.DigitalWalletCommands;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.UserCommands;

public class CreateUserCommand : IRequest<ApiResponse<UserResponse>>
{
    public UserRequest UserRequest { get; set; }

    public CreateUserCommand(UserRequest userRequest)
    {
        UserRequest = userRequest;
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ApiResponse<UserResponse>>
{

    private readonly IUnitOfWork<User> _unitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUnitOfWork<User> unitOfWork, IMediator mediator, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        // Create a digital wallet for the user
        var digitalWalletForUser = await _mediator.Send(new CreateDigitalWalletCommand(
            new DigitalWalletRequest
            {
                Balance = 0m,
                PointBalance = 0m,
            }));

        if (digitalWalletForUser == null || digitalWalletForUser.Data == null)
        {
            return new ApiResponse<UserResponse>("Failed to create digital wallet.");
        }

        var user = _mapper.Map<User>(request.UserRequest);

        await _unitOfWork.Repository.Insert(user);
        await _unitOfWork.CommitWithTransaction();

        // Update digital wallet for assing user ID to digital wallet
        var updateDigitalWalletResult = await _mediator.Send(
            new UpdateDigitalWalletCommand(digitalWalletForUser.Data.Id, new DigitalWalletRequest
            {
                UserId = user.Id,
                Balance = 0,
                PointBalance = 0,
            }));

        if (!updateDigitalWalletResult.IsSuccess)
        {
            return new ApiResponse<UserResponse>("Failed to update digital wallet with user ID.");
        }

        return new ApiResponse<UserResponse>(_mapper.Map<UserResponse>(user));
    }
}