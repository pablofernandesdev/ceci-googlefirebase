using CeciGoogleFirebase.Domain.DTO.Auth;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CeciGoogleFirebase.Service.Validators.Login
{
    public class LoginValidator : AbstractValidator<LoginDTO>
    {
        public LoginValidator()
        {
            RuleFor(c => c.Username)
                .NotEmpty().WithMessage("Please enter the username.")
                .NotNull().WithMessage("Please enter the username.");

            RuleFor(c => c.Password)
                .NotEmpty().WithMessage("Please enter the password.")
                .NotNull().WithMessage("Please enter the password.");
        }
    }
}
