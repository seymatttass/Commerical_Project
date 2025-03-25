using FluentValidation;
using Basket.API.DTOS.BasketDTO.Basket;

namespace Basket.API.DTOS.BasketDTO.Validators
{
    public class UpdateBasketValidators : AbstractValidator<UpdateBasketDTO>
    {
        public UpdateBasketValidators()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.StockId)
                .GreaterThan(0).WithMessage("StockId must be greater than 0.");
        }
    }
}
