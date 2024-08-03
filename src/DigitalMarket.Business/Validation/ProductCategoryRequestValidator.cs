using DigitalMarket.Schema.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Business.Validation
{
    public class ProductCategoryRequestValidator : AbstractValidator<ProductCategoryRequest>
    {

        public ProductCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("CategoryId is required")
                .NotNull()
                .WithMessage("CategoryId is required")
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than 0");

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required")
                .NotNull()
                .WithMessage("ProductId is required")
                .GreaterThan(0)
                .WithMessage("ProductId must be greater than 0");
        }
    }
}
