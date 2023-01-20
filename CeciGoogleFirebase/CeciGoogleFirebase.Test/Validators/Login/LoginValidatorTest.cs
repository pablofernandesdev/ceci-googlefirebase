using CeciGoogleFirebase.Domain.DTO.Auth;
using CeciGoogleFirebase.Service.Validators;
using CeciGoogleFirebase.Service.Validators.Login;
using CeciGoogleFirebase.Test.Fakers.Auth;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.Login
{
    public class LoginValidatorTest
    {
        private readonly LoginValidator _validator;

        public LoginValidatorTest()
        {
            _validator = new LoginValidator();
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new LoginDTO();

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.Username);
            result.ShouldHaveValidationErrorFor(user => user.Password);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = AuthFaker.LoginDTO().Generate();

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.Username);
            result.ShouldNotHaveValidationErrorFor(user => user.Password);
        }
    }
}
