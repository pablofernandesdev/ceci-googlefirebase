using System.ComponentModel.DataAnnotations;

namespace CeciGoogleFirebase.Domain.DTO.User
{
    /// <summary>
    /// User import DTO
    /// </summary>
    public class UserImportDTO
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

        /// <summary>
        /// Decrypted user password
        /// </summary>
        public string PasswordBase64Decode { get; set; }

        /// <summary>
        /// Identifier role
        /// </summary>
        public int RoleId { get; set; }
    }
}
