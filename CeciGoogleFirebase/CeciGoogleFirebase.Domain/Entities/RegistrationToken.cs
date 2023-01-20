namespace CeciGoogleFirebase.Domain.Entities
{
    public class RegistrationToken : BaseEntity
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public int UserId { get; set; }

        public string Token { get; set; }

        public virtual User User { get; set; }
    }
}
