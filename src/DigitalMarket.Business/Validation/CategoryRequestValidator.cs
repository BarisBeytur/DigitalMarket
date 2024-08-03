using DigitalMarket.Schema.Request;
using FluentValidation;

namespace DigitalMarket.Business.Validation
{
    public class CategoryRequestValidator : AbstractValidator<CategoryRequest>
    {
        public CategoryRequestValidator()
        {
            
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .NotNull()
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters");

            RuleFor(x => x.Url)
                .NotEmpty()
                .WithMessage("Url is required")
                .NotNull()
                .WithMessage("Url is required")
                .MaximumLength(100)
                .WithMessage("Url must not exceed 100 characters");

            RuleFor(x => x.Tags)
                .NotEmpty()
                .WithMessage("Tags is required")
                .NotNull()
                .WithMessage("Tags is required")
                .MaximumLength(100)
                .WithMessage("Tags must not exceed 100 characters");
        }
    }
}
