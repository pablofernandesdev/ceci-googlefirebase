using Bogus;
using CeciGoogleFirebase.Test.Fakers.User;

namespace CeciGoogleFirebase.Test.Fakers.RegistrationToken
{
    public class RegistrationTokenFaker
    {
        public static Faker<CeciGoogleFirebase.Domain.Entities.RegistrationToken> RegistrationTokenEntity()
        {
            return new Faker<CeciGoogleFirebase.Domain.Entities.RegistrationToken>()
                .CustomInstantiator(p => new CeciGoogleFirebase.Domain.Entities.RegistrationToken
                {
                    Active = true,
                    UserId = p.Random.Int(),
                    Id = p.Random.Int(),
                    RegistrationDate = p.Date.Recent(),
                    Token = p.Random.String2(30),
                    User = UserFaker.UserEntity().Generate()
                });
        }
    }
}
