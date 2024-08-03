using DigitalMarket.Schema.Request;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalMarket.Business.Validation
{
    public class UserRequestValidator : AbstractValidator<UserRequest>
    {
        public UserRequestValidator()
        {
            
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .NotNull()  
                .WithMessage("Name is required")
                .MaximumLength(50)
                .WithMessage("Name cannot be longer than 50 characters");

            RuleFor(x => x.Surname)
                .NotEmpty()
                .WithMessage("Surname is required")
                .NotNull()
                .WithMessage("Surname is required")
                .MaximumLength(50)
                .WithMessage("Surname cannot be longer than 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .NotNull()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email is not valid");

            RuleFor(x => x.Role)
                .NotEmpty()
                .WithMessage("Role is required")
                .NotNull()
                .WithMessage("Role is required")
                .MaximumLength(50)
                .WithMessage("Role cannot be longer than 50 characters");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .NotNull()
                .WithMessage("Password is required")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long");

            RuleFor(x => x.Status)
                .NotEmpty()
                .WithMessage("Status is required")
                .NotNull()
                .WithMessage("Status is required");

        }
    }
}
