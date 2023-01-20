using Hangfire.Annotations;
using Hangfire.Dashboard;
using System;
using System.Net;
using System.Text;

namespace CeciGoogleFirebase.WebApplication.Filters
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string _user;
        private readonly string _password;

        public HangfireAuthorizationFilter(string user, string password)
        {
            _user = user;
            _password = password;
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            string authHeader = httpContext.Request.Headers["Authorization"];

            if (authHeader != null && authHeader.StartsWith("Basic "))
            {
                // Get the encoded username and password
                var encodedUsernamePassword = authHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries)[1]?.Trim();

                // Decode from Base64 to string
                var decodedUsernamePassword = Encoding.UTF8.GetString(Convert.FromBase64String(encodedUsernamePassword));

                // Split username and password
                var username = decodedUsernamePassword.Split(':', 2)[0];
                var password = decodedUsernamePassword.Split(':', 2)[1];

                // Check if login is correct
                if (IsAuthorized(username, password))
                {
                    return true;
                }
            }

            // Return authentication type (causes browser to show login dialog)
            httpContext.Response.Headers["WWW-Authenticate"] = "Basic";

            // Return unauthorized
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            return false;
        }

        private bool IsAuthorized(string username, string password)
        {
            // Check that username and password are correct
            return username.Equals(_user, StringComparison.InvariantCultureIgnoreCase)
                    && password.Equals(_password);
        }
    }
}
