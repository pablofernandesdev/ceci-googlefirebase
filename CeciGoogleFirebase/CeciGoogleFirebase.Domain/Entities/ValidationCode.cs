using System;

namespace CeciGoogleFirebase.Domain.Entities
{
    public class ValidationCode : BaseEntity
    {
        public string Code { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsActive => !IsExpired;

        /// <summary>
        /// User identifier
        /// </summary>
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
