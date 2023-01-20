using CeciGoogleFirebase.Domain.DTO.Role;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using FluentValidation;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Validators.Role
{
    public class RoleUpdateValidator : AbstractValidator<RoleUpdateDTO>
    {
        private readonly IUnitOfWork _uow;

        public RoleUpdateValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.RoleId)
                .NotNull().WithMessage("Please enter the role.")
                .MustAsync(async (roleId, cancellation) => {
                    return await RoleValid(roleId);
                }).WithMessage("Role invalid.");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Please enter the name.")
                .NotNull().WithMessage("Please enter the name.");
        }

        private async Task<bool> RoleValid(int roleId)
        {
            return await _uow.Role.GetFirstOrDefaultAsync(x => x.Id.Equals(roleId)) != null;
        }
    }
}
