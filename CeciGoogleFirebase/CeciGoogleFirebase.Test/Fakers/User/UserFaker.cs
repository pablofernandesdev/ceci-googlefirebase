using Bogus;
using CeciGoogleFirebase.Domain.DTO.Register;
using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Infra.CrossCutting.Extensions;
using CeciGoogleFirebase.Infra.CrossCutting.Helper;
using System.Collections.Generic;

namespace CeciGoogleFirebase.Test.Fakers.User
{
    public static class UserFaker
    {
        public static Faker<CeciGoogleFirebase.Domain.Entities.User> UserEntity()
        {
            return new Faker<CeciGoogleFirebase.Domain.Entities.User>()
                .CustomInstantiator(p => new CeciGoogleFirebase.Domain.Entities.User
                {
                    Id = p.Random.Int(1, 99),
                    Name = p.Person.FullName,
                    Email = p.Person.Email,
                    Password = PasswordExtension.EncryptPassword("dGVzdGUy"),
                    RoleId = p.Random.Int(1, 2),
                    Role = new Domain.Entities.Role
                    {
                        Active = true,
                        Id = p.Random.Int(1, 2),
                        Name = p.Random.Word(),
                        RegistrationDate = p.Date.Recent()
                    },
                    ChangePassword = p.Random.Bool(),
                    Active = p.Random.Bool(),
                    RefreshToken = new List<CeciGoogleFirebase.Domain.Entities.RefreshToken>(),
                    RegistrationDate = p.Date.Recent(),
                    RegistrationToken = new List<CeciGoogleFirebase.Domain.Entities.RegistrationToken>(),
                    Validated = p.Random.Bool(),
                    ValidationCode = new List<CeciGoogleFirebase.Domain.Entities.ValidationCode>(),
                    Address = new List<Domain.Entities.Address>()                   
                });
        }

        public static Faker<UserAddDTO> UserAddDTO()
        {
            return new Faker<UserAddDTO>()
                .CustomInstantiator(p => new UserAddDTO
                {
                    Name = p.Person.FullName,
                    Email = p.Person.Email,
                    Password = StringHelper.Base64Encode(p.Random.Word()),
                    RoleId = p.Random.Int(1, 2)
                });
        }

        public static Faker<UserSelfRegistrationDTO> UserSelfRegistrationDTO()
        {
            return new Faker<UserSelfRegistrationDTO>()
                .CustomInstantiator(p => new UserSelfRegistrationDTO
                {
                    Name = p.Person.FullName,
                    Email = p.Person.Email,
                    Password = StringHelper.Base64Encode(p.Random.Word())
                });
        }

        public static Faker<UserImportDTO> UserImportDTO()
        {
            return new Faker<UserImportDTO>()
                .CustomInstantiator(p => new UserImportDTO
                {
                    Name = p.Person.FullName,
                    Email = p.Person.Email,
                    Password = StringHelper.Base64Encode(p.Random.Word()),
                    RoleId = p.Random.Int(1, 2),
                    PasswordBase64Decode = p.Random.Word(),
                });
        }

        public static Faker<UserResultDTO> UserResultDTO()
        {
            return new Faker<UserResultDTO>()
                .CustomInstantiator(p => new UserResultDTO
                {
                    Name = p.Person.FullName,
                    Email = p.Person.Email,
                    UserId = p.Random.Int(1, 2),
                    Username = p.Person.Email,
                    Role = p.Random.String2(10)
                });
        }

        public static Faker<UserDeleteDTO> UserDeleteDTO()
        {
            return new Faker<UserDeleteDTO>()
                .CustomInstantiator(p => new UserDeleteDTO
                {
                    UserId = p.Random.Int(1, 2)
                });
        }

        public static Faker<UserUpdateDTO> UserUpdateDTO()
        {
            return new Faker<UserUpdateDTO>()
                .CustomInstantiator(p => new UserUpdateDTO
                {
                    UserId = p.Random.Int(1, 2),
                    Name = p.Person.FullName,
                    Email = p.Person.Email,
                    Password = StringHelper.Base64Encode(p.Random.Word()),
                    RoleId = p.Random.Int(1, 2)
                });
        }

        public static Faker<UserUpdateRoleDTO> UserUpdateRoleDTO()
        {
            return new Faker<UserUpdateRoleDTO>()
                .CustomInstantiator(p => new UserUpdateRoleDTO
                {
                    UserId = p.Random.Int(1, 2),
                    RoleId = p.Random.Int(1, 2)
                });
        }

        public static Faker<UserFilterDTO> UserFilterDTO()
        {
            return new Faker<UserFilterDTO>()
                .CustomInstantiator(p => new UserFilterDTO
                {
                    Name = p.Person.FullName,
                    Email = p.Person.Email,
                    Search = p.Random.Word(),
                    RoleId = p.Random.Int(1, 2),
                    Page = 1,
                    PerPage = 10
                });
        }

        public static Faker<UserRedefinePasswordDTO> UserRedefinePasswordDTO()
        {
            return new Faker<UserRedefinePasswordDTO>()
                .CustomInstantiator(p => new UserRedefinePasswordDTO
                {
                    CurrentPassword = "dGVzdGUx",
                    NewPassword = "dGVzdGUy"
                });
        }

        public static Faker<UserLoggedUpdateDTO> UserLoggedUpdateDTO()
        {
            return new Faker<UserLoggedUpdateDTO>()
                .CustomInstantiator(p => new UserLoggedUpdateDTO
                {
                    Name = p.Person.FullName,
                    Email = p.Person.Email
                });
        }
    }
}
