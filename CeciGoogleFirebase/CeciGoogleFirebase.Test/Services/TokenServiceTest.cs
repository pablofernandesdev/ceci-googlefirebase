using CeciGoogleFirebase.Service.Services;
using CeciGoogleFirebase.Test.Fakers.User;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;

namespace CeciGoogleFirebase.Test.Services
{
    public class TokenServiceTest
    {
        [Fact]
        public void Generate_token_successfully()
        {
            //Arrange
            var userResultFaker = UserFaker.UserResultDTO().Generate();

            var tokenService = TokenServiceConstrutor();

            //Act
            var result = tokenService.GenerateToken(userResultFaker);

            //Assert
            Assert.NotEmpty(result);
        }

        private TokenService TokenServiceConstrutor()
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"JwtToken:Secret", "g7bgM7MEkTvz33JnGkGoiQJdpELmYhVD"},
                //...populate as needed for the test
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            return new TokenService(configuration);
        }
    }
}
