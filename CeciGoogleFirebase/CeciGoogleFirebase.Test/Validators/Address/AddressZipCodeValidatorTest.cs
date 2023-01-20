using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Service.Validators.Address;
using FluentValidation.TestHelper;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.Address
{
    public class AddressZipCodeValidatorTest
    {
        private readonly AddressZipCodeValidator _validator;

        public AddressZipCodeValidatorTest()
        {
            _validator = new AddressZipCodeValidator();
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new AddressZipCodeDTO();

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.ZipCode);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = new AddressZipCodeDTO
            {
                ZipCode = "72000000"
            };

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.ZipCode);
        }
    }
}
