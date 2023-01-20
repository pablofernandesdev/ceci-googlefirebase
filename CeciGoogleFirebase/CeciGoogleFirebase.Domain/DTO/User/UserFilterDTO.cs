using CeciGoogleFirebase.Domain.DTO.Commons;
using System.ComponentModel.DataAnnotations;

namespace CeciGoogleFirebase.Domain.DTO.User
{
    public class UserFilterDTO : QueryFilter
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
        /// Identifier role
        /// </summary>
        public int RoleId { get; set; }
    }
}
