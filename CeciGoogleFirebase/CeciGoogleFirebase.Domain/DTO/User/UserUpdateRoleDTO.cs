using System;
using System.Collections.Generic;
using System.Text;

namespace CeciGoogleFirebase.Domain.DTO.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserUpdateRoleDTO
    {
        /// <summary>
        /// Identifier user
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Identifier role
        /// </summary>
        public int RoleId { get; set; }
    }
}
