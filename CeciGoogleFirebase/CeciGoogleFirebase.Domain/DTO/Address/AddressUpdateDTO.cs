namespace CeciGoogleFirebase.Domain.DTO.Address
{
    public class AddressUpdateDTO
    {
        public int AddressId { get; set; }

        public int UserId { get; set; }

        public string ZipCode { get; set; }

        public string Street { get; set; }

        public string District { get; set; }

        public string Locality { get; set; }

        public int Number { get; set; }

        public string Complement { get; set; }

        public string Uf { get; set; }
    }
}
