using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Service.Validators.Address;
using FluentValidation.TestHelper;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.Address
{
    public class AddressIdentifierValidatorTest
    {
        private readonly AddressIdentifierValidator _validator;

        public AddressIdentifierValidatorTest()
        {
            _validator = new AddressIdentifierValidator();
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new AddressIdentifierDTO();

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.AddressId);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = new AddressIdentifierDTO
            {
                AddressId = 1
            };

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.AddressId);
        }
    }
}
