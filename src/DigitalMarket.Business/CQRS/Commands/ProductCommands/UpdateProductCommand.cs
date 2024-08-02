using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.ProductCommands;

public class UpdateProductCommand : IRequest<ApiResponse<ProductResponse>>
{
    public long Id { get; set; }
    public ProductRequest Request { get; set; }

    public UpdateProductCommand(long id, ProductRequest request)
    {
        Request = request;
        Id = id;
    }
}

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResponse<ProductResponse>>
{

    private readonly IUnitOfWork<Product> _unitOfWork;

    public UpdateProductCommandHandler(IUnitOfWork<Product> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }


    public async Task<ApiResponse<ProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
        {
            return new ApiResponse<ProductResponse>("Item not found");
        }

        // mapping
        item.Description = request.Request.Description;
        item.Price = request.Request.Price;
        item.PointPercentage = request.Request.PointPercentage;
        item.MaxPoint = request.Request.MaxPoint;
        item.Name = request.Request.Name;
        item.StockCount = request.Request.StockCount;
        item.IsActive = request.Request.IsActive;

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<ProductResponse>(
            new ProductResponse
            {
                Id = item.Id,
                Description = item.Description,
                MaxPoint = item.MaxPoint,
                Name = item.Name,
                PointPercentage = item.PointPercentage,
                Price = item.Price,
                StockCount = item.StockCount,
                IsActive = item.IsActive
            });
    }
}
