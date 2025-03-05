using FluentValidation;
using Product.API.DTOS.CategoryDTO.Category;

namespace Product.API.DTOS.CategoryDTO.Validator
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDTO>
    {
        public UpdateCategoryDtoValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Kategori id boş olamaz.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Stok id boş olamaz");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Ürün ıd alanı boş olamaz");

            RuleFor(x => x.Active)
               .NotEmpty().WithMessage("Stok bilgisi boş olamaz");
        }
    }
}
