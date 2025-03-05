using FluentValidation;
using Product.API.DTOS.CategoryDTO.Category;

namespace Product.API.DTOS.CategoryDTO.Validator
{
    public class CreateCategoryDtoValidator : AbstractValidator<UpdateCategoryDTO>
    {
        public CreateCategoryDtoValidator()
        {

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Stok id boş olamaz");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Ürün ıd alanı boş olamaz");

            RuleFor(x => x.Active)
               .NotEmpty().WithMessage("Stok bilgisi boş olamaz");
        }

    }
}
