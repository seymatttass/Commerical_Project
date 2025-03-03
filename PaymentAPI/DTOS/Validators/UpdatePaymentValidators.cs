using FluentValidation;
using Payment.API.DTOS.Payments;

namespace Payment.API.DTOS.Validators
{
    public class UpdatePaymentValidators : AbstractValidator<UpdatePaymentDTO>
    {
        public UpdatePaymentValidators()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("OrderId must be greater than 0.");

            RuleFor(x => x.BasketId)
                .GreaterThan(0).WithMessage("BasketId must be greater than 0.");

            RuleFor(x => x.PaymentType)
                .GreaterThan(0).WithMessage("PaymentType must be greater than 0.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Date cannot be in the future.");

            RuleFor(x => x.PaymentTotal)
                .GreaterThan(0).WithMessage("PaymentTotal must be greater than zero.");
        }
    }
}
