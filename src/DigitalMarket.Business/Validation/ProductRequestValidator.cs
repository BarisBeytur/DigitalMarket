using DigitalMarket.Schema.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Business.Validation
{
    public class ProductRequestValidator : AbstractValidator<ProductRequest>
    {
        public ProductRequestValidator()
        {
            
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .NotNull()
                .WithMessage("Name is required")
                .MaximumLength(100)
                .WithMessage("Name must be less than 100 characters");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Description is required")
                .NotNull()
                .WithMessage("Description is required")
                .MaximumLength(500)
                .WithMessage("Description must be less than 500 characters");

            RuleFor(x => x.StockCount)
                .NotEmpty()
                .WithMessage("Stock count is required")
                .NotNull()
                .WithMessage("Stock count is required")
                .GreaterThan(0)
                .WithMessage("Stock count must be greater than 0");

            RuleFor(x => x.Price)
                .NotEmpty()
                .WithMessage("Price is required")
                .NotNull()
                .WithMessage("Price is required")
                .GreaterThan(0)
                .WithMessage("Price must be greater than 0");

            RuleFor(x => x.PointPercentage)
                .NotEmpty()
                .WithMessage("Point percentage is required")
                .NotNull()
                .WithMessage("Point percentage is required")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Point percentage must be greater than or equal to 0");

            RuleFor(x => x.MaxPoint)
                .NotNull()
                .WithMessage("Max point is required")
                .NotEmpty()
                .WithMessage("Max point is required")
                .GreaterThanOrEqualTo(0)
                .WithMessage("Max point must be greater than or equal to 0");

            RuleFor(x => x.IsActive)
                .NotNull()
                .WithMessage("Is active is required")
                .NotEmpty()
                .WithMessage("Is active is required");

        }
    }
}
