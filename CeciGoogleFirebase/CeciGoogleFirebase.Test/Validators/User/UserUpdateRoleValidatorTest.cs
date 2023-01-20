using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Service.Validators.User;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.User
{
    public class UserUpdateRoleValidatorTest
    {
        private UserUpdateRoleValidator validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public UserUpdateRoleValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            validator = new UserUpdateRoleValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new UserUpdateRoleDTO();

            _mockUnitOfWork.Setup(x => x.Role.GetFirstOrDefaultAsync(c => c.Id == 1))
                .ReturnsAsync(value: null);

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(c => c.Id == 1))
               .ReturnsAsync(value: null);

            //act
            var result = validator.TestValidate(model);

            //assert
            result.ShouldHaveValidationErrorFor(user => user.RoleId);
            result.ShouldHaveValidationErrorFor(user => user.UserId);
        }

        /*[Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = UserFaker.UserUpdateRoleDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Role.GetByConditionAsync(c => c.Id.Equals(model.RoleId)))
                .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            _mockUnitOfWork.Setup(x => x.User.GetByConditionAsync(c => c.Id.Equals(model.UserId)))
              .ReturnsAsync(UserFaker.UserEntity().Generate());

            //act
            var result = validator.TestValidate(model);

            //assert
            result.ShouldNotHaveValidationErrorFor(user => user.RoleId);
            result.ShouldNotHaveValidationErrorFor(user => user.UserId);
        }*/
    }
}
