using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service.External;
using CeciGoogleFirebase.Service.Services;
using CeciGoogleFirebase.Test.Fakers.Commons;
using CeciGoogleFirebase.Test.Fakers.Notification;
using CeciGoogleFirebase.Test.Fakers.RegistrationToken;
using CeciGoogleFirebase.Test.Fakers.User;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciGoogleFirebase.Test.Services
{
    public class NotificationServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IFirebaseService> _mockFirebaseService;

        public NotificationServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockFirebaseService = new Mock<IFirebaseService>();
        }

        [Fact]
        public async Task Send_notification_successfully()
        {
            //Arrange
            var notificationSendDTOFaker = NotificationFaker.NotificationSendDTO().Generate();
            var registrationTokenEntityFaker = RegistrationTokenFaker.RegistrationTokenEntity().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(c => c.Id == notificationSendDTOFaker.IdUser))
                .ReturnsAsync(UserFaker.UserEntity().Generate());

            _mockUnitOfWork.Setup(x => x.RegistrationToken.GetFirstOrDefaultAsync(c => c.UserId == notificationSendDTOFaker.IdUser))
                .ReturnsAsync(registrationTokenEntityFaker);

            _mockFirebaseService.Setup(x => x.SendNotificationAsync(registrationTokenEntityFaker.Token,
                notificationSendDTOFaker.Title,
                notificationSendDTOFaker.Body))
                .ReturnsAsync(ResultResponseFaker.ResultResponse(HttpStatusCode.OK));

            var importService = NotificationServiceConstrutor();

            //Act
            var result = await importService.SendAsync(notificationSendDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Send_notification_exception()
        {
            //Arrange
            var notificationSendDTOFaker = NotificationFaker.NotificationSendDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(c => c.Id == notificationSendDTOFaker.IdUser))
                .ThrowsAsync(new System.Exception());

            var importService = NotificationServiceConstrutor();

            //Act
            var result = await importService.SendAsync(notificationSendDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        private NotificationService NotificationServiceConstrutor()
        {
            return new NotificationService(
                _mockUnitOfWork.Object,
                _mockFirebaseService.Object);
        }
    }
}
