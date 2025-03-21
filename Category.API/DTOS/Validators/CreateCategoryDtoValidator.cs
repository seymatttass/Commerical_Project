using Category.API.DTOS.Category;
using FluentValidation;

namespace Category.API.DTOS.Validators
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

