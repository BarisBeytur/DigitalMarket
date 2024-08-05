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
    private readonly IUnitOfWork<User> _userUnitOfWork;
    private readonly IUnitOfWork<DigitalWallet> _digitalWalletUnitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUnitOfWork<User> unitOfWork, IMediator mediator, IMapper mapper, IUnitOfWork<DigitalWallet> digitalWalletUnitOfWork)
    {
        _userUnitOfWork = unitOfWork;
        _mediator = mediator;
        _mapper = mapper;
        _digitalWalletUnitOfWork = digitalWalletUnitOfWork;
    }

    public async Task<ApiResponse<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var digitalWallet = new DigitalWallet
        {
            Balance = 0m,
            PointBalance = 0m,
            IsActive = true,
            InsertDate = DateTime.Now,
            InsertUser = "System", 
        };

        await _digitalWalletUnitOfWork.Repository.Insert(digitalWallet);
        await _digitalWalletUnitOfWork.CommitWithTransaction();

        var user = _mapper.Map<User>(request.UserRequest);
        user.DigitalWalletId = digitalWallet.Id;

        await _userUnitOfWork.Repository.Insert(user);
        await _userUnitOfWork.CommitWithTransaction();

        digitalWallet.UserId = user.Id;
        _digitalWalletUnitOfWork.Repository.Update(digitalWallet);
        await _digitalWalletUnitOfWork.CommitWithTransaction();

        return new ApiResponse<UserResponse>(_mapper.Map<UserResponse>(user));
    }
}
