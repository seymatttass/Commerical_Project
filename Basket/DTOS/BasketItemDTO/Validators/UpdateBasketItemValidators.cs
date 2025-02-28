using FluentValidation;
using Basket.API.DTOS;

namespace Basket.API.DTOS.Validators
{
    public class UpdateBasketItemValidators : AbstractValidator<UpdateBasketItemDTO>
    {
        public UpdateBasketItemValidators()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.BasketId)
                .GreaterThan(0).WithMessage("BasketId must be greater than 0.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.");

            RuleFor(x => x.Count)
                .GreaterThan(0).WithMessage("Count must be greater than 0.");
        }
    }
}
