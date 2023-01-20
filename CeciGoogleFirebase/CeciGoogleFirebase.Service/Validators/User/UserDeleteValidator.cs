using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Validators.User
{
    public class UserDeleteValidator : AbstractValidator<UserDeleteDTO>
    {
        private readonly IUnitOfWork _uow;

        public UserDeleteValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.UserId)
                .NotEmpty().WithMessage("Please enter the identifier user.")
                .NotNull().WithMessage("Please enter the identifier user.")
                .MustAsync(async (userId, cancellation) => {
                    return await UserValid(userId);
                }).WithMessage("User invalid.");
        }

        private async Task<bool> UserValid(int userId)
        {
            return await _uow.User.GetFirstOrDefaultAsync(x => x.Id.Equals(userId)) != null;
        }
    }
}
