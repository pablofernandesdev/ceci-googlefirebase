using Newtonsoft.Json;

namespace CeciGoogleFirebase.Domain.DTO.Address
{
    public class AddressResultDTO
    {
        [JsonProperty("zipCode")]
        public string ZipCode { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("district")]
        public string District { get; set; }

        [JsonProperty("locality")]
        public string Locality { get; set; }

        [JsonProperty("uf")]
        public string Uf { get; set; }

        [JsonProperty("complement")]
        public string Complement { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }
    }
}
