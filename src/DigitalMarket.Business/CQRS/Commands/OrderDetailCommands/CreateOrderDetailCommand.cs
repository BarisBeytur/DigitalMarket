using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.OrderDetailCommands;

public class CreateOrderDetailCommand : IRequest<ApiResponse<OrderDetailResponse>>
{
    public OrderDetailRequest OrderDetailRequest { get; set; }

    public CreateOrderDetailCommand(OrderDetailRequest orderDetailRequest)
    {
        OrderDetailRequest = orderDetailRequest;
    }
}

public class CreateOrderDetailCommandHandler : IRequestHandler<CreateOrderDetailCommand, ApiResponse<OrderDetailResponse>>
{

    private readonly IUnitOfWork<OrderDetail> _unitOfWork;
    private readonly IMapper _mapper;

    public CreateOrderDetailCommandHandler(IUnitOfWork<OrderDetail> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResponse<OrderDetailResponse>> Handle(CreateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        // mapping
        var item = _mapper.Map<OrderDetail>(request.OrderDetailRequest);

        await _unitOfWork.Repository.Insert(item);

        await _unitOfWork.Commit();

        return new ApiResponse<OrderDetailResponse>(_mapper.Map<OrderDetailResponse>(item));
    }
}


