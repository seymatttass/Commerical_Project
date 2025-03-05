using FluentValidation;
using Order.API.DTOS.OrdersDTO.Orders;

namespace Order.API.DTOS.OrdersDTO.Validators
{
    public class UpdateOrdersValidator : AbstractValidator<UpdateOrdersDTO>
    {
        public UpdateOrdersValidator()
        {
            RuleFor(x => x.ID)
                .GreaterThan(0).WithMessage("ID must be greater than zero.");

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
