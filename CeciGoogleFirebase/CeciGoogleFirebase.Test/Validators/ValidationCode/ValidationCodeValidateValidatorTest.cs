using CeciGoogleFirebase.Domain.DTO.ValidationCode;
using CeciGoogleFirebase.Service.Validators.ValidationCode;
using CeciGoogleFirebase.Test.Fakers.ValidationCode;
using FluentValidation.TestHelper;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.ValidationCode
{
    public class ValidationCodeValidateValidatorTest
    {
        private readonly ValidationCodeValidateValidator _validator;

        public ValidationCodeValidateValidatorTest()
        {
            _validator = new ValidationCodeValidateValidator();
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new ValidationCodeValidateDTO();

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.Code);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = ValidationCodeFaker.ValidationCodeValidateDTO().Generate();

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.Code);
        }
    }
}
