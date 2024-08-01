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

    public UpdateUserCommandHandler(IUnitOfWork<User> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<ApiResponse<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
        {
            return new ApiResponse<UserResponse>("Item not found");
        }

        // mapping
        item.Name = request.Request.Name;
        item.Surname = request.Request.Surname;
        item.Email = request.Request.Email;
        item.Password = request.Request.Password;
        item.Status = request.Request.Status;
        item.Role = request.Request.Role;

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<UserResponse>(
            new UserResponse
            {
                Id = item.Id,
                Name = item.Name,
                Surname = item.Surname,
                Email = item.Email,
                Role = item.Role,
                Status = item.Status,
                DigitalWalletId = item.DigitalWalletId
            });
    }
}

