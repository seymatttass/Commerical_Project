using FluentValidation;
using Stock.API.DTOS.Stock;

namespace Stock.API.DTOS.Validators
{
    public class UpdateStockDtoValidator : AbstractValidator<UpdateStockDTO>
    {
        public UpdateStockDtoValidator()
        {
            RuleFor(x => x.StockId)
                .NotEmpty().WithMessage("Stok id boş olamaz");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Ürün ıd alanı boş olamaz");

            RuleFor(x => x.Count) //ilçe
               .NotEmpty().WithMessage("Stok bilgisi boş olamaz");

        }
    }
}
