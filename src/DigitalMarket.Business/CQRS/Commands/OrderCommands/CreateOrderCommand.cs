using AutoMapper;
using Azure.Core;
using DigitalMarket.Base.Enums;
using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.CartCommands;
using DigitalMarket.Business.CQRS.Commands.DigitalWalletCommands;
using DigitalMarket.Business.CQRS.Commands.OrderDetailCommands;
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
    public PaymentRequest PaymentRequest { get; set; }

    public CreateOrderCommand(OrderRequest orderRequest, PaymentRequest paymentRequest)
    {
        OrderRequest = orderRequest;
        PaymentRequest = paymentRequest;
    }
}

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ApiResponse<OrderResponse>>
{

    private readonly IUnitOfWork<Order> _orderUnitOfWork;
    private readonly IUnitOfWork<Coupon> _couponUnitOfWork;
    private readonly IMediator _mediator;
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;


    public CreateOrderCommandHandler(IUnitOfWork<Order> orderUnitOfWork, IMapper mapper, IMediator mediator, IUnitOfWork<Coupon> couponUnitOfWork, IPaymentService paymentService, IUnitOfWork<OrderDetail> orderDetailUnitOfWork)
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
            return new ApiResponse<OrderResponse>("User not found");

        var cartItems = await _mediator.Send(new GetCartByUserIdQuery(request.OrderRequest.UserId));
        await CheckStock(cartItems.Data);
        TotalAmountResponse totalAmountResponse = await CalculateTotalAmount(request.OrderRequest.UserId, request.OrderRequest.CouponCode, cartItems.Data);
        decimal totalAmount = totalAmountResponse.totalAmountAfterPointApplied;
        decimal originalTotalAmount = totalAmountResponse.BasketAmount;

        var order = _mapper.Map<Order>(request.OrderRequest);

        order.OrderNumber = GenerateOrderNumber();
        order.BasketAmount = originalTotalAmount;
        order.TotalAmount = totalAmountResponse.totalAmountAfterPointApplied;
        order.Status = Convert.ToInt16(Enums.OrderStatus.Approved);
        order.CouponAmount = originalTotalAmount - totalAmountResponse.totalAmountAfterCouponApplied;
        order.PointAmount = totalAmountResponse.totalAmountAfterCouponApplied - totalAmount;

        var paymentResult = await _paymentService.GetPayment(request.OrderRequest.UserId, request.PaymentRequest);

        if (paymentResult.IsSuccess)
        {
            order.Status = Convert.ToInt16(Enums.OrderStatus.Delivered);
            await _orderUnitOfWork.Repository.Insert(order);
            await _orderUnitOfWork.CommitWithTransaction();

            await DecreasePoint(request.OrderRequest.UserId, totalAmountResponse);
            await DecreaseStockCounts(cartItems.Data);
            await AddPointToDigitalWallet(cartItems.Data, request.OrderRequest.UserId, totalAmount, originalTotalAmount);
            await _mediator.Send(new DeleteCartCommand(request.OrderRequest.UserId));
            await CreateOrderDetails(cartItems.Data,order.Id);
         
            return new ApiResponse<OrderResponse>(_mapper.Map<OrderResponse>(order));
        }
        else
        {
            return new ApiResponse<OrderResponse>("Payment failed");
        }

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
            totalAmountAfterPointApplied = Convert.ToDecimal(totalAmountAfterPointApplied.TotalAmount)
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

    private async Task<decimal> AddPointToDigitalWallet(IEnumerable<CartResponse> cartItems, long userId, decimal paidAmount, decimal originalTotalAmount)
    {
        var totalPointAmount = 0m;

        if (paidAmount <= 0)
        {
            return totalPointAmount;
        }

        var paymentRatio = paidAmount / originalTotalAmount;

        foreach (var item in cartItems)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(item.ProductId));

            var potentialPointAmount = (item.Price * item.Quantity) * (product.Data.PointPercentage / 100);

            if (potentialPointAmount > product.Data.MaxPoint)
            {
                potentialPointAmount = product.Data.MaxPoint;
            }

            var pointAmount = potentialPointAmount * paymentRatio;

            await _mediator.Send(new AddPointToDigitalWalletCommand(userId, pointAmount));
            totalPointAmount += pointAmount;
        }

        return totalPointAmount;
    }

    private async Task<ApplyPointResponse> ApplyPoint(long userId, decimal? totalAmount)
    {

        var digitalWallet = await _mediator.Send(new GetDigitalWalletByUserIdQuery(userId));
        decimal? totalAmountForOpr = totalAmount;

        if (digitalWallet.Data.PointBalance >= totalAmountForOpr)
        {
            digitalWallet.Data.PointBalance -= totalAmountForOpr;
            totalAmountForOpr = 0;
        }
        else
        {
            totalAmountForOpr -= digitalWallet.Data.PointBalance;
            digitalWallet.Data.PointBalance = 0;
        }

        var response = new ApplyPointResponse
        {
            PointBalance = digitalWallet.Data.PointBalance,
            TotalAmount = totalAmountForOpr
        };
        return response;
    }

    private async Task DecreasePoint(long userId, TotalAmountResponse totalAmountResponse)
    {
        var digitalWallet = await _mediator.Send(new GetDigitalWalletByUserIdQuery(userId));

        ApplyPointResponse applyPointResponse = await ApplyPoint(userId, totalAmountResponse.totalAmountAfterCouponApplied);

        await _mediator.Send(new UpdateDigitalWalletCommand(digitalWallet.Data.Id, new DigitalWalletRequest
        {
            PointBalance = applyPointResponse.PointBalance,
            UserId = userId
        }));
    }

    private async Task CheckStock(IEnumerable<CartResponse> cartItems)
    {
        foreach (var item in cartItems)
        {
            var product = await _mediator.Send(new GetProductByIdQuery(item.ProductId));

            if (product.Data.StockCount < item.Quantity)
            {
                throw new Exception($"{product.Data.Name} is out of stock");
            }
        }
    }

    private string GenerateOrderNumber()
    {
        Random random = new Random();
        return random.Next(100000000, 999999999).ToString();
    }

    private async Task CreateOrderDetails(IEnumerable<CartResponse> cartItems, long orderId)
    {
        foreach (var item in cartItems)
        {
            await _mediator.Send(new CreateOrderDetailCommand(new OrderDetailRequest
            {
                OrderId = orderId,
                Price = item.Price,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
            }));
        }

    }
}
