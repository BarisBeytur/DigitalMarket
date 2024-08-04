using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Queries.ProductQueries;

public class GetProductByCategoryQuery : IRequest<ApiResponse<List<ProductWithCategoryResponse>>>
{
    public long CategoryId { get; set; }
}

public class GetProductByCategoryQueryHandler : IRequestHandler<GetProductByCategoryQuery, ApiResponse<List<ProductWithCategoryResponse>>>
{

    private readonly IUnitOfWork<Product> _productUnitOfWork;
    private readonly IUnitOfWork<ProductCategory> _productCategoryUnitOfWork;
    private readonly IMapper _mapper;

    public GetProductByCategoryQueryHandler(IUnitOfWork<Product> productUnitOfWork, IUnitOfWork<ProductCategory> productCategoryUnitOfWork, IMapper mapper)
    {
        _productUnitOfWork = productUnitOfWork;
        _productCategoryUnitOfWork = productCategoryUnitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<List<ProductWithCategoryResponse>>> Handle(GetProductByCategoryQuery request, CancellationToken cancellationToken)
    {

        var result = await _productCategoryUnitOfWork.Repository.Where(x => x.CategoryId == request.CategoryId, "Product", "Category");

        var mappedResult = _mapper.Map<List<ProductWithCategoryResponse>>(result);

        return new ApiResponse<List<ProductWithCategoryResponse>>(mappedResult);

    }
}

