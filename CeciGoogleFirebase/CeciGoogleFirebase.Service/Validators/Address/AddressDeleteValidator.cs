using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Validators.Address
{
    public class AddressDeleteValidator : AbstractValidator<AddressDeleteDTO>
    {
        private readonly IUnitOfWork _uow;

        public AddressDeleteValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.AddressId)
                .NotEmpty().WithMessage("Please enter the address id.")
                .NotNull().WithMessage("Please enter the address id.")
                .MustAsync(async (addressId, cancellation) => {
                    return await AddressValid(addressId);
                }).WithMessage("Address invalid.");           
        }

        private async Task<bool> AddressValid(int addressId)
        {
            return await _uow.Address.GetFirstOrDefaultAsync(x => x.Id.Equals(addressId)) != null;
        }
    }
}
