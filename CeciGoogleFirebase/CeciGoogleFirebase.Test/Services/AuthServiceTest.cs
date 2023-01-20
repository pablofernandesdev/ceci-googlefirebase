using AutoMapper;
using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using CeciGoogleFirebase.Domain.Mapping;
using CeciGoogleFirebase.Infra.CrossCutting.Extensions;
using CeciGoogleFirebase.Service.Services;
using CeciGoogleFirebase.Test.Fakers.Auth;
using CeciGoogleFirebase.Test.Fakers.RefreshToken;
using CeciGoogleFirebase.Test.Fakers.User;
using Hangfire;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciGoogleFirebase.Test.Services
{
    public class AuthServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ITokenService> _mockTokenService;
        private readonly IMapper _mapper;
        private readonly Mock<IBackgroundJobClient> _mockBackgroundJobClient;
        private readonly Mock<IEmailService> _mockEmailService;

        public AuthServiceTest()
        {
            _mockUnitOfWork = new Moq.Mock<IUnitOfWork>();
            _mockTokenService = new Moq.Mock<ITokenService>();
            _mockBackgroundJobClient = new Mock<IBackgroundJobClient>();
            _mockEmailService = new Mock<IEmailService>();

            //Auto mapper configuration
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Authenticate_successfully()
        {
            //Arrange
            var userEntityFaker = UserFaker.UserEntity().Generate();
            var loginDTOFaker = AuthFaker.LoginDTO().Generate();
            var userValidFaker = new CeciGoogleFirebase.Domain.Entities.User
            {
                Id = userEntityFaker.Id,
                Name = userEntityFaker.Name,
                Email = userEntityFaker.Email,
                Password = userEntityFaker.Password,
                RoleId = userEntityFaker.RoleId,
                Role = userEntityFaker.Role
            };
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenEntity().Generate();
            //var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";

            userEntityFaker.Password = PasswordExtension.EncryptPassword(loginDTOFaker.Password);

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultNoTrackingAsync(c => c.Email.Equals(loginDTOFaker.Username)))
                .ReturnsAsync(userEntityFaker);

            _mockUnitOfWork.Setup(x => x.User.GetUserByIdAsync(userEntityFaker.Id))
                .ReturnsAsync(userValidFaker);

            //validar porque o valor esta sendo retornado nulo
            _mockTokenService.Setup(x => x.GenerateToken(_mapper.Map<UserResultDTO>(userValidFaker)))
                .Returns(It.IsAny<string>());

            _mockUnitOfWork.Setup(x => x.RefreshToken.AddAsync(refreshTokenFaker))
                .ReturnsAsync(refreshTokenFaker);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.AuthenticateAsync(loginDTOFaker, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Authenticate_unauthorized_password_incorret()
        {
            //Arrange
            var userEntityFaker = UserFaker.UserEntity().Generate();
            var loginDTOFaker = AuthFaker.LoginDTO().Generate();

            userEntityFaker.Password = PasswordExtension.EncryptPassword("bm92b3Rlc3Rl");

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultNoTrackingAsync(c => c.Email.Equals(loginDTOFaker.Username)))
                .ReturnsAsync(userEntityFaker);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.AuthenticateAsync(loginDTOFaker, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.Unauthorized));
        }

        [Fact]
        public async Task Authenticate_user_not_found()
        {
            //Arrange
            var loginDTOFaker = AuthFaker.LoginDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(c => c.Email.Equals(loginDTOFaker.Username)))
                .ReturnsAsync(value: null);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.AuthenticateAsync(loginDTOFaker, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.Unauthorized));
        }

        [Fact]
        public async Task Authenticate_exception()
        {
            //Arrange
            var loginDTOFaker = AuthFaker.LoginDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultNoTrackingAsync(c => c.Email.Equals(loginDTOFaker.Username)))
                .ThrowsAsync(new Exception());

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.AuthenticateAsync(loginDTOFaker, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        [Fact]
        public async Task Refresh_token_successfully()
        {
            //Arrange
            var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenEntity().Generate();

            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync(c => c.Token.Equals(jwtToken)))
                .ReturnsAsync(refreshTokenFaker);

            //validar porque o valor esta sendo retornado nulo
            _mockTokenService.Setup(x => x.GenerateToken(_mapper.Map<UserResultDTO>(refreshTokenFaker.User)))
                .Returns(It.IsAny<string>());

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RefreshTokenAsync(jwtToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Refresh_token_expired()
        {
            //Arrange
            var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenExpiredEntity().Generate();

            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync(c => c.Token.Equals(jwtToken)))
                .ReturnsAsync(refreshTokenFaker);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RefreshTokenAsync(jwtToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.Unauthorized));
        }

        [Fact]
        public async Task Refresh_token_exception()
        {
            //Arrange
            var jwtToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync(c => c.Token.Equals(jwtToken)))
                .ThrowsAsync(new Exception());

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RefreshTokenAsync(jwtToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        [Fact]
        public async Task Revoke_token_successfully()
        {
            //Arrange
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenEntity().Generate();
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync(c => c.Token.Equals(refreshToken) && c.IsActive))
                .ReturnsAsync(refreshTokenFaker);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RevokeTokenAsync(refreshToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Revoke_token_null_token()
        {
            //Arrange
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenEntity().Generate();
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync(c => c.Token.Equals(refreshToken) && c.IsActive))
                .ReturnsAsync(value: null);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RevokeTokenAsync(refreshToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.Unauthorized));
        }

        [Fact]
        public async Task Revoke_token_exception()
        {
            //Arrange
            var refreshTokenFaker = RefreshTokenFaker.RefreshTokenEntity().Generate();
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";

            _mockUnitOfWork.Setup(x => x.RefreshToken.GetFirstOrDefaultAsync(c => c.Token.Equals(refreshToken) && c.IsActive))
                 .ThrowsAsync(new Exception());

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.RevokeTokenAsync(refreshToken, "127.0.0.1");

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        [Fact]
        public async Task Forgot_password_successfully()
        {
            //Arrange
            var userEntityFaker = UserFaker.UserEntity().Generate();
            var forgotPasswordDtoFaker = AuthFaker.ForgotPasswordDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(c => c.Email.Equals(forgotPasswordDtoFaker.Email)))
                .ReturnsAsync(userEntityFaker);

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.ForgotPasswordAsync(forgotPasswordDtoFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Forgot_password_exception()
        {
            //Arrange
            var forgotPasswordDtoFaker = AuthFaker.ForgotPasswordDTO().Generate();

            _mockUnitOfWork.Setup(x => x.User.GetFirstOrDefaultAsync(c => c.Email.Equals(forgotPasswordDtoFaker.Email)))
            .ThrowsAsync(new Exception());

            var authService = AuthServiceConstrutor();

            //act
            var result = await authService.ForgotPasswordAsync(forgotPasswordDtoFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        private AuthService AuthServiceConstrutor()
        {
            return new AuthService(_mockTokenService.Object,
                _mockEmailService.Object,
                _mockUnitOfWork.Object,
                _mapper,
                _mockBackgroundJobClient.Object);
        }
    }
}
