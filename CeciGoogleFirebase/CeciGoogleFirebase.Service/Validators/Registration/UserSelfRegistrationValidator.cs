using CeciGoogleFirebase.Domain.DTO.Register;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Infra.CrossCutting.Helper;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Validators.Registration
{
    public class UserSelfRegistrationValidator : AbstractValidator<UserSelfRegistrationDTO>
    {
        private readonly IUnitOfWork _uow;

        public UserSelfRegistrationValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please enter the name.")
                .NotNull().WithMessage("Please enter the name.");

            RuleFor(c => c.Email)
                .EmailAddress()
                .MustAsync(async (email, cancellation) => {
                    return !await RegisteredEmail(email);
                }).WithMessage("E-mail already registered.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please enter the password.")
                .NotNull().WithMessage("Please enter the password.");

            When(c => !string.IsNullOrEmpty(c.Password), () => {
                RuleFor(c => c.Password)
                    .Must(c => StringHelper.IsBase64String(c))
                    .WithMessage("Password must be base64 encoded.");
            });
        }

        private async Task<bool> RegisteredEmail(string email)
        {
            return await _uow.User.GetFirstOrDefaultAsync(x => x.Email.Equals(email)) != null;
        }
    }
}
