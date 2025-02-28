using FluentValidation;
using ProductCategory.DTOS.ProductCategory;

namespace ProductCategory.DTOS.Validators
{
    public class CreateProductCategoryDtoValidator : AbstractValidator<UpdateProductCategoryDTO>
    {
        public CreateProductCategoryDtoValidator()
        {

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ıd alanı boş olamaz");

            RuleFor(x => x.ProductId)
               .NotEmpty().WithMessage("Product Id bilgisi boş olamaz");
        }
    }
}
