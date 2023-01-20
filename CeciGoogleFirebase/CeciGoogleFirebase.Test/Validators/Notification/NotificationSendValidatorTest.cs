using CeciGoogleFirebase.Domain.DTO.Notification;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Service.Validators.Notification;
using CeciGoogleFirebase.Test.Fakers.Notification;
using CeciGoogleFirebase.Test.Fakers.User;
using FluentValidation.TestHelper;
using Moq;
using Xunit;

namespace CeciGoogleFirebase.Test.Validators.Notification
{
    public class NotificationSendValidatorTest
    {
        private readonly NotificationSendValidator _validator;
        private readonly Moq.Mock<IUnitOfWork> _mockUnitOfWork;

        public NotificationSendValidatorTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _validator = new NotificationSendValidator(_mockUnitOfWork.Object);
        }

        [Fact]
        public void There_should_be_an_error_when_properties_are_null()
        {
            //Arrange
            var model = new NotificationSendDTO();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(x => x.Id.Equals(It.IsAny<int>())))
                .ReturnsAsync(value: null);

            //act
            var result = _validator.TestValidate(model);

            //assert
            //result.ShouldHaveValidationErrorFor(user => user.IdUser);
            result.ShouldHaveValidationErrorFor(user => user.Title);
            result.ShouldHaveValidationErrorFor(user => user.Body);
        }

        [Fact]
        public void There_should_not_be_an_error_for_the_properties()
        {
            //Arrange
            var model = NotificationFaker.NotificationSendDTO().Generate();
            
            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(x => x.Id.Equals(model.IdUser)))
                .ReturnsAsync(UserFaker.UserEntity().Generate());

            //act
            var result = _validator.TestValidate(model);

            //assert
            //result.ShouldNotHaveValidationErrorFor(user => user.IdUser);
            result.ShouldNotHaveValidationErrorFor(user => user.Title);
            result.ShouldNotHaveValidationErrorFor(user => user.Body);
        }
    }
}
