using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.Interfaces.Service.External;
using CeciGoogleFirebase.Infra.CrossCutting.Settings;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Services.External
{
    public class SendGridService : ISendGridService
    {
        private readonly ExternalProviders _externalProviders;

        public SendGridService(IOptions<ExternalProviders> externalProviders)
        {
            _externalProviders = externalProviders.Value;
        }

        public async Task<ResultResponse> SendEmailAsync(
                  string email,
                  string subject,
                  string message)
        {
            var response = new ResultResponse();

            var result = await Execute(_externalProviders.SendGrid.ApiKey, subject, message, email);

            response.StatusCode = result.StatusCode;
            response.Message = await result.Body.ReadAsStringAsync();

            return response;
        }

        private async Task<Response> Execute(
            string apiKey,
            string subject,
            string message,
            string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_externalProviders.SendGrid.SenderEmail, _externalProviders.SendGrid.SenderName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // disable tracking settings
            // ref.: https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);
            msg.SetOpenTracking(false);
            msg.SetGoogleAnalytics(false);
            msg.SetSubscriptionTracking(false);

            return await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}