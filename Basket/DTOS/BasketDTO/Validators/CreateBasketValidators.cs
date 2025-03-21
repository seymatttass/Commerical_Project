using FluentValidation;
using Basket.API.DTOS.BasketDTO.Basket;

namespace Basket.API.DTOS.BasketDTO.Validators
{
    public class CreateBasketValidators : AbstractValidator<CreateBasketDTO>
    {
        public CreateBasketValidators()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");


        }
    }
}
