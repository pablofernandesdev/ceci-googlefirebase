using AutoMapper;
using CeciGoogleFirebase.Domain.DTO.ViaCep;
using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service.External;
using CeciGoogleFirebase.Domain.Mapping;
using CeciGoogleFirebase.Service.Services;
using CeciGoogleFirebase.Test.Fakers.Address;
using CeciGoogleFirebase.Test.Fakers.Commons;
using CeciGoogleFirebase.Test.Fakers.ViaCep;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciGoogleFirebase.Test.Services
{
    public class AddressServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IViaCepService> _mockViaCepService;
        private readonly IMapper _mapper;

        public AddressServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockViaCepService = new Mock<IViaCepService>();

            //Auto mapper configuration
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Add_address_successfully()
        {
            //Arrange
            var addressEntityFaker = AddressFaker.AddressEntity().Generate();

            _mockUnitOfWork.Setup(x => x.Address.AddAsync(addressEntityFaker))
                .ReturnsAsync(addressEntityFaker);

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.AddAsync(AddressFaker.AddressAddDTO().Generate());

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Failed_to_add_address()
        {
            //Arrange
            var addressEntityFaker = AddressFaker.AddressEntity().Generate();

            _mockUnitOfWork.Setup(x => x.Address.AddAsync(addressEntityFaker))
                .ReturnsAsync(addressEntityFaker);

            _mockUnitOfWork.Setup(x => x.CommitAsync())
                .ThrowsAsync(new Exception());

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.AddAsync(AddressFaker.AddressAddDTO().Generate());

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Update_address_successfully()
        {
            //Arrange
            var addressUpdateDTOFaker = AddressFaker.AddressUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(c => c.Id == addressUpdateDTOFaker.AddressId))
                .ReturnsAsync(_mapper.Map<Address>(addressUpdateDTOFaker));

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.UpdateAsync(addressUpdateDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Failed_update_address()
        {
            //Arrange
            var addressUpdateDTOFaker = AddressFaker.AddressUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(c => c.Id == addressUpdateDTOFaker.AddressId))
                .ThrowsAsync(new Exception());

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.UpdateAsync(addressUpdateDTOFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Delete_address_successfully()
        {
            //Arrange
            var addresId = AddressFaker.AddressIdentifierDTO().Generate().AddressId;

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(c => c.Id == addresId))
                .ReturnsAsync(AddressFaker.AddressEntity().Generate());

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.DeleteAsync(addresId);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Failed_delete_address()
        {
            //Arrange
            var addresId = AddressFaker.AddressIdentifierDTO().Generate().AddressId;

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(c => c.Id == addresId))
                .ThrowsAsync(new Exception());

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.DeleteAsync(addresId);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_address_by_filter()
        {
            //Arrange
            var addressEntityFaker = AddressFaker.AddressEntity().Generate(3);
            var addressFilterDto = AddressFaker.AddressFilterDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetByFilterAsync(addressFilterDto))
                .ReturnsAsync(addressEntityFaker);

            _mockUnitOfWork.Setup(x => x.Address.GetTotalByFilterAsync(addressFilterDto))
                .ReturnsAsync(addressEntityFaker.Count);

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.GetAsync(addressFilterDto);

            //Assert
            Assert.True(result.Data.Any() && result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Failed_get_address_by_filter()
        {
            //Arrange
            var addressEntityFaker = AddressFaker.AddressEntity().Generate(3);
            var addressFilterDto = AddressFaker.AddressFilterDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetByFilterAsync(addressFilterDto))
                .ThrowsAsync(new Exception());

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.GetAsync(addressFilterDto);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_by_id()
        {
            //Arrange
            var addressEntityFaker = AddressFaker.AddressEntity().Generate();
            var adressId = addressEntityFaker.Id;

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultNoTrackingAsync(x => x.Id == adressId))
                .ReturnsAsync(addressEntityFaker);

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.GetByIdAsync(adressId);

            //Assert
            Assert.True(result.Data != null && result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Failed_get_by_id()
        {
            //Arrange
            var addressEntityFaker = AddressFaker.AddressEntity().Generate();
            var adressId = addressEntityFaker.Id;

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultNoTrackingAsync(x => x.Id == adressId))
                .ThrowsAsync(new Exception());

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.GetByIdAsync(adressId);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_address_by_zip_code()
        {
            //Arrange
            var viaCepAddressResponseFaker = ViaCepFaker.ViaCepAddressResponseDTO().Generate();
            var zipCodeFaker = AddressFaker.AddressZipCodeDTO().Generate();

            _mockViaCepService.Setup(x => x.GetAddressByZipCodeAsync(zipCodeFaker.ZipCode))
                .ReturnsAsync(ResultResponseFaker.ResultResponseData<ViaCepAddressResponseDTO>(viaCepAddressResponseFaker, HttpStatusCode.OK));

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.GetAddressByZipCodeAsync(zipCodeFaker);

            //Assert
            Assert.True(result.Data != null && result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Error_getting_address_by_zip_code()
        {
            //Arrange
            var viaCepAddressResponseFaker = ViaCepFaker.ViaCepAddressResponseDTO().Generate();
            var zipCodeFaker = AddressFaker.AddressZipCodeDTO().Generate();

            _mockViaCepService.Setup(x => x.GetAddressByZipCodeAsync(zipCodeFaker.ZipCode))
                .ReturnsAsync(ResultResponseFaker.ResultResponseData<ViaCepAddressResponseDTO>(viaCepAddressResponseFaker, HttpStatusCode.InternalServerError));

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.GetAddressByZipCodeAsync(zipCodeFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Failed_get_address_by_zip_code()
        {
            //Arrange
            var zipCodeFaker = AddressFaker.AddressZipCodeDTO().Generate();

            _mockViaCepService.Setup(x => x.GetAddressByZipCodeAsync(zipCodeFaker.ZipCode))
                .ThrowsAsync(new Exception());

            var addressService = AddressServiceConstrutor();

            //Act
            var result = await addressService.GetAddressByZipCodeAsync(zipCodeFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        private AddressService AddressServiceConstrutor()
        {
            return new AddressService(
                _mockUnitOfWork.Object,
                _mockViaCepService.Object,
                _mapper);
        }
    }
}
