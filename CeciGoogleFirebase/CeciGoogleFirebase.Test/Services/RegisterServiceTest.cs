using AutoMapper;
using CeciGoogleFirebase.Domain.DTO.Register;
using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using CeciGoogleFirebase.Domain.Mapping;
using CeciGoogleFirebase.Infra.CrossCutting.Extensions;
using CeciGoogleFirebase.Service.Services;
using CeciGoogleFirebase.Test.Fakers.Address;
using CeciGoogleFirebase.Test.Fakers.Commons;
using CeciGoogleFirebase.Test.Fakers.Email;
using CeciGoogleFirebase.Test.Fakers.Register;
using CeciGoogleFirebase.Test.Fakers.Role;
using CeciGoogleFirebase.Test.Fakers.User;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace CeciGoogleFirebase.Test.Services
{
    public class RegisterServiceTest
    {
        private readonly string _claimNameIdentifier = "1";
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IBackgroundJobClient> _mockBackgroundJobClient;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly IMapper _mapper;

        public RegisterServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockEmailService = new Mock<IEmailService>();
            _mockBackgroundJobClient = new Mock<IBackgroundJobClient>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            //Auto mapper configuration
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();

            //http context configuration
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "example name"),
                new Claim(ClaimTypes.NameIdentifier, _claimNameIdentifier),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            _mockHttpContextAccessor.Setup(h => h.HttpContext.User).Returns(user);
        }

        [Fact]
        public async Task User_self_registration_successfully()
        {
            //Arrange
            var userEntityFaker = UserFaker.UserEntity().Generate();
            var emailRequestDTOFaker = EmailFaker.EmailRequestDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Role.GetBasicProfile())
               .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            _mockUnitOfWork.Setup(x => x.User.AddAsync(userEntityFaker))
                .ReturnsAsync(userEntityFaker);

            _mockEmailService.Setup(x => x.SendEmailAsync(emailRequestDTOFaker))
                .ReturnsAsync(ResultResponseFaker.ResultResponse(HttpStatusCode.OK).Generate());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.SelfRegistrationAsync(UserFaker.UserSelfRegistrationDTO().Generate());

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task User_self_registration_exception()
        {
            //Arrange
            _mockUnitOfWork.Setup(x => x.Role.GetBasicProfile())
               .ThrowsAsync(new Exception());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.SelfRegistrationAsync(UserFaker.UserSelfRegistrationDTO().Generate());

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        [Fact]
        public async Task Update_logged_user_successfully()
        {
            //Arrange
            var userUpdateDTOFaker = UserFaker.UserLoggedUpdateDTO().Generate();
            var userId = _claimNameIdentifier;

            _mockUnitOfWork.Setup(x => x.User
                .GetFirstOrDefaultNoTrackingAsync(c => c.Email == userUpdateDTOFaker.Email && c.Id != Convert.ToInt32(userId)))
                .ReturnsAsync(value: null);

            _mockUnitOfWork.Setup(x => x.User
                .GetFirstOrDefaultAsync(c => c.Id == Convert.ToInt32(userId)))
                .ReturnsAsync(UserFaker.UserEntity().Generate());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.UpdateLoggedUserAsync(userUpdateDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Update_logged_user_email_already_registered()
        {
            //Arrange
            var userUpdateDTOFaker = UserFaker.UserLoggedUpdateDTO().Generate();
            var userId = _claimNameIdentifier;

            _mockUnitOfWork.Setup(x => x.User
                    .GetFirstOrDefaultNoTrackingAsync(c => c.Email == userUpdateDTOFaker.Email && c.Id != Convert.ToInt32(userId)))
                    .ReturnsAsync(UserFaker.UserEntity().Generate());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.UpdateLoggedUserAsync(userUpdateDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.BadRequest));
        }

        [Fact]
        public async Task Update_logged_user_exception()
        {
            //Arrange
            var userUpdateDTOFaker = UserFaker.UserLoggedUpdateDTO().Generate();
            var userId = _claimNameIdentifier;

            _mockUnitOfWork.Setup(x => x.User
                    .GetFirstOrDefaultNoTrackingAsync(c => c.Email == userUpdateDTOFaker.Email && c.Id != Convert.ToInt32(userId)))
                    .ThrowsAsync(new Exception());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.UpdateLoggedUserAsync(userUpdateDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        [Fact]
        public async Task Get_logged_in_user_successfully()
        {
            //Arrange
            var userId = _claimNameIdentifier;

            _mockUnitOfWork.Setup(x => x.User.GetUserByIdAsync(Convert.ToInt32(userId)))
                .ReturnsAsync(UserFaker.UserEntity().Generate());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.GetLoggedInUserAsync();

            //Assert
            Assert.True(result.Data != null && result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_logged_in_user_exception()
        {
            //Arrange
            var userId = _claimNameIdentifier;

            _mockUnitOfWork.Setup(x => x.User.GetUserByIdAsync(Convert.ToInt32(userId)))
                .ThrowsAsync(new Exception());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.GetLoggedInUserAsync();

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        [Fact]
        public async Task Redefine_user_password_successfully()
        {
            //Arrange
            var userEntityFaker = UserFaker.UserEntity().Generate();
            var userId = _claimNameIdentifier;

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(c => c.Id.Equals(Convert.ToInt32(userId))))
                .ReturnsAsync(userEntityFaker);

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.RedefinePasswordAsync(new UserRedefinePasswordDTO
            {
                CurrentPassword = PasswordExtension.DecryptPassword(userEntityFaker.Password),
                NewPassword = "dGVzdGUy"
            });

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Redefine_user_password_invalid_password()
        {
            //Arrange
            var userEntityFaker = UserFaker.UserEntity().Generate();
            var userId = _claimNameIdentifier;

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(c => c.Id.Equals(Convert.ToInt32(userId))))
                .ReturnsAsync(userEntityFaker);

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.RedefinePasswordAsync(new UserRedefinePasswordDTO
            {
                CurrentPassword = "xxxxxx",
                NewPassword = "dGVzdGUy"
            });

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.Unauthorized));
        }

        [Fact]
        public async Task Redefine_user_password_exception()
        {
            //Arrange
            var userId = _claimNameIdentifier;

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(c => c.Id.Equals(Convert.ToInt32(userId))))
                .ThrowsAsync(new Exception());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.RedefinePasswordAsync(UserFaker.UserRedefinePasswordDTO().Generate());

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        [Fact]
        public async Task Add_logged_in_user_address_successfully()
        {
            //Arrange
            var addressEntityFaker = AddressFaker.AddressEntity().Generate();

            _mockUnitOfWork.Setup(x => x.Address.AddAsync(addressEntityFaker))
                .ReturnsAsync(addressEntityFaker);

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.AddLoggedUserAddressAsync(AddressLoggedUserFaker.AddressLoggedUserAddDTO().Generate());

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Add_logged_in_user_address_exception()
        {
            //Arrange
            var addressEntityFaker = AddressFaker.AddressEntity().Generate();

            _mockUnitOfWork.Setup(x => x.Address.AddAsync(addressEntityFaker))
                .ReturnsAsync(addressEntityFaker);

            _mockUnitOfWork.Setup(x => x.CommitAsync())
                .ThrowsAsync(new Exception());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.AddLoggedUserAddressAsync(AddressLoggedUserFaker.AddressLoggedUserAddDTO().Generate());

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Update_logged_in_user_address_successfully()
        {
            //Arrange
            var userId = _claimNameIdentifier;

            var addressUpdateDTOFaker = AddressLoggedUserFaker.AddressLoggedUserUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(x => x.UserId == Convert.ToInt32(userId)
                    && x.Id == addressUpdateDTOFaker.AddressId))
                .ReturnsAsync(_mapper.Map<Address>(addressUpdateDTOFaker));

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.UpdateLoggedUserAddressAsync(addressUpdateDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Update_logged_in_user_address_exception()
        {
            //Arrange
            var userId = _claimNameIdentifier;

            var addressUpdateDTOFaker = AddressLoggedUserFaker.AddressLoggedUserUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(x => x.UserId == Convert.ToInt32(userId)
                    && x.Id == addressUpdateDTOFaker.AddressId))
                    .ThrowsAsync(new Exception());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.UpdateLoggedUserAddressAsync(addressUpdateDTOFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Update_logged_in_user_address_failed()
        {
            //Arrange
            var userId = _claimNameIdentifier;

            var addressUpdateDTOFaker = AddressLoggedUserFaker.AddressLoggedUserUpdateDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(x => x.UserId == Convert.ToInt32(userId)
                    && x.Id == addressUpdateDTOFaker.AddressId))
                        .ReturnsAsync(value: null);

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.UpdateLoggedUserAddressAsync(addressUpdateDTOFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Inactivate_logged_in_user_address_successfully()
        {
            //Arrange
            var userId = _claimNameIdentifier;

            var addressUpdateDTOFaker = AddressFaker.AddressEntity().Generate();
            var addressIdentifierDTOFaker = AddressFaker.AddressDeleteDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(x => x.UserId == Convert.ToInt32(userId)
                    && x.Id == addressIdentifierDTOFaker.AddressId))
                .ReturnsAsync(addressUpdateDTOFaker);

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.InactivateLoggedUserAddressAsync(addressIdentifierDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Inactivate_logged_in_user_address_exception()
        {
            //Arrange
            var userId = _claimNameIdentifier;

            var addressUpdateDTOFaker = AddressFaker.AddressEntity().Generate();
            var addressIdentifierDTOFaker = AddressFaker.AddressDeleteDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(x => x.UserId == Convert.ToInt32(userId)
                    && x.Id == addressIdentifierDTOFaker.AddressId))
                .ThrowsAsync(new Exception());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.InactivateLoggedUserAddressAsync(addressIdentifierDTOFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Inactivate_logged_in_user_address_failed()
        {
            //Arrange
            var userId = _claimNameIdentifier;

            var addressUpdateDTOFaker = AddressFaker.AddressEntity().Generate();
            var addressIdentifierDTOFaker = AddressFaker.AddressDeleteDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultAsync(x => x.UserId == Convert.ToInt32(userId)
                    && x.Id == addressIdentifierDTOFaker.AddressId))
                .ReturnsAsync(value: null);

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.InactivateLoggedUserAddressAsync(addressIdentifierDTOFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_logged_in_user_address_by_filter()
        {
            //Arrange
            var userId = _claimNameIdentifier;
            var addressEntityFaker = AddressFaker.AddressEntity().Generate(3);
            var addressFilterDto = AddressFaker.AddressFilterDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetLoggedUserAddressesAsync(Convert.ToInt32(userId), addressFilterDto))
                .ReturnsAsync(addressEntityFaker);

            _mockUnitOfWork.Setup(x => x.Address.GetTotalLoggedUserAddressesAsync(Convert.ToInt32(userId), addressFilterDto))
                .ReturnsAsync(addressEntityFaker.Count);

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.GetLoggedUserAddressesAsync(addressFilterDto);

            //Assert
            Assert.True(result.Data.Any() && result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_logged_in_user_address_by_filter_exception()
        {
            //Arrange
            var userId = _claimNameIdentifier;
            var addressEntityFaker = AddressFaker.AddressEntity().Generate(3);
            var addressFilterDto = AddressFaker.AddressFilterDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetLoggedUserAddressesAsync(Convert.ToInt32(userId), addressFilterDto))
                .ThrowsAsync(new Exception());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.GetLoggedUserAddressesAsync(addressFilterDto);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_logged_in_user_address()
        {
            //Arrange
            var userId = _claimNameIdentifier;
            var addressEntityFaker = AddressFaker.AddressEntity().Generate();
            var addressIdentifierDTOFaker = AddressFaker.AddressIdentifierDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultNoTrackingAsync(x => x.UserId == Convert.ToInt32(userId)
                        && x.Id == addressIdentifierDTOFaker.AddressId))
                .ReturnsAsync(addressEntityFaker);

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.GetLoggedUserAddressAsync(addressIdentifierDTOFaker);

            //Assert
            Assert.True(result.Data != null && result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_logged_in_user_address_exception()
        {
            //Arrange
            var userId = _claimNameIdentifier;
            var addressIdentifierDTOFaker = AddressFaker.AddressIdentifierDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultNoTrackingAsync(x => x.UserId == Convert.ToInt32(userId)
                        && x.Id == addressIdentifierDTOFaker.AddressId))
                .ThrowsAsync(new Exception());

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.GetLoggedUserAddressAsync(addressIdentifierDTOFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Get_logged_in_user_address_failed()
        {
            //Arrange
            var userId = _claimNameIdentifier;
            var addressIdentifierDTOFaker = AddressFaker.AddressIdentifierDTO().Generate();

            _mockUnitOfWork.Setup(x => x.Address.GetFirstOrDefaultNoTrackingAsync(x => x.UserId == Convert.ToInt32(userId)
                        && x.Id == addressIdentifierDTOFaker.AddressId))
                .ReturnsAsync(value: null);

            var registerService = RegisterServiceConstrutor();

            //Act
            var result = await registerService.GetLoggedUserAddressAsync(addressIdentifierDTOFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        private RegisterService RegisterServiceConstrutor()
        {
            return new RegisterService(_mockUnitOfWork.Object,
                _mapper,
                _mockHttpContextAccessor.Object,
                _mockBackgroundJobClient.Object,
                _mockEmailService.Object);
        }
    }
}
