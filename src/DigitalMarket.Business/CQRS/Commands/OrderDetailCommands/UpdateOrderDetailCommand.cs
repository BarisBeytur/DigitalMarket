using AutoMapper;
using DigitalMarket.Base.Response;
using DigitalMarket.Data.Domain;
using DigitalMarket.Data.UnitOfWork;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using System.Diagnostics;

namespace DigitalMarket.Business.CQRS.Commands.OrderDetailCommands;

public class UpdateOrderDetailCommand : IRequest<ApiResponse<OrderDetailResponse>>
{
    public long Id { get; set; }
    public OrderDetailRequest Request { get; set; }

    public UpdateOrderDetailCommand(long id, OrderDetailRequest request)
    {
        Request = request;
        Id = id;
    }
}

public class UpdateOrderDetailCommandHandler : IRequestHandler<UpdateOrderDetailCommand, ApiResponse<OrderDetailResponse>>
{

    private readonly IUnitOfWork<OrderDetail> _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateOrderDetailCommandHandler(IUnitOfWork<OrderDetail> unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }


    public async Task<ApiResponse<OrderDetailResponse>> Handle(UpdateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var item = await _unitOfWork.Repository.GetById(request.Id);

        if (item == null)
            return new ApiResponse<OrderDetailResponse>("Item not found");

        _mapper.Map(request.Request, item);

        _unitOfWork.Repository.Update(item);
        await _unitOfWork.Commit();

        return new ApiResponse<OrderDetailResponse>(_mapper.Map<OrderDetailResponse>(item));
    }
}

