using System.ComponentModel.DataAnnotations;

namespace CeciGoogleFirebase.Domain.DTO.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserUpdateDTO
    {
        /// <summary>
        /// Identifier user
        /// </summary>
        public int UserId { get; set; }

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
        /// Identifier role
        /// </summary>
        public int RoleId { get; set; }
    }
}
