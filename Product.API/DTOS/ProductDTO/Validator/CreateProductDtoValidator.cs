using FluentValidation;
using Product.API.DTOS.ProductDTO.Product;

namespace Product.API.DTOS.ProductDTO.Validator
{
    public class CreateProductDtoValidator : AbstractValidator<UpdateProductDTO>
    {
        public CreateProductDtoValidator()
        {
            RuleFor(x => x.ProductCategoryId)
                .NotEmpty().WithMessage("ürün id boş olamaz.");

            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("ürün kodu id boş olamaz");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün ıd alanı boş olamaz");

            RuleFor(x => x.Price)
               .NotEmpty().WithMessage("ürün fiyatı bilgisi boş olamaz");
        }
    }
}
