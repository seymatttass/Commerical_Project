using Stock.API.DTOS.Stock;
using FluentValidation;
using Stock.API.DTOS.Stock;

namespace Stock.API.DTOS.Validators
{
    public class CreateStockDtoValidator : AbstractValidator<CreateStockDTO>
    {
        public CreateStockDtoValidator()
        {

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Ürün ıd alanı boş olamaz");

            RuleFor(x => x.Count) //ilçe
               .NotEmpty().WithMessage("Stok bilgisi boş olamaz");
        }
    }
}
