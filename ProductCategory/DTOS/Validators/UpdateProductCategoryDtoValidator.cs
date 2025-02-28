using FluentValidation;
using ProductCategory.DTOS.ProductCategory;

namespace ProductCategory.DTOS.Validators
{
    public class UpdateProductCategoryDtoValidator : AbstractValidator<UpdateProductCategoryDTO>
    {
        public UpdateProductCategoryDtoValidator()
        {

            RuleFor(x => x.ProductCategoryId)
                .NotEmpty().WithMessage("ProductCategory id boş olamaz");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ıd alanı boş olamaz");

            RuleFor(x => x.ProductId)
               .NotEmpty().WithMessage("Product Id bilgisi boş olamaz");
        }
    }
}
