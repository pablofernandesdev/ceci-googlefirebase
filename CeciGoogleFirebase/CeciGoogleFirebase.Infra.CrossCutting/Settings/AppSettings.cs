using System.Diagnostics.CodeAnalysis;

namespace CeciGoogleFirebase.Infra.CrossCutting.Settings
{
    [ExcludeFromCodeCoverage]
    public class ConnectionStrings
    {
        public string CeciDatabase { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class SwaggerSettings
    {
        /// <summary>
        /// Authorized swagger user
        /// </summary>
        public string SwaggerUserAuthorized { get; set; }

        /// <summary>
        /// Swagger authorized user password
        /// </summary>
        public string SwaggerAuthorizedPassword { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ExternalProviders
    {
        public SendGrid SendGrid { get; set; }
        public Firebase Firebase { get; set; }
        public ViaCep ViaCep { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class SendGrid
    {
        public string ApiKey { get; set; }

        public string SenderEmail { get; set; }

        public string SenderName { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class Firebase
    {
        public string ServerApiKey { get; set; }

        public string SenderId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ViaCep
    {
        public string ApiUrl { get; set; }
    }

    public class EmailSettings
    {
        public string Mail { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class RoleSettings
    {
        public string BasicRoleName { get; set; }
    }
}
