using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;

namespace DigitalMarket.Business.CQRS.Commands.OrderCommands;

public class UpdateOrderCommand : IRequest<ApiResponse<OrderResponse>>
{
    public long Id { get; set; }
    public OrderRequest Request { get; set; }

    public UpdateOrderCommand(long id, OrderRequest request)
    {
        Request = request;
        Id = id;
    }
}

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResponse<OrderResponse>>
{

    private readonly IUnitOfWork<Order> _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOrderCommandHandler(IUnitOfWork<Order> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<ApiResponse<OrderResponse>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
            return new ApiResponse<OrderResponse>("Item not found");

        _mapper.Map(request.Request, item);

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<OrderResponse>(_mapper.Map<OrderResponse>(item));
    }
}
