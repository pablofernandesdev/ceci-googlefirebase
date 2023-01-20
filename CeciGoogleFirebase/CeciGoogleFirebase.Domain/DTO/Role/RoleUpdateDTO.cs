using System;
using System.Collections.Generic;
using System.Text;

namespace CeciGoogleFirebase.Domain.DTO.Role
{
    /// <summary>
    /// Role update DTO
    /// </summary>
    public class RoleUpdateDTO
    {
        /// <summary>
        /// Identifier role
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// Name role
        /// </summary>
        public string Name { get; set; }
    }
}
