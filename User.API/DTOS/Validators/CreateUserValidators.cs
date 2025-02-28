using FluentValidation;
using Users.API.DTOS.Users;

namespace Users.API.DTOS.Validators
{
    public class CreateUserValidators : AbstractValidator<CreateUserDTO>
    {
        public CreateUserValidators()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.")
                .MaximumLength(50).WithMessage("Password cannot exceed 50 characters.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

            RuleFor(x => x.Surname)
                .NotEmpty().WithMessage("Surname is required.")
                .MaximumLength(50).WithMessage("Surname cannot exceed 50 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MaximumLength(50).WithMessage("Email cannot exceed 50 characters.");

            RuleFor(x => x.Birthdate)
                .LessThan(DateTime.Now).WithMessage("Birthdate cannot be in the future.")
                .When(x => x.Birthdate.HasValue);

            RuleFor(x => x.TelNo)
                .MaximumLength(15).WithMessage("Phone number cannot exceed 15 characters.")
                .Matches(@"^\+?[0-9\s\-]+$").WithMessage("Invalid phone number format.")
                .When(x => !string.IsNullOrEmpty(x.TelNo));
        }
    }
}
