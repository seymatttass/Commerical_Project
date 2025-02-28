using FluentValidation;
using Product.API.DTOS.Product;

namespace Product.API.DTOS.Validators
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDTO>
    {

        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ürün id boş olamaz.");

            RuleFor(x => x.ProductCategoryId)
               .NotEmpty().WithMessage("ProductCategoryId bilgisi boş olamaz");

            RuleFor(x => x.Code)
              .NotEmpty().WithMessage("ürün kodu boş olamaz.");

            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("ürün ismi bilgisi boş olamaz");

            RuleFor(x => x.Price)
              .NotEmpty().WithMessage("ürün fiyatı boş olamaz.");


        }

    }
}
