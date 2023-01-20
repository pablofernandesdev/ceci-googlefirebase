using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using CeciGoogleFirebase.Test.Fakers.Address;
using CeciGoogleFirebase.Test.Fakers.Commons;
using CeciGoogleFirebase.WebApplication.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciGoogleFirebase.Test.Controllers
{
    public class AddressControllerTest
    {
        private readonly Mock<IAddressService> _mockAddressService;

        public AddressControllerTest()
        {
            _mockAddressService = new Mock<IAddressService>();
        }

        [Fact]
        public async Task Get_by_zip_code()
        {
            //Arrange
            var addressResultDto = AddressFaker.AddressResultDTO().Generate();
            var addressZipCodeDto = AddressFaker.AddressZipCodeDTO().Generate();
            var resultDataResponse = ResultResponseFaker.ResultResponseData<AddressResultDTO>(addressResultDto, It.IsAny<HttpStatusCode>());

            _mockAddressService.Setup(x => x.GetAddressByZipCodeAsync(addressZipCodeDto))
                .ReturnsAsync(resultDataResponse);

            var userController = AddressControllerConstrutor();

            //Act
            var result = await userController.GetByZipCode(addressZipCodeDto);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse<AddressResultDTO>>(objResult.Value);
        }

        [Fact]
        public async Task Add_address()
        {
            //Arrange
            var addressAddDto = AddressFaker.AddressAddDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockAddressService.Setup(x => x.AddAsync(addressAddDto))
                .ReturnsAsync(resultResponse);

            var addressController = AddressControllerConstrutor();

            //Act
            var result = await addressController.Add(addressAddDto);
            _mockAddressService.Verify(x => x.AddAsync(addressAddDto), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Update_address()
        {
            //Arrange
            var addressUpdateDto = AddressFaker.AddressUpdateDTO().Generate();
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockAddressService.Setup(x => x.UpdateAsync(addressUpdateDto))
                .ReturnsAsync(resultResponse);

            var addressController = AddressControllerConstrutor();

            //Act
            var result = await addressController.Update(addressUpdateDto);
            _mockAddressService.Verify(x => x.UpdateAsync(addressUpdateDto), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Delete_address()
        {
            //Arrange
            var addressDeleteDto = AddressFaker.AddressDeleteDTO().Generate();
            var addressId = addressDeleteDto.AddressId;
            var resultResponse = ResultResponseFaker.ResultResponse(It.IsAny<HttpStatusCode>());

            _mockAddressService.Setup(x => x.DeleteAsync(addressId))
                .ReturnsAsync(resultResponse);

            var addressController = AddressControllerConstrutor();

            //Act
            var result = await addressController.Delete(addressDeleteDto);
            _mockAddressService.Verify(x => x.DeleteAsync(addressId), Times.Once);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse>(objResult.Value);
        }

        [Fact]
        public async Task Get_all()
        {
            //Arrange
            var addressResultDto = AddressFaker.AddressResultDTO().Generate(2);
            var addressFilterDto = AddressFaker.AddressFilterDTO().Generate();
            var resultDataResponse = ResultDataResponseFaker.ResultDataResponse<IEnumerable<AddressResultDTO>>(addressResultDto, It.IsAny<HttpStatusCode>());

            _mockAddressService.Setup(x => x.GetAsync(addressFilterDto))
                .ReturnsAsync(resultDataResponse);

            var userController = AddressControllerConstrutor();

            //Act
            var result = await userController.Get(addressFilterDto);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultDataResponse<IEnumerable<AddressResultDTO>>>(objResult.Value);
        }

        [Fact]
        public async Task Get_by_id()
        {
            //Arrange
            var addressResultDto = AddressFaker.AddressResultDTO().Generate();
            var addressIdentifierDto = AddressFaker.AddressIdentifierDTO().Generate();
            var addressId = addressIdentifierDto.AddressId;
            var resultDataResponse = ResultResponseFaker.ResultResponseData<AddressResultDTO>(addressResultDto, It.IsAny<HttpStatusCode>());

            _mockAddressService.Setup(x => x.GetByIdAsync(addressId))
                .ReturnsAsync(resultDataResponse);

            var userController = AddressControllerConstrutor();

            //Act
            var result = await userController.GetById(addressIdentifierDto);

            //Assert
            var objResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.IsType<ResultResponse<AddressResultDTO>>(objResult.Value);
        }

        private AddressController AddressControllerConstrutor()
        {
            return new AddressController(_mockAddressService.Object);
        }
    }
}
