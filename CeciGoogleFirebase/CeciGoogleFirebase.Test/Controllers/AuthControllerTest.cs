using CeciGoogleFirebase.Domain.DTO.Auth;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using CeciGoogleFirebase.Test.Fakers.Auth;
using CeciGoogleFirebase.Test.Fakers.Commons;
using CeciGoogleFirebase.Test.Fakers.ValidationCode;
using CeciGoogleFirebase.WebApplication.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciGoogleFirebase.Test.Controllers
{
    public class AuthControllerTest
    {
        private readonly Mock<IAuthService> _mockAuthService;
        private readonly Mock<IValidationCodeService> _mockValidationCodeService;

        public AuthControllerTest()
        {
            _mockAuthService = new Moq.Mock<IAuthService>();
            _mockValidationCodeService = new Mock<IValidationCodeService>();
        }

        [Fact]
        public async Task Auth_user()
        {
            //Arrange
            var loginDTO = AuthFaker.LoginDTO().Generate();
            var resultAuthDTO = AuthFaker.AuthResultDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponseData(resultAuthDTO, It.IsAny<HttpStatusCode>());

            _mockAuthService.Setup(x => x.AuthenticateAsync(loginDTO, It.IsAny<string>()))
                .ReturnsAsync(resultResponse);

            var roleController = AuthControllerConstrutor();

            //Act
            var result = await roleController.Auth(loginDTO);
            _mockAuthService.Verify(x => x.AuthenticateAsync(loginDTO, It.IsAny<string>()), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse<AuthResultDTO>>(objResult.Value);
        }

        [Fact]
        public async Task Auth_user_return_remote_ip_address_map_to_iPv4()
        {
            //Arrange
            var loginDTO = AuthFaker.LoginDTO().Generate();
            var resultAuthDTO = AuthFaker.AuthResultDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponseData(resultAuthDTO, It.IsAny<HttpStatusCode>());

            _mockAuthService.Setup(x => x.AuthenticateAsync(loginDTO, It.IsAny<string>()))
                .ReturnsAsync(resultResponse);

            var roleController = AuthControllerConstrutor();

            var httpContext = new DefaultHttpContext()
            {
                Connection =
                {
                    RemoteIpAddress = new System.Net.IPAddress(16885952)
                }
            };
            httpContext.Request.Headers["Teste"] = "fake_X-Forwarded-For"; //Set header
            //Controller needs a controller context 
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            roleController.ControllerContext = controllerContext;

            //Act
            var result = await roleController.Auth(loginDTO);
            _mockAuthService.Verify(x => x.AuthenticateAsync(loginDTO, It.IsAny<string>()), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse<AuthResultDTO>>(objResult.Value);
        }

        [Fact]
        public async Task Refresh_token_user()
        {
            //Arrange
            var loginDTO = AuthFaker.LoginDTO().Generate();
            var resultAuthDTO = AuthFaker.AuthResultDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponseData(resultAuthDTO, It.IsAny<HttpStatusCode>());

            _mockAuthService.Setup(x => x.RefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(resultResponse);

            var roleController = AuthControllerConstrutor();

            //Act
            var result = await roleController.RefreshToken();
            _mockAuthService.Verify(x => x.RefreshTokenAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse<AuthResultDTO>>(objResult.Value);
        }

        [Fact]
        public async Task Revoke_token_user()
        {
            //Arrange
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockAuthService.Setup(x => x.RevokeTokenAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(resultResponse);

            var roleController = AuthControllerConstrutor();

            //Act
            var result = await roleController.RevokeToken();
            _mockAuthService.Verify(x => x.RevokeTokenAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Forgot_password_user()
        {
            //Arrange
            var forgotPasswordDtoFaker = AuthFaker.ForgotPasswordDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockAuthService.Setup(x => x.ForgotPasswordAsync(forgotPasswordDtoFaker))
                .ReturnsAsync(resultResponse);

            var roleController = AuthControllerConstrutor();

            //Act
            var result = await roleController.ForgotPassword(forgotPasswordDtoFaker);
            _mockAuthService.Verify(x => x.ForgotPasswordAsync(forgotPasswordDtoFaker), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Send_validation_code()
        {
            //Arrange
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockValidationCodeService.Setup(x => x.SendAsync())
                .ReturnsAsync(resultResponse);

            var roleController = AuthControllerConstrutor();

            //Act
            var result = await roleController.SendValidationCode();
            _mockValidationCodeService.Verify(x => x.SendAsync(), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Validate_validation_code()
        {
            //Arrange
            var validationCodeValidateDto = ValidationCodeFaker.ValidationCodeValidateDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockValidationCodeService.Setup(x => x.ValidateCodeAsync(validationCodeValidateDto))
                .ReturnsAsync(resultResponse);

            var authController = AuthControllerConstrutor();

            //Act
            var result = await authController.ValidateValidationCode(validationCodeValidateDto);
            _mockValidationCodeService.Verify(x => x.ValidateCodeAsync(validationCodeValidateDto), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        public AuthController AuthControllerConstrutor()
        {
            var roleController = new AuthController(_mockAuthService.Object,
                _mockValidationCodeService.Object) { };

            //https://stackoverflow.com/a/50117120
            var httpContext = new DefaultHttpContext(); // or mock a `HttpContext`
            httpContext.Request.Headers["X-Forwarded-For"] = "fake_X-Forwarded-For"; //Set header
                                                                                     //Controller needs a controller context 
            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            roleController.ControllerContext = controllerContext;

            return roleController;
        }
    }
}
