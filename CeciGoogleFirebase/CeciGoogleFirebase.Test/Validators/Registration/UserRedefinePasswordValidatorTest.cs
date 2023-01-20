using CeciGoogleFirebase.Domain.DTO.Register;
using CeciGoogleFirebase.Infra.CrossCutting.Helper;
using CeciGoogleFirebase.Service.Validators.Registration;
using FluentValidation.TestHelper;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.Registration
{
    public class UserRedefinePasswordValidatorTest
    {
        private readonly UserRedefinePasswordValidator _validator;

        public UserRedefinePasswordValidatorTest()
        {
            _validator = new UserRedefinePasswordValidator();
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new UserRedefinePasswordDTO();

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.CurrentPassword);
            result.ShouldHaveValidationErrorFor(user => user.NewPassword);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = new UserRedefinePasswordDTO
            {
                CurrentPassword = StringHelper.Base64Encode("abc"),
                NewPassword = StringHelper.Base64Encode("abc")
            };

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.CurrentPassword);
            result.ShouldNotHaveValidationErrorFor(user => user.NewPassword);
        }
    }
}
