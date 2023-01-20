using CeciGoogleFirebase.Infra.CrossCutting.Settings;
using CeciGoogleFirebase.Service.Services;
using CeciGoogleFirebase.Test.Fakers.Email;
using CeciGoogleFirebase.Test.Fakers.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciGoogleFirebase.Test.Services
{
    public class EmailServiceTest
    {
        private readonly Moq.Mock<IOptions<EmailSettings>> _emailSettings;

        public EmailServiceTest()
        {
            _emailSettings = new Moq.Mock<IOptions<EmailSettings>>();
        }

        [Fact]
        public async Task Send_email_successfully()
        {
            //Arrange
            var emailRequestDTOFaker = EmailFaker.EmailRequestDTO().Generate();
            _emailSettings.Setup(ap => ap.Value).Returns(EmailSettingsFaker.EmailSettings().Generate());

            var emailService = EmailServiceConstrutor();

            //Act
            var result = await emailService.SendEmailAsync(emailRequestDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Send_email_with_attachment_successfully()
        {
            //Arrange
            var emailRequestDTOFaker = EmailFaker.EmailRequestDTO().Generate();
            _emailSettings.Setup(ap => ap.Value).Returns(EmailSettingsFaker.EmailSettings().Generate());

            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            //add attachment to request
            emailRequestDTOFaker.Attachments = new List<IFormFile>
            {
                file
            };

            var emailService = EmailServiceConstrutor();

            //Act
            var result = await emailService.SendEmailAsync(emailRequestDTOFaker);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Send_email_exception_invalid_port()
        {
            //Arrange
            var emailSettings = EmailSettingsFaker.EmailSettings().Generate();
            //set invalid port
            emailSettings.Port = 9999999;

            var emailRequestDTOFaker = EmailFaker.EmailRequestDTO().Generate();
            _emailSettings.Setup(ap => ap.Value).Returns(emailSettings);

            var emailService = EmailServiceConstrutor();

            //Act
            var result = await emailService.SendEmailAsync(emailRequestDTOFaker);

            //Assert
            Assert.False(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        private EmailService EmailServiceConstrutor()
        {
            return new EmailService(_emailSettings.Object);
        }
    }
}
