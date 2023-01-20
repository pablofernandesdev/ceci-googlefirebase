using Bogus;
using CeciGoogleFirebase.Domain.DTO.Role;
using CeciGoogleFirebase.Test.Fakers.User;
using System.Collections.Generic;

namespace CeciGoogleFirebase.Test.Fakers.Role
{
    public static class RoleFaker
    {
        public static Faker<CeciGoogleFirebase.Domain.Entities.Role> RoleEntity()
        {
            return new Faker<CeciGoogleFirebase.Domain.Entities.Role>()
                .CustomInstantiator(p => new CeciGoogleFirebase.Domain.Entities.Role
                {
                    Active = true,
                    Id = p.Random.Int(1, 2),
                    Name = p.Random.Word(),
                    RegistrationDate = p.Date.Recent(),
                    User = new List<CeciGoogleFirebase.Domain.Entities.User>()                   
                });
        }

        public static Faker<RoleAddDTO> RoleAddDTO()
        {
            return new Faker<RoleAddDTO>()
                .CustomInstantiator(p => new RoleAddDTO
                {
                    Name = p.Random.Word(),
                });
        }

        public static Faker<RoleUpdateDTO> RoleUpdateDTO()
        {
            return new Faker<RoleUpdateDTO>()
                .CustomInstantiator(p => new RoleUpdateDTO
                {
                    Name = p.Random.Word(),
                    RoleId = p.Random.Int()
                });
        }

        public static Faker<RoleResultDTO> RoleResultDTO()
        {
            return new Faker<RoleResultDTO>()
                .CustomInstantiator(p => new RoleResultDTO
                {
                    Name = p.Random.Word(),
                    RoleId = p.Random.Int()
                });
        }
    }
}
