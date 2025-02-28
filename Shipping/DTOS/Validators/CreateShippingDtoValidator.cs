using FluentValidation;
using Shipping.API.DTOS.Shipping;

namespace Shipping.API.DTOS.Validators
{
    public class CreateShippingDtoValidator : AbstractValidator<CreateShippingDTO>
    {
        public CreateShippingDtoValidator()
        {
            RuleFor(x => x.CargoCompanyName)
                .NotEmpty().WithMessage("kargo şirket ismi boş olamaz");

            RuleFor(x => x.Active)
                .NotEmpty().WithMessage("aktiflik alanı boş olamaz");

            RuleFor(x => x.Shipcharge)
               .NotEmpty().WithMessage("Stok bilgisi boş olamaz");

            RuleFor(x => x.Free)
               .NotEmpty().WithMessage("kargo ücreti boş olamaz");


        }
    }
}
