using FluentValidation;
using Order.API.DTOS.OrdersDTO.Orders;

namespace Order.API.DTOS.OrdersDTO.Validators
{
    public class CreateOrdersValidator : AbstractValidator<CreateOrdersDTO>
    {
        public CreateOrdersValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("CustomerId must be greater than zero.");

            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("OrderDate cannot be empty.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("OrderDate cannot be in the future.");

            RuleFor(x => x.TotalAmount)
                .GreaterThan(0).WithMessage("TotalAmount must be a positive value.");
        }
    }
}
