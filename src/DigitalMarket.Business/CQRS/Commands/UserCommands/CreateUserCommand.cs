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

    public CreateUserCommandHandler(IUnitOfWork<User> unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {

        // Create a digital wallet for the user
        var digitalWalletForUser = await _mediator.Send(new CreateDigitalWalletCommand(new DigitalWalletRequest
        {
            Balance = 0m,
            PointBalance = 0m,
        }));

        if (digitalWalletForUser == null || digitalWalletForUser.Data == null)
        {
            return new ApiResponse<UserResponse>("Failed to create digital wallet.");
        }

        // mapping
        var item = new User
        {
            Email = request.UserRequest.Email,
            Name = request.UserRequest.Name,
            Surname = request.UserRequest.Surname,
            Password = request.UserRequest.Password,
            DigitalWalletId = digitalWalletForUser.Data.Id,
            Role = request.UserRequest.Role,
            Status = true,        
        };

        await _unitOfWork.Repository.Insert(item);
        await _unitOfWork.CommitWithTransaction();

        // Update digital wallet with user ID
        var updateDigitalWalletResult = await _mediator.Send(new UpdateDigitalWalletCommand(digitalWalletForUser.Data.Id, new DigitalWalletRequest
        {
            UserId = item.Id,
            Balance = 0,
            PointBalance = 0,
        }));

        if (!updateDigitalWalletResult.IsSuccess)
        {
            return new ApiResponse<UserResponse>("Failed to update digital wallet with user ID.");
        }

        return new ApiResponse<UserResponse>(
            new UserResponse
            {
                Id = item.Id,
                Email = item.Email,
                Name = item.Name,
                Surname = item.Surname,
                Role = item.Role,
                Status = item.Status,
                DigitalWalletId = item.DigitalWalletId,
            });
    }
}