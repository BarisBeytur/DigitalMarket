using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.ProductCategoryCommands;

public class CreateProductCategoryCommand : IRequest<ApiResponse<ProductCategoryResponse>>
{
    public ProductCategoryRequest ProductCategoryRequest { get; set; }

    public CreateProductCategoryCommand(ProductCategoryRequest productCategoryRequest)
    {
        ProductCategoryRequest = productCategoryRequest;
    }
}

public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, ApiResponse<ProductCategoryResponse>>
{

    private readonly IUnitOfWork<ProductCategory> _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCategoryCommandHandler(IUnitOfWork<ProductCategory> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ProductCategoryResponse>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        
        var item = _mapper.Map<ProductCategory>(request.ProductCategoryRequest);

        await _unitOfWork.Repository.Insert(item);

        await _unitOfWork.Commit();

        return new ApiResponse<ProductCategoryResponse>(_mapper.Map<ProductCategoryResponse>(item));
    }
}
