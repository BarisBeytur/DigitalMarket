using AutoMapper;
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
    private readonly IMapper _mapper;

    public CreateCategoryCommandHandler(IUnitOfWork<Category> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<CategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {

        var item = _mapper.Map<Category>(request.CategoryRequest);

        await _unitOfWork.Repository.Insert(item);

        await _unitOfWork.Commit();

        return new ApiResponse<CategoryResponse>(_mapper.Map<CategoryResponse>(item));
    }
}
