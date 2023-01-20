using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CeciGoogleFirebase.Domain.DTO.Role
{
    /// <summary>
    /// 
    /// </summary>
    public class IdentifierRoleDTO
    {
        /// <summary>
        /// Identifier user
        /// </summary>
        [BindProperty(Name = "roleId")]
        public int RoleId { get; set; }
    }
}
