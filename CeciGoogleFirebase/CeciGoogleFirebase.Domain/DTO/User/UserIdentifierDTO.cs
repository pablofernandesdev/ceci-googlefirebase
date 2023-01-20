using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CeciGoogleFirebase.Domain.DTO.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserIdentifierDTO
    {
        /// <summary>
        /// Identifier user
        /// </summary>
        [BindProperty(Name = "userId")]
        public int UserId { get; set; }
    }
}
