using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.ProductCategoryCommands;

public class UpdateProductCategoryCommand : IRequest<ApiResponse<ProductCategoryResponse>>
{
    public long Id { get; set; }
    public ProductCategoryRequest Request { get; set; }

    public UpdateProductCategoryCommand(long id, ProductCategoryRequest request)
    {
        Request = request;
        Id = id;
    }
}

public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, ApiResponse<ProductCategoryResponse>>
{

    private readonly IUnitOfWork<ProductCategory> _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateProductCategoryCommandHandler(IUnitOfWork<ProductCategory> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<ApiResponse<ProductCategoryResponse>> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
            return new ApiResponse<ProductCategoryResponse>("Item not found");

        _mapper.Map(request.Request, item);

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<ProductCategoryResponse>(_mapper.Map<ProductCategoryResponse>(item));
    }
}

