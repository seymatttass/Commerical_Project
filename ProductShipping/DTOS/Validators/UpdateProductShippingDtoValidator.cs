using FluentValidation;
using ProductShipping.DTOS.ProductShipping;

namespace ProductShipping.DTOS.Validators
{
    public class UpdateProductShippingDtoValidator : AbstractValidator<UpdateProductShippingDTO>
    {
        public UpdateProductShippingDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId boş olamaz");

            RuleFor(x => x.ShippingId)
                .NotEmpty().WithMessage("ShippingId boş olamaz");
        }
    }
}
