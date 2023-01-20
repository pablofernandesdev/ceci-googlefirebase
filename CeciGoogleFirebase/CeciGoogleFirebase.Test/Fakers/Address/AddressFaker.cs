using Bogus;
using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Test.Fakers.User;
using System;

namespace CeciGoogleFirebase.Test.Fakers.Address
{
    public static class AddressFaker
    {
        public static Faker<CeciGoogleFirebase.Domain.Entities.Address> AddressEntity()
        {
            return new Faker<CeciGoogleFirebase.Domain.Entities.Address>()
                .CustomInstantiator(p => new CeciGoogleFirebase.Domain.Entities.Address
                {
                    Id = p.Random.Int(),
                    Active = p.Random.Bool(),
                    RegistrationDate = p.Date.Past(),
                    User = UserFaker.UserEntity().Generate(),                   
                    Complement = p.Address.StreetAddress(),
                    District = p.Address.StreetAddress(),
                    Locality = p.Address.Locale,
                    Number = Convert.ToInt32(p.Address.BuildingNumber()),
                    Street = p.Address.StreetAddress(),
                    Uf = p.Address.CityPrefix(),
                    UserId = p.Random.Int(),
                    ZipCode = p.Address.ZipCode()
                });
        }

        public static Faker<AddressUpdateDTO> AddressUpdateDTO()
        {
            return new Faker<AddressUpdateDTO>()
                .CustomInstantiator(p => new AddressUpdateDTO
                {
                    AddressId = p.Random.Int(),
                    Complement = p.Address.StreetAddress(),
                    District = p.Address.StreetAddress(),
                    Locality = p.Address.Locale,
                    Number = Convert.ToInt32(p.Address.BuildingNumber()),
                    Street = p.Address.StreetAddress(),
                    Uf = p.Address.CityPrefix(),
                    UserId = p.Random.Int(),
                    ZipCode = p.Address.ZipCode()
                });
        }

        public static Faker<AddressFilterDTO> AddressFilterDTO()
        {
            return new Faker<AddressFilterDTO>()
                .CustomInstantiator(p => new AddressFilterDTO
                {
                    District = p.Address.CityPrefix(),
                    Locality = p.Locale,
                    Page = p.Random.Int(),
                    PerPage = p.Random.Int(),
                    Search = p.Random.Word(),
                    Uf = p.Address.CityPrefix()
                });
        }

        public static Faker<AddressAddDTO> AddressAddDTO()
        {
            return new Faker<AddressAddDTO>()
                .CustomInstantiator(p => new AddressAddDTO
                {
                    Complement = p.Address.StreetAddress(),
                    District = p.Address.StreetAddress(),
                    Locality = p.Address.Locale,
                    Number = Convert.ToInt32(p.Address.BuildingNumber()),
                    Street = p.Address.StreetAddress(),
                    Uf = p.Address.CityPrefix(),
                    UserId = p.Random.Int(),
                    ZipCode = p.Address.ZipCode()
                });
        }

        public static Faker<AddressZipCodeDTO> AddressZipCodeDTO()
        {
            return new Faker<AddressZipCodeDTO>()
                .CustomInstantiator(p => new AddressZipCodeDTO
                {
                    ZipCode = p.Address.ZipCode()
                });
        }

        public static Faker<AddressIdentifierDTO> AddressIdentifierDTO()
        {
            return new Faker<AddressIdentifierDTO>()
                .CustomInstantiator(p => new AddressIdentifierDTO
                {
                    AddressId = p.Random.Int()
                });
        }

        public static Faker<AddressDeleteDTO> AddressDeleteDTO()
        {
            return new Faker<AddressDeleteDTO>()
                .CustomInstantiator(p => new AddressDeleteDTO
                {
                    AddressId = p.Random.Int()
                });
        }

        public static Faker<AddressResultDTO> AddressResultDTO()
        {
            return new Faker<AddressResultDTO>()
                .CustomInstantiator(p => new AddressResultDTO
                {
                    Uf = p.Address.CityPrefix(),
                    District = p.Address.StreetAddress(),
                    ZipCode = p.Address.ZipCode(),
                    Complement = p.Address.StreetAddress(),
                    Locality = p.Address.StreetAddress(),
                    Street = p.Address.CityPrefix(),
                    Number = p.Address.BuildingNumber()
                });
        }
    }
}
