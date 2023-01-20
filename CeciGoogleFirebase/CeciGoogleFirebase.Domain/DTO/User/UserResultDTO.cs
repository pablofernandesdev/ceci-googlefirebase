using System;
using System.Collections.Generic;
using System.Text;

namespace CeciGoogleFirebase.Domain.DTO.User
{
    public class UserResultDTO
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Name user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Email user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Role user
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Is external provider
        /// </summary>
        public bool IsExternalProvider { get; set; }
    }
}
