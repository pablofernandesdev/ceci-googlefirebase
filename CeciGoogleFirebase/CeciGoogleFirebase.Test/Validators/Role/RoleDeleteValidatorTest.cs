using CeciGoogleFirebase.Domain.DTO.Role;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Service.Validators.Role;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.Role
{
    public class RoleDeleteValidatorTest
    {
        private readonly RoleDeleteValidator _validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public RoleDeleteValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _validator = new RoleDeleteValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new RoleDeleteDTO();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id.Equals(It.IsAny<int>())))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(role => role.RoleId);
        }

        /*[Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = new RoleDeleteDTO { 
                RoleId = 1};

            _mockUnitOfWork.Setup(x => x.Role.GetByConditionAsync(c => c.Id.Equals(It.IsAny<int>())))
                .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(role => role.RoleId);
        }*/
    }
}
