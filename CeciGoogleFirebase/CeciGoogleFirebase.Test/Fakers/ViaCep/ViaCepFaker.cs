using Bogus;
using CeciGoogleFirebase.Domain.DTO.ViaCep;
using System;

namespace CeciGoogleFirebase.Test.Fakers.ViaCep
{
    public static class ViaCepFaker
    {
        public static Faker<ViaCepAddressResponseDTO> ViaCepAddressResponseDTO()
        {
            return new Faker<ViaCepAddressResponseDTO>()
                .CustomInstantiator(p => new ViaCepAddressResponseDTO
                {
                    Bairro = p.Address.StreetAddress(),
                    Cep = p.Address.ZipCode(),
                    Complemento = p.Address.FullAddress(),
                    Ddd = Convert.ToString(p.Random.Int(1, 3)),
                    Gia = Convert.ToString(p.Random.Int(1, 3)),
                    Ibge = Convert.ToString(p.Random.Int(1, 3)),
                    Localidade = p.Address.Locale,
                    Logradouro = p.Address.StreetName(),
                    Siafi = Convert.ToString(p.Random.Int(1, 3)),
                    Uf = p.Address.CityPrefix()
                });
        }
    }
}
