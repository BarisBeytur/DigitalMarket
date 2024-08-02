using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.ProductCommands;

public class CreateProductCommand : IRequest<ApiResponse<ProductResponse>>
{
    public ProductRequest ProductRequest { get; set; }

    public CreateProductCommand(ProductRequest productRequest)
    {
        ProductRequest = productRequest;
    }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ApiResponse<ProductResponse>>
{
    private readonly IUnitOfWork<Product> _productUnitOfWork;
    private readonly IUnitOfWork<ProductCategory> _productCategoriesUnitOfWork;

    public CreateProductCommandHandler(IUnitOfWork<Product> productUnitOfWork, IUnitOfWork<ProductCategory> productCategoriesUnitOfWork)
    {
        _productUnitOfWork = productUnitOfWork;
        _productCategoriesUnitOfWork = productCategoriesUnitOfWork;
    }

    public async Task<ApiResponse<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.ProductRequest.Name,
            Description = request.ProductRequest.Description,
            StockCount = request.ProductRequest.StockCount,
            Price = request.ProductRequest.Price,
            PointPercentage = request.ProductRequest.PointPercentage,
            MaxPoint = request.ProductRequest.MaxPoint,
            IsActive = request.ProductRequest.IsActive,
        };

        await _productUnitOfWork.Repository.Insert(product);
        await _productUnitOfWork.Commit();

        var productResponse = new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            StockCount = product.StockCount,
            Price = product.Price,
            PointPercentage = product.PointPercentage,
            MaxPoint = product.MaxPoint,
            IsActive = product.IsActive,
        };

        return new ApiResponse<ProductResponse>(productResponse);
    }

}
