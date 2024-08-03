using AutoMapper;
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
    private readonly IMapper _mapper;

    public UpdateProductCommandHandler(IUnitOfWork<Product> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<ApiResponse<ProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
            return new ApiResponse<ProductResponse>("Item not found");

        _mapper.Map(request.Request, item);

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<ProductResponse>(_mapper.Map<ProductResponse>(item));
    }
}
