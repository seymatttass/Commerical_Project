using FluentValidation;
using Invoice.API.DTOS.Invoice;

namespace Invoice.API.DTOS.Validators
{
    public class UpdateInvoiceDtoValidator : AbstractValidator<UpdateInvoiceDTO>
    {
        public UpdateInvoiceDtoValidator()
        {

            RuleFor(x => x.InvoiceId)
                .NotEmpty().WithMessage("fatura id alanı boş olamaz");

            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("sipariş id alanı boş olamaz");

            RuleFor(x => x.Cargoficheno)
                .NotEmpty().WithMessage("kargo fiş numarası boş olamaz");

            RuleFor(x => x.TotalPrice)
                .NotEmpty().WithMessage("toplam fiyat alanı boş olamaz");

        }
    }
}
