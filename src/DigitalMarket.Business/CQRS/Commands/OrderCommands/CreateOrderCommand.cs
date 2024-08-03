using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.OrderCommands;

public class CreateOrderCommand : IRequest<ApiResponse<OrderResponse>>
{
    public OrderRequest OrderRequest { get; set; }

    public CreateOrderCommand(OrderRequest orderRequest)
    {
        OrderRequest = orderRequest;
    }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse<OrderResponse>>
{

    private readonly IUnitOfWork<Order> _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderCommandHandler(IUnitOfWork<Order> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        
        var item = _mapper.Map<Order>(request.OrderRequest);

        await _unitOfWork.Repository.Insert(item);

        await _unitOfWork.Commit();

        return new ApiResponse<OrderResponse>(_mapper.Map<OrderResponse>(item));
    }
}
