using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.CategoryCommands;

public class CreateCategoryCommand : IRequest<ApiResponse<CategoryResponse>>
{
    public CategoryRequest CategoryRequest { get; set; }

    public CreateCategoryCommand(CategoryRequest categoryRequest)
    {
        CategoryRequest = categoryRequest;
    }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, ApiResponse<CategoryResponse>>
{

    private readonly IUnitOfWork<Category> _unitOfWork;

    public CreateCategoryCommandHandler(IUnitOfWork<Category> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        // mapping
        var item = new Category
        {
            Name = request.CategoryRequest.Name,
            Url = request.CategoryRequest.Url,
            Tags = request.CategoryRequest.Tags,
            InsertUser = "testuser",
            InsertDate = DateTime.Now,
            IsActive = true
        };

        await _unitOfWork.Repository.Insert(item);

        await _unitOfWork.Commit();

        return new ApiResponse<CategoryResponse>(
            new CategoryResponse
            {
                Id = item.Id,
                Name = item.Name,
                Url = item.Url,
                Tags = item.Tags
            });
    }
}
