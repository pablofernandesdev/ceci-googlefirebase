using Bogus;
using CeciGoogleFirebase.Domain.DTO.Auth;

namespace CeciGoogleFirebase.Test.Fakers.Auth
{
    public static class AuthFaker
    {
        public static Faker<LoginDTO> LoginDTO()
        {
            return new Faker<LoginDTO>()
                .CustomInstantiator(p => new LoginDTO
                {
                    Username = p.Person.UserName,
                    Password = "dGVzdGU="
                });
        }

        public static Faker<AuthResultDTO> AuthResultDTO()
        {
            return new Faker<AuthResultDTO>()
                .CustomInstantiator(p => new AuthResultDTO
                {
                    Username = p.Person.UserName,
                    Name = p.Person.FirstName,
                    RefreshToken = p.Random.String2(20),
                    Role = p.Random.Word(),
                    UserId = p.Random.Int(),
                    Token = p.Random.String2(30)
                });
        }

        public static Faker<ForgotPasswordDTO> ForgotPasswordDTO()
        {
            return new Faker<ForgotPasswordDTO>()
                .CustomInstantiator(p => new ForgotPasswordDTO
                {
                    Email = p.Person.Email
                });
        }
    }
}
