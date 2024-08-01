using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.CategoryCommands;

public class DeleteCategoryCommand : IRequest<ApiResponse>
{
    public long Id { get; set; }
}

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResponse>
{

    private readonly IUnitOfWork<Category> _unitOfWork;

    public DeleteCategoryCommandHandler(IUnitOfWork<Category> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var repo = _unitOfWork.Repository;

        var item = await repo.GetById(request.Id);

        if (item == null)
        {
            return new ApiResponse("Item not found");
        }

        await repo.Delete(request.Id);

        await _unitOfWork.Commit();

        return new ApiResponse(true, "Deleted succesfully");
    }
}
