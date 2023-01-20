using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CeciGoogleFirebase.Domain.DTO.Auth
{
    public class AuthResultDTO
    {
        /// <summary>
        /// User identifier code
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// User role
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Token jwt
        /// </summary>
        public string Token { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
    }
}
