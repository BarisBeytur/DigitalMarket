using AutoMapper;
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
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(IUnitOfWork<Product> productUnitOfWork, IMapper mapper)
    {
        _productUnitOfWork = productUnitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<ProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var item = _mapper.Map<Product>(request.ProductRequest);

        await _productUnitOfWork.Repository.Insert(item);
        await _productUnitOfWork.Commit();

        return new ApiResponse<ProductResponse>(_mapper.Map<ProductResponse>(item));
    }

}
