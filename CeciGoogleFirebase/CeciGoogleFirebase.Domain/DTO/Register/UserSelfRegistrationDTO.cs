using System.ComponentModel.DataAnnotations;

namespace CeciGoogleFirebase.Domain.DTO.Register
{
    /// <summary>
    /// User self registration DTO
    /// </summary>
    public class UserSelfRegistrationDTO
    {
        /// <summary>
        /// Name user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email user
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Password user
        /// </summary>
        public string Password { get; set; }
    }
}
