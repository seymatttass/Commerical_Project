using FluentValidation;
using InvoiceDetails.DTOS.InvoiceDetails;

namespace InvoiceDetails.DTOS.Validators
{
    public class CreateInvoiceDetailsDtoValidator : AbstractValidator<CreateInvoiceDetailsDTO>
    {
        public CreateInvoiceDetailsDtoValidator()
        {
            RuleFor(x => x.InvoiceId)
                .NotEmpty().WithMessage("fatura id alanı boş olamaz");

            RuleFor(x => x.OrderItemId)
                .NotEmpty().WithMessage("orderitem id bilgisi boş olamaz");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ürün id bilgisi boş olamaz");


        }
    }
}
