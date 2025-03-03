using FluentValidation;
using ProductShippinng.API.DTOS.ProductShipping;

namespace ProductShippinng.API.DTOS.Validators
{
    public class UpdateProductShippingDtoValidator : AbstractValidator<UpdateProductShippingDTO>
    {
        public UpdateProductShippingDtoValidator()
        {

            // RuleFor(x => x.ProductShippingId)
            //    .NotEmpty().WithMessage("ProductShippingId Id boş olamaz");

            RuleFor(x => x.ProductId)
                    .NotEmpty().WithMessage("ürün id alanı boş olamaz");

            RuleFor(x => x.ShippingId)
                    .NotEmpty().WithMessage("taşıma id bilgisi boş olamaz");

        }
    }
}
