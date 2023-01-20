using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using CeciGoogleFirebase.Test.Fakers.Address;
using CeciGoogleFirebase.Test.Fakers.Commons;
using CeciGoogleFirebase.Test.Fakers.Register;
using CeciGoogleFirebase.Test.Fakers.User;
using CeciGoogleFirebase.WebApplication.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciGoogleFirebase.Test.Controllers
{
    public class RegisterControllerTest
    {
        private readonly Mock<IRegisterService> _mockRegisterService;

        public RegisterControllerTest()
        {
            _mockRegisterService = new Mock<IRegisterService>();
        }

        [Fact]
        public async Task Redefine_password()
        {
            //Arrange
            var redefinePasswordDto = UserFaker.UserRedefinePasswordDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockRegisterService.Setup(x => x.RedefinePasswordAsync(redefinePasswordDto))
                .ReturnsAsync(resultResponse);

            var userController = RegisterControllerConstrutor();

            //Act
            var result = await userController.RedefinePassword(redefinePasswordDto);
            _mockRegisterService.Verify(x => x.RedefinePasswordAsync(redefinePasswordDto), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Add_logged_in_user_address()
        {
            //Arrange
            var addressLoggedUserAddDTO = AddressLoggedUserFaker.AddressLoggedUserAddDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockRegisterService.Setup(x => x.AddLoggedUserAddressAsync(addressLoggedUserAddDTO))
                .ReturnsAsync(resultResponse);

            var userController = RegisterControllerConstrutor();

            //Act
            var result = await userController.AddLoggedInUserAddressAsync(addressLoggedUserAddDTO);
            _mockRegisterService.Verify(x => x.AddLoggedUserAddressAsync(addressLoggedUserAddDTO), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Update_logged_in_user_address()
        {
            //Arrange
            var addressLoggedUserUpdateDTO = UserFaker.UserLoggedUpdateDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockRegisterService.Setup(x => x.UpdateLoggedUserAsync(addressLoggedUserUpdateDTO))
                .ReturnsAsync(resultResponse);

            var userController = RegisterControllerConstrutor();

            //Act
            var result = await userController.UpdateLoggedInUserAddress(addressLoggedUserUpdateDTO);
            _mockRegisterService.Verify(x => x.UpdateLoggedUserAsync(addressLoggedUserUpdateDTO), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Delete_logged_in_user_address()
        {
            //Arrange
            var addressLoggedUserDeleteDTO = AddressFaker.AddressDeleteDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockRegisterService.Setup(x => x.InactivateLoggedUserAddressAsync(addressLoggedUserDeleteDTO))
                .ReturnsAsync(resultResponse);

            var userController = RegisterControllerConstrutor();

            //Act
            var result = await userController.DeleteLoggedInUserAddress(addressLoggedUserDeleteDTO);
            _mockRegisterService.Verify(x => x.InactivateLoggedUserAddressAsync(addressLoggedUserDeleteDTO), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Self_registration_user()
        {
            //Arrange
            var userSelfRegistrationDto = UserFaker.UserSelfRegistrationDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockRegisterService.Setup(x => x.SelfRegistrationAsync(userSelfRegistrationDto))
                .ReturnsAsync(resultResponse);

            var userController = RegisterControllerConstrutor();

            //Act
            var result = await userController.SelfRegistration(userSelfRegistrationDto);
            _mockRegisterService.Verify(x => x.SelfRegistrationAsync(userSelfRegistrationDto), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Update_logged_in_user()
        {
            //Arrange
            var userLoggedUpdateDTO = UserFaker.UserLoggedUpdateDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockRegisterService.Setup(x => x.UpdateLoggedUserAsync(userLoggedUpdateDTO))
                .ReturnsAsync(resultResponse);

            var userController = RegisterControllerConstrutor();

            //Act
            var result = await userController.UpdateLoggedInUser(userLoggedUpdateDTO);
            _mockRegisterService.Verify(x => x.UpdateLoggedUserAsync(userLoggedUpdateDTO), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Get_logged_in_user()
        {
            //Arrange
            var userResultDTO = UserFaker.UserResultDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponseData(userResultDTO, It.IsAny<HttpStatusCode>());

            _mockRegisterService.Setup(x => x.GetLoggedInUserAsync())
                .ReturnsAsync(resultResponse);

            var userController = RegisterControllerConstrutor();

            //Act
            var result = await userController.GetLoggedInUser();
            _mockRegisterService.Verify(x => x.GetLoggedInUserAsync(), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse<UserResultDTO>>(objResult.Value);
        }

        [Fact]
        public async Task Get_logged_in_user_address()
        {
            //Arrange
            var addressResultDto = AddressFaker.AddressResultDTO().Generate(2);
            var addressFilterDto = AddressFaker.AddressFilterDTO().Generate();
            var resultDataResponse = ResultDataResponseFaker.ResultDataResponse<IEnumerable<AddressResultDTO>>(addressResultDto, It.IsAny<HttpStatusCode>());

            _mockRegisterService.Setup(x => x.GetLoggedUserAddressesAsync(addressFilterDto))
                .ReturnsAsync(resultDataResponse);

            var userController = RegisterControllerConstrutor();

            //Act
            var result = await userController.GetLoggedInUserAddresss(addressFilterDto);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultDataResponse<IEnumerable<AddressResultDTO>>>(objResult.Value);
        }

        private RegisterController RegisterControllerConstrutor()
        {
            return new RegisterController(_mockRegisterService.Object);
        }
    }
}
