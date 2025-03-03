using FluentValidation;
using InvoiceDetails.DTOS.InvoiceDetails;

namespace InvoiceDetails.DTOS.Validators
{
    public class UpdateInvoiceDetailsDtoValidator : AbstractValidator<UpdateInvoiceDetailsDTO>
    {
        public UpdateInvoiceDetailsDtoValidator()
        {
            RuleFor(x => x.InvoiceDetailsId)
    .NotEmpty().WithMessage("faturadetay id alanı boş olamaz");

            RuleFor(x => x.InvoiceId)
                .NotEmpty().WithMessage("fatura id bilgisi boş olamaz");

            RuleFor(x => x.OrderItemId)
                .NotEmpty().WithMessage("sipariş detay id bilgisi boş olamaz");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ürün id alanı boş olamaz");

        }
    }
}
