using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Service.Services;
using CeciGoogleFirebase.Test.Fakers.User;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciGoogleFirebase.Test.Services
{
    public class ReportServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public ReportServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
        }

        [Fact]
        public async Task Generate_users_report_successfully()
        {
            //Arrange
            var userFilterDto = UserFaker.UserFilterDTO().Generate();
            _mockUnitOfWork.Setup(x => x.User.GetByFilterAsync(userFilterDto))
                .ReturnsAsync(UserFaker.UserEntity().Generate(3));

            var reportService = ReportServiceConstrutor();

            //Act
            var result = await reportService.GenerateUsersReport(userFilterDto);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Generate_users_report_empty_list()
        {
            //Arrange
            var userFilterDto = UserFaker.UserFilterDTO().Generate();
            _mockUnitOfWork.Setup(x => x.User.GetByFilterAsync(userFilterDto))
                .ReturnsAsync(UserFaker.UserEntity().Generate(0));

            var reportService = ReportServiceConstrutor();

            //Act
            var result = await reportService.GenerateUsersReport(userFilterDto);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Generate_users_report_exception()
        {
            //Arrange
            var userFilterDto = UserFaker.UserFilterDTO().Generate();
            _mockUnitOfWork.Setup(x => x.User.GetByFilterAsync(userFilterDto))
                .ThrowsAsync(new Exception());

            var reportService = ReportServiceConstrutor();

            //Act
            var result = await reportService.GenerateUsersReport(userFilterDto);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        private ReportService ReportServiceConstrutor()
        {
            return new ReportService(_mockUnitOfWork.Object);
        }
    }
}
