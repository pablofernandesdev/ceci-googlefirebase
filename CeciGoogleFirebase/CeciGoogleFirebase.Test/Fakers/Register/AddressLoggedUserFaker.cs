using Bogus;
using CeciGoogleFirebase.Domain.DTO.Register;

namespace CeciGoogleFirebase.Test.Fakers.Register
{
    public static class AddressLoggedUserFaker
    {
        public static Faker<AddressLoggedUserAddDTO> AddressLoggedUserAddDTO()
        {
            return new Faker<AddressLoggedUserAddDTO>()
                .CustomInstantiator(p => new AddressLoggedUserAddDTO
                {
                    Uf = p.Address.CityPrefix(),
                    District = p.Address.StreetAddress(),
                    ZipCode = p.Address.ZipCode(),
                    Complement = p.Address.StreetAddress(),
                    Locality = p.Address.StreetAddress(),
                    Street = p.Address.CityPrefix(),
                    Number = p.Random.Number()
                });
        }

        public static Faker<AddressLoggedUserUpdateDTO> AddressLoggedUserUpdateDTO()
        {
            return new Faker<AddressLoggedUserUpdateDTO>()
                .CustomInstantiator(p => new AddressLoggedUserUpdateDTO
                {
                    AddressId = p.Random.Number(),
                    Uf = p.Address.CityPrefix(),
                    District = p.Address.StreetAddress(),
                    ZipCode = p.Address.ZipCode(),
                    Complement = p.Address.StreetAddress(),
                    Locality = p.Address.StreetAddress(),
                    Street = p.Address.CityPrefix(),
                    Number = p.Random.Number()
                });
        }
    }
}
