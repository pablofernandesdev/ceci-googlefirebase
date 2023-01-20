namespace CeciGoogleFirebase.Domain.DTO.Notification
{
    /// <summary>
    /// Send notification
    /// </summary>
    public class NotificationSendDTO
    {
        /// <summary>
        /// Identifier user 
        /// </summary>
        public int IdUser { get; set; }

        /// <summary>
        /// Notification title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Notification body
        /// </summary>
        public string Body { get; set; }
    }
}
