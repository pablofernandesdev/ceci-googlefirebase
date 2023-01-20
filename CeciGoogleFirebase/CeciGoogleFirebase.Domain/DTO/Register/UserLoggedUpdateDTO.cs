using System.ComponentModel.DataAnnotations;

namespace CeciGoogleFirebase.Domain.DTO.Register
{
    public class UserLoggedUpdateDTO
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
    }
}
