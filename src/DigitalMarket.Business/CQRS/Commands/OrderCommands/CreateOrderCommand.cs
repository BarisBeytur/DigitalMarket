using AutoMapper;
using DigitalMarket.Base.Enums;
using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.ProductCommands;
using DigitalMarket.Business.CQRS.Queries.CartQueries;
using DigitalMarket.Business.CQRS.Queries.ProductQueries;
using DigitalMarket.Business.CQRS.Queries.UserQueries;
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

    private readonly IUnitOfWork<Order> _orderUnitOfWork;
    private readonly IUnitOfWork<Coupon> _couponUnitOfWork;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;


    public CreateOrderCommandHandler(IUnitOfWork<Order> orderUnitOfWork, IMapper mapper, IMediator mediator, IUnitOfWork<Coupon> couponUnitOfWork)
    {
        _orderUnitOfWork = orderUnitOfWork;
        _couponUnitOfWork = couponUnitOfWork;

        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<ApiResponse<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {

        var isUserExists = await CheckUser(request.OrderRequest.UserId);

        if (!isUserExists)
        {
            return new ApiResponse<OrderResponse>("User not found");
        }

        var cartItems = await _mediator.Send(new GetCartByUserIdQuery(request.OrderRequest.UserId));

        decimal totalAmount = CalculateTotalAmount(cartItems.Data);

        decimal totalAmountAfterCouponApplied = totalAmount;

        if (!string.IsNullOrEmpty(request.OrderRequest.CouponCode))
        {
            totalAmountAfterCouponApplied = await ApplyCouponCode(totalAmount, request.OrderRequest.CouponCode);
        }

        // TODO: puan sistemi entegre edilecek

        var order = _mapper.Map<Order>(request.OrderRequest);

        order.BasketAmount = totalAmount;
        order.TotalAmount = totalAmountAfterCouponApplied;
        order.Status = Convert.ToInt16(Enums.OrderStatus.Approved);
        order.CouponAmount = totalAmount - totalAmountAfterCouponApplied;
        //order.PointAmount = 

        await _orderUnitOfWork.Repository.Insert(order);

        await _orderUnitOfWork.Commit();

        await DecreaseStockCounts(cartItems.Data);

        return new ApiResponse<OrderResponse>(_mapper.Map<OrderResponse>(order));
    }

    private decimal CalculateTotalAmount(IEnumerable<CartResponse> cartItems)
    {
        return cartItems.Sum(item => item.Price * item.Quantity);
    }

    private async Task<bool> CheckUser(long userId)
    {
        var user = await _mediator.Send(new GetUserByIdQuery(userId));
        if (user.Data == null)
        {
            return false;
        }
        return true;
    }

    private async Task<decimal> ApplyCouponCode(decimal totalAmount, string code)
    {
        var couponCode = await _couponUnitOfWork.Repository.Where(x => x.Code == code);
        if (couponCode != null)
        {
            var coupon = couponCode.FirstOrDefault();
            totalAmount -= coupon?.Discount ?? 0;
        }
        return totalAmount;
    }

    private async Task DecreaseStockCounts(IEnumerable<CartResponse> cartItems)
    {
        foreach (var item in cartItems)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(item.ProductId));
            product.Data.StockCount -= item.Quantity;
            await _mediator.Send(new UpdateProductCommand(item.ProductId, _mapper.Map<ProductRequest>(product.Data)));
        }
    }
}
