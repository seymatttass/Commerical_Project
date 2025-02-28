using FluentValidation;
using Shipping.API.DTOS.Shipping;

namespace Shipping.API.DTOS.Validators
{
    public class UpdateShippingDtoValidator : AbstractValidator<UpdateShippingDTO>
    {
        public UpdateShippingDtoValidator()
        {
            RuleFor(x => x.ShippingId)
                .NotEmpty().WithMessage("taşıma id boş olamaz");

            RuleFor(x => x.CargoCompanyName)
                .NotEmpty().WithMessage("taşıma ad alanı boş olamaz");

            RuleFor(x => x.Active)
               .NotEmpty().WithMessage("taşıma aktifliği bilgisi boş olamaz");

            RuleFor(x => x.Shipcharge)
                .NotEmpty().WithMessage("taşıma ücreti boş olamaz");

            RuleFor(x => x.Free)
                .NotEmpty().WithMessage("taşıma ücreti alanı boş olamaz");

            RuleFor(x => x.EstimatedDays)
               .NotEmpty().WithMessage("tahmini taşıma süresi boş olamaz");
        }
    }
}
