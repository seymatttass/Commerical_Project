using FluentValidation;
using Basket.API.DTOS;

namespace Basket.API.DTOS.Validators
{
    public class CreateBasketItemValidators : AbstractValidator<CreateBasketItemDTO>
    {
        public CreateBasketItemValidators()
        {


            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Count)
                .GreaterThan(0).WithMessage("Count must be greater than 0.");
        }
    }
}
