using Address.API.DTOS.Address;
using FluentValidation;

namespace Address.API.DTOS.Validators
{
    public class UpdateAddressDtoValidator : AbstractValidator<UpdateAddressDTO>
    {
        public UpdateAddressDtoValidator()
        {
            RuleFor(x => x.AddressId)
                .NotEmpty().WithMessage("Id boş olamaz");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("başlık alanı boş olamaz");

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Ülke bilgisi boş olamaz")
                .MaximumLength(500).WithMessage("Ülke bilgisi en fazla 500 karakter olabilir");

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Şehir bilgisi boş olamaz")
                .MaximumLength(50).WithMessage("Şehir bilgisi en fazla 50 karakter olabilir");

            RuleFor(x => x.District) //ilçe
                .NotEmpty().WithMessage("kategori açıklaması boş olamaz")
                .MaximumLength(500).WithMessage("kategori açıklaması en fazla 500 karakter olabilir");

            RuleFor(x => x.PostalCode)
                .NotEmpty().WithMessage("Posta kodu alanı boş olamaz")
                .MaximumLength(5).WithMessage("Posta kodu en fazla 5 karakter olabilir");

            RuleFor(x => x.AddressText)
                .NotEmpty().WithMessage("Adres açıklaması boş olamaz")
                .MaximumLength(500).WithMessage("Adres açıklaması en fazla 500 karakter olabilir");
        }
    }
}
