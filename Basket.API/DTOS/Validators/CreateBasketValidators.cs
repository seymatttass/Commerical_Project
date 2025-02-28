﻿using FluentValidation;
using Basket.API.DTOS;

namespace Basket.API.DTOS.Validators
{
    public class CreateBasketValidators : AbstractValidator<CreateBasketDTO>
    {
        public CreateBasketValidators()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.StockId)
                .GreaterThan(0).WithMessage("StockId must be greater than 0.");

            RuleFor(x => x.TotalPrice)
                .GreaterThanOrEqualTo(0).WithMessage("TotalPrice must be a positive value.");
        }
    }
}
