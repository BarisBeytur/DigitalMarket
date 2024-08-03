using DigitalMarket.Schema.Request;
using FluentValidation;

namespace DigitalMarket.Business.Validation
{
    public class OrderDetailRequestValidator : AbstractValidator<OrderDetailRequest>
    {
        public OrderDetailRequestValidator()
        {
            
            RuleFor(x => x.OrderId)
                .NotEmpty()
                .WithMessage("OrderId is required")
                .NotNull()
                .WithMessage("OrderId is required")
                .GreaterThan(0)
                .WithMessage("OrderId must be greater than 0");

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required")
                .NotNull()
                .WithMessage("ProductId is required")
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0");

            RuleFor(x => x.Quantity)
                .NotEmpty()
                .WithMessage("Quantity is required")
                .NotNull()
                .WithMessage("Quantity is required")    
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Price is required")
                .NotNull()
                .WithMessage("Price is required")
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor(x => x.PointAmount)
                .NotEmpty()
                .WithMessage("PointAmount is required")
                .NotNull()
                .WithMessage("PointAmount is required")
                .GreaterThanOrEqualTo(0)
                .WithMessage("PointAmount must be greater than or equal 0");
        }
    }
}
