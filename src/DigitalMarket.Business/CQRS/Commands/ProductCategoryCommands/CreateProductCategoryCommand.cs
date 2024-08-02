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

    public CreateProductCategoryCommandHandler(IUnitOfWork<ProductCategory> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResponse<ProductCategoryResponse>> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        // mapping
        var item = new ProductCategory
        {
            CategoryId = request.ProductCategoryRequest.CategoryId,
            ProductId = request.ProductCategoryRequest.ProductId,
        };

        await _unitOfWork.Repository.Insert(item);

        await _unitOfWork.Commit();

        return new ApiResponse<ProductCategoryResponse>(
            new ProductCategoryResponse
            {
                Id = item.Id,
                CategoryId = request.ProductCategoryRequest.CategoryId,
                ProductId = request.ProductCategoryRequest.ProductId,
            });
    }
}
