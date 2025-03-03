using FluentValidation;
using Invoice.API.DTOS.Invoice;

namespace Invoice.API.DTOS.Validators
{
    public class CreateInvoiceDtoValidator : AbstractValidator<CreateInvoiceDTO>
    {
        public CreateInvoiceDtoValidator()
        {

            RuleFor(x => x.OrderId)
                .NotEmpty().WithMessage("sipariş id boş olamaz");

            RuleFor(x => x.Cargoficheno)
                .NotEmpty().WithMessage("kargo fiş numarası boş olamaz");

            RuleFor(x => x.TotalPrice)
                .NotEmpty().WithMessage("toplam fiyat alanı boş olamaz");

        }
    }
}
