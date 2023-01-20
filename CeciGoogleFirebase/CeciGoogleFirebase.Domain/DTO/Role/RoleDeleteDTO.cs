using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CeciGoogleFirebase.Domain.DTO.Role
{
    public class RoleDeleteDTO
    {
        /// <summary>
        /// Identifier user
        /// </summary>
        [BindProperty(Name = "roleId")]
        public int RoleId { get; set; }
    }
}
