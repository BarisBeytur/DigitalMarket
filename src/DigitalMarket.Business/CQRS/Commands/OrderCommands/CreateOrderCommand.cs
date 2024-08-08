using AutoMapper;
using Azure.Core;
using DigitalMarket.Base.Enums;
using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.CartCommands;
using DigitalMarket.Business.CQRS.Commands.DigitalWalletCommands;
using DigitalMarket.Business.CQRS.Commands.ProductCommands;
using DigitalMarket.Business.CQRS.Queries.CartQueries;
using DigitalMarket.Business.CQRS.Queries.DigitalWalletQueries;
using DigitalMarket.Business.CQRS.Queries.ProductQueries;
using DigitalMarket.Business.CQRS.Queries.UserQueries;
using DigitalMarket.Business.Services.PaymentService;
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
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;


    public CreateOrderCommandHandler(IUnitOfWork<Order> orderUnitOfWork, IMapper mapper, IMediator mediator, IUnitOfWork<Coupon> couponUnitOfWork, IPaymentService paymentService)
    {
        _orderUnitOfWork = orderUnitOfWork;
        _couponUnitOfWork = couponUnitOfWork;

        _mapper = mapper;
        _mediator = mediator;
        _paymentService = paymentService;
    }

    public async Task<ApiResponse<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {

        var isUserExists = await CheckUser(request.OrderRequest.UserId);

        if (!isUserExists)
        {
            return new ApiResponse<OrderResponse>("User not found");
        }

        var cartItems = await _mediator.Send(new GetCartByUserIdQuery(request.OrderRequest.UserId));

        TotalAmountResponse totalAmountResponse = await CalculateTotalAmount(request.OrderRequest.UserId, request.OrderRequest.CouponCode, cartItems.Data);

        decimal totalAmount = totalAmountResponse.totalAmountAfterPointApplied;

        // fake payment servis olusturulacak

        var order = _mapper.Map<Order>(request.OrderRequest);

        order.BasketAmount = totalAmountResponse.BasketAmount;
        order.TotalAmount = totalAmountResponse.totalAmountAfterPointApplied;
        order.Status = Convert.ToInt16(Enums.OrderStatus.Approved);
        order.CouponAmount = totalAmountResponse.BasketAmount - totalAmountResponse.totalAmountAfterCouponApplied;
        order.PointAmount = totalAmountResponse.totalAmountAfterCouponApplied - totalAmount;

        await _orderUnitOfWork.Repository.Insert(order);
        await _orderUnitOfWork.CommitWithTransaction();

        await DecreaseStockCounts(cartItems.Data);
        await AddPointToDigitalWallet(cartItems.Data, request.OrderRequest.UserId);


        // deletes cart after order is created
        //await _mediator.Send(new DeleteCartCommand(request.OrderRequest.UserId));

        return new ApiResponse<OrderResponse>(_mapper.Map<OrderResponse>(order));
    }


    private async Task<TotalAmountResponse> CalculateTotalAmount(long userId, string couponCode, IEnumerable<CartResponse> cartItems)
    {
        decimal totalAmount = cartItems.Sum(item => item.Price * item.Quantity);

        decimal totalAmountAfterCouponApplied = totalAmount;

        if (!string.IsNullOrEmpty(couponCode))
        {
            totalAmountAfterCouponApplied = await ApplyCouponCode(totalAmount, couponCode);
        }

        var totalAmountAfterPointApplied = await ApplyPoint(userId, totalAmountAfterCouponApplied);

        return new TotalAmountResponse
        {
            BasketAmount = Convert.ToDecimal(totalAmount),
            totalAmountAfterCouponApplied = totalAmountAfterCouponApplied,
            totalAmountAfterPointApplied = Convert.ToDecimal(totalAmountAfterPointApplied)
        };
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

    private async Task<decimal> AddPointToDigitalWallet(IEnumerable<CartResponse> cartItems, long userId)
    {
        var totalPointAmount = 0m;

        foreach (var item in cartItems)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(item.ProductId));
            var pointAmount = (product.Data.Price * item.Quantity) * (product.Data.PointPercentage / 100);

            if (pointAmount > product.Data.MaxPoint)
            {
                pointAmount = product.Data.MaxPoint;
            }

            await _mediator.Send(new AddPointToDigitalWalletCommand(userId, pointAmount));
            totalPointAmount += pointAmount;
        }
        return totalPointAmount;
    }

    private async Task<decimal?> ApplyPoint(long userId, decimal? totalAmount)
    {

        var digitalWallet = await _mediator.Send(new GetDigitalWalletByUserIdQuery(userId));

        if (digitalWallet.Data.PointBalance > 0)
        {
            if (digitalWallet.Data.PointBalance >= totalAmount)
            {
                digitalWallet.Data.PointBalance -= totalAmount;
                totalAmount = 0;
            }
            else
            {
                totalAmount -= digitalWallet.Data.PointBalance;
                digitalWallet.Data.PointBalance = 0;
            }

            await _mediator.Send(new UpdateDigitalWalletCommand(digitalWallet.Data.Id, new DigitalWalletRequest
            {
                PointBalance = digitalWallet.Data.PointBalance,
                UserId = userId
            }));
        }

        return totalAmount;


    }
}
