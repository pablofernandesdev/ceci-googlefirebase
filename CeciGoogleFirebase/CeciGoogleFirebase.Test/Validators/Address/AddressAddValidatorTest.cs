using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Service.Validators.Address;
using CeciGoogleFirebase.Test.Fakers.Address;
using CeciGoogleFirebase.Test.Fakers.User;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.Address
{
    public class AddressAddValidatorTest
    {
        private readonly AddressAddValidator _validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public AddressAddValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _validator = new AddressAddValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new AddressAddDTO();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(c => c.Id.Equals(It.IsAny<int>())))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.UserId);
            result.ShouldHaveValidationErrorFor(user => user.ZipCode);
            result.ShouldHaveValidationErrorFor(user => user.Street);
            result.ShouldHaveValidationErrorFor(user => user.District);
            result.ShouldHaveValidationErrorFor(user => user.Locality);
            result.ShouldHaveValidationErrorFor(user => user.Number);
            result.ShouldHaveValidationErrorFor(user => user.Uf);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = AddressFaker.AddressAddDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(c => c.Id.Equals(It.IsAny<int>())))
                .ReturnsAsync(UserFaker.UserEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            //result.ShouldNotHaveValidationErrorFor(user => user.UserId);
            result.ShouldNotHaveValidationErrorFor(user => user.ZipCode);
            result.ShouldNotHaveValidationErrorFor(user => user.Street);
            result.ShouldNotHaveValidationErrorFor(user => user.District);
            result.ShouldNotHaveValidationErrorFor(user => user.Locality);
            result.ShouldNotHaveValidationErrorFor(user => user.Number);
            result.ShouldNotHaveValidationErrorFor(user => user.Uf);
        }
    }
}
