using DigitalMarket.Schema.Request;
using FluentValidation;

namespace DigitalMarket.Business.Validation
{
    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        public OrderRequestValidator()
        {
            
            RuleFor(x => x.TotalAmount)
                .NotEmpty()
                .WithMessage("TotalAmount must be not empty")
                .NotNull()
                .WithMessage("TotalAmount must be not null")
                .GreaterThan(0)
                .WithMessage("TotalAmount must be greater than 0");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId must be not empty")
                .NotNull()
                .WithMessage("UserId must be not null")
                .GreaterThan(0)
                .WithMessage("UserId must be greater than 0");

            RuleFor(x => x.CouponCode)
                .MaximumLength(10)
                .WithMessage("CouponCode must be less than 10 characters");

        }
    }
}
