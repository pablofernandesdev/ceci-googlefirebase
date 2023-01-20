using CeciGoogleFirebase.Domain.DTO.Notification;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Validators.Notification
{
    public class NotificationSendValidator : AbstractValidator<NotificationSendDTO>
    {
        private readonly IUnitOfWork _uow;

        public NotificationSendValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.IdUser)
              .NotNull().WithMessage("Please enter the user identifier.")
              .MustAsync(async (userId, cancellation) => {
                  return await UserValid(userId);
              }).WithMessage("User invalid.");

            RuleFor(c => c.Title)
               .NotEmpty().WithMessage("Please enter the notification title.")
               .NotNull().WithMessage("Please enter the notification title.");

            RuleFor(c => c.Body)
               .NotEmpty().WithMessage("Please enter the notification body.")
               .NotNull().WithMessage("Please enter the notification body.");
        }

        private async Task<bool> UserValid(int userId)
        {
            return await _uow.User.GetFirstOrDefaultAsync(x => x.Id.Equals(userId)) != null;
        }
    }
}
