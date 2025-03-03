using FluentValidation;
using ProductShipping.DTOS.ProductShipping;

namespace ProductShipping.DTOS.Validators
{
    public class CreateProductShippingDtoValidator : AbstractValidator<CreateProductShippingDTO>
    {
        public CreateProductShippingDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId boş olamaz");
 
            RuleFor(x => x.ShippingId)
                .NotEmpty().WithMessage("ShippingId boş olamaz");
        }
    }
}
