using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Service.Validators.Address;
using CeciGoogleFirebase.Test.Fakers.Address;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.Address
{
    public class AddressDeleteValidatorTest
    {
        private readonly AddressDeleteValidator _validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public AddressDeleteValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _validator = new AddressDeleteValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new AddressDeleteDTO();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(c => c.Id.Equals(It.IsAny<int>())))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.AddressId);
        }

        /*[Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = new AddressDeleteDTO
            {
                AddressId = 1
            };

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(c => c.Id.Equals(model.AddressId)))
                .ReturnsAsync(AddressFaker.AddressEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.AddressId);
        }*/
    }
}
