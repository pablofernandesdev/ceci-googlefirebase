using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Validators.Address
{
    public class AddressAddValidator : AbstractValidator<AddressAddDTO>
    {
        private readonly IUnitOfWork _uow;

        public AddressAddValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("Please enter the identifier user.")
                .NotNull().WithMessage("Please enter the identifier user.")
                .MustAsync(async (userId, cancellation) => {
                    return await UserValid(userId);
                }).WithMessage("User invalid.");

            RuleFor(c => c.ZipCode)
                .NotEmpty().WithMessage("Please enter the zip code.")
                .NotNull().WithMessage("Please enter the zip code.");

            RuleFor(c => c.Street)
                .NotEmpty().WithMessage("Please enter the street.")
                .NotNull().WithMessage("Please enter the street.");

            RuleFor(c => c.District)
                 .NotEmpty().WithMessage("Please enter the district.")
                 .NotNull().WithMessage("Please enter the district.");

            RuleFor(c => c.Locality)
                 .NotEmpty().WithMessage("Please enter the locality.")
                 .NotNull().WithMessage("Please enter the locality.");

            RuleFor(c => c.Number)
                 .NotEmpty().WithMessage("Please enter the number.")
                 .NotNull().WithMessage("Please enter the number.");

            RuleFor(c => c.Uf)
                 .MaximumLength(2)
                 .NotEmpty().WithMessage("Please enter the uf.")
                 .NotNull().WithMessage("Please enter the uf.");
        }

        private async Task<bool> UserValid(int userId)
        {
            return await _uow.User.GetFirstOrDefaultAsync(x => x.Id.Equals(userId)) != null;
        }
    }
}
