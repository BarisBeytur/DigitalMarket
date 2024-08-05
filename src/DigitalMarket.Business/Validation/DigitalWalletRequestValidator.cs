using DigitalMarket.Schema.Request;
using FluentValidation;

namespace DigitalMarket.Business.Validation
{
    public class DigitalWalletRequestValidator : AbstractValidator<DigitalWalletRequest>
    {

        public DigitalWalletRequestValidator()
        {
            RuleFor(x => x.PointBalance)
                .NotEmpty()
                .WithMessage("PointBalance is required")
                .NotNull()
                .WithMessage("PointBalance is required")
                .GreaterThanOrEqualTo(0)
                .WithMessage("PointBalance must be greater than or equal to 0");


            RuleFor(x => x.UserId)
                .NotNull()
                .WithMessage("UserId is required")
                .NotEmpty()
                .WithMessage("UserId is required")
                .GreaterThan(0)
                .WithMessage("UserId must be greater than 0");
        }
    }
}
