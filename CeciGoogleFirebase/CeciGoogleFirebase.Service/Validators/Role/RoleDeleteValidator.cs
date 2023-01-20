using CeciGoogleFirebase.Domain.DTO.Role;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Validators.Role
{
    public class RoleDeleteValidator : AbstractValidator<RoleDeleteDTO>
    {
        private readonly IUnitOfWork _uow;

        public RoleDeleteValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(c => c.RoleId)
                .NotEmpty().WithMessage("Please enter the identifier role.")
                .NotNull().WithMessage("Please enter the identifier role.")
                .MustAsync(async (roleId, cancellation) => {
                    return await RoleValid(roleId);
                }).WithMessage("Role invalid.");
        }

        private async Task<bool> RoleValid(int roleId)
        {
            return await _uow.Role.GetFirstOrDefaultAsync(x => x.Id.Equals(roleId)) != null;
        }
    }
}
