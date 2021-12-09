using FluentValidation;
using Ordering.Domain.Entities;

namespace Ordering.BusinessLogic.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(o => o.Username)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("User name is required.")
                .NotNull()
                .MaximumLength(50)
                .WithMessage("User name must not exceed 50 characters.");

            RuleFor(o => o.EmailAddress)
                    .NotEmpty()
                    .WithMessage("Email address is required"); 
            RuleFor(o => o.TotalPrice)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Total price is required")
                .GreaterThan(0).WithMessage("Total price should be greater than 0");
        }
    }
}
