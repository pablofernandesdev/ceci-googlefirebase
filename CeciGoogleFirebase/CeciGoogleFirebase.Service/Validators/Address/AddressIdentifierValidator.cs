using CeciGoogleFirebase.Domain.DTO.Address;
using FluentValidation;

namespace CeciGoogleFirebase.Service.Validators.Address
{
    public class AddressIdentifierValidator : AbstractValidator<AddressIdentifierDTO>
    {
        public AddressIdentifierValidator()
        {
            RuleFor(c => c.AddressId)
                .NotEmpty().WithMessage("Please enter the address id.")
                .NotNull().WithMessage("Please enter the address id.");
        }
    }
}
