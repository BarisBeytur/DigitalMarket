using DigitalMarket.Schema.Request;
using FluentValidation;

namespace DigitalMarket.Business.Validation
{
    public class CouponRequestValidator : AbstractValidator<CouponRequest>
    {
        public CouponRequestValidator()
        {
            
            RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage("Code is required")
                .NotNull()
                .WithMessage("Code is required")
                .MaximumLength(10)
                .WithMessage("Code must be less than 10 characters");

            RuleFor(x => x.Discount)
                .NotEmpty()
                .WithMessage("Discount is required")
                .NotNull()
                .WithMessage("Discount is required")
                .GreaterThan(0)
                .WithMessage("Discount must be greater than 0");
        }
    }
}
