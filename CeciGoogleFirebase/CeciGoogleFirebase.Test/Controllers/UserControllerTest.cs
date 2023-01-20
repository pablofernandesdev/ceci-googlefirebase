using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using CeciGoogleFirebase.Test.Fakers.Commons;
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
    public class UserControllerTest
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IImportService> _mockImportService;

        public UserControllerTest()
        {
            _mockUserService = new Moq.Mock<IUserService>();
            _mockImportService = new Mock<IImportService>();    
        }

        [Fact]
        public async Task Get_all()
        {
            //Arrange
            var userResultDto = UserFaker.UserResultDTO().Generate(2);
            var userFilterDto = UserFaker.UserFilterDTO().Generate();
            var resultDataResponse = ResultDataResponseFaker.ResultDataResponse<IEnumerable<UserResultDTO>>(userResultDto, It.IsAny<HttpStatusCode>());

            _mockUserService.Setup(x => x.GetAsync(userFilterDto))
                .ReturnsAsync(resultDataResponse);

            var userController = UserControllerConstrutor();

            //Act
            var result = await userController.Get(userFilterDto);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultDataResponse<IEnumerable<UserResultDTO>>>(objResult.Value);
        }

        [Fact]
        public async Task Add_user()
        {
            //Arrange
            var userAddDto = UserFaker.UserAddDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockUserService.Setup(x => x.AddAsync(userAddDto))
                .ReturnsAsync(resultResponse);

            var userController = UserControllerConstrutor();

            //Act
            var result = await userController.Add(userAddDto);
            _mockUserService.Verify(x => x.AddAsync(userAddDto), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Update_user()
        {
            //Arrange
            var userUpdateDto = UserFaker.UserUpdateDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockUserService.Setup(x => x.UpdateAsync(userUpdateDto))
                .ReturnsAsync(resultResponse);

            var userController = UserControllerConstrutor();

            //Act
            var result = await userController.Update(userUpdateDto);
            _mockUserService.Verify(x => x.UpdateAsync(userUpdateDto), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Update_role_user()
        {
            //Arrange
            var userUpdateRole = UserFaker.UserUpdateRoleDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockUserService.Setup(x => x.UpdateRoleAsync(userUpdateRole))
                .ReturnsAsync(resultResponse);

            var userController = UserControllerConstrutor();

            //Act
            var result = await userController.UpdateRole(userUpdateRole);
            _mockUserService.Verify(x => x.UpdateRoleAsync(userUpdateRole), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Delete_user()
        {
            //Arrange
            var userDeleteDto = UserFaker.UserDeleteDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockUserService.Setup(x => x.DeleteAsync(userDeleteDto))
                .ReturnsAsync(resultResponse);

            var userController = UserControllerConstrutor();

            //Act
            var result = await userController.Delete(userDeleteDto);
            _mockUserService.Verify(x => x.DeleteAsync(userDeleteDto), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Get_user_by_id()
        {
            //Arrange
            var userResultDto = UserFaker.UserResultDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponseData<UserResultDTO>(userResultDto, It.IsAny<HttpStatusCode>());

            _mockUserService.Setup(x => x.GetByIdAsync(1))
                .ReturnsAsync(resultResponse);

            var userController = UserControllerConstrutor();

            //Act
            var result = await userController.GetById(new UserIdentifierDTO { 
                UserId = 1});

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse<UserResultDTO>>(objResult.Value);
        }

        [Fact]
        public async Task Import_users()
        {
            //Arrange
            var fileUploadDTO = new Domain.DTO.Import.FileUploadDTO();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockImportService.Setup(x => x.ImportUsersAsync(fileUploadDTO))
                .ReturnsAsync(resultResponse);

            var userController = UserControllerConstrutor();

            //Act
            var result = await userController.Import(fileUploadDTO);
            _mockImportService.Verify(x => x.ImportUsersAsync(fileUploadDTO), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        private UserController UserControllerConstrutor()
        {
            return new UserController(_mockUserService.Object,
                _mockImportService.Object);
        }
    }
}
