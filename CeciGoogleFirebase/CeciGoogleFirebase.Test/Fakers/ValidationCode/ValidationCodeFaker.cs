using Bogus;
using CeciGoogleFirebase.Domain.DTO.ValidationCode;
using CeciGoogleFirebase.Infra.CrossCutting.Extensions;
using CeciGoogleFirebase.Test.Fakers.User;

namespace CeciGoogleFirebase.Test.Fakers.ValidationCode
{
    public class ValidationCodeFaker
    {
        public static Faker<CeciGoogleFirebase.Domain.Entities.ValidationCode> ValidationCodeEntity()
        {
            return new Faker<CeciGoogleFirebase.Domain.Entities.ValidationCode>()
                .CustomInstantiator(p => new CeciGoogleFirebase.Domain.Entities.ValidationCode
                {
                    UserId = p.Random.Int(),
                    Active = true,
                    Code = PasswordExtension.EncryptPassword(p.Random.Word()),
                    Expires = p.Date.Future(),
                    Id = p.Random.Int(),
                    RegistrationDate = p.Date.Recent(),
                    User = UserFaker.UserEntity().Generate()               
                });
        }

        public static Faker<ValidationCodeValidateDTO> ValidationCodeValidateDTO()
        {
            return new Faker<ValidationCodeValidateDTO>()
                .CustomInstantiator(p => new ValidationCodeValidateDTO
                {
                    Code = p.Random.Word()
                });
        }
    }
}
