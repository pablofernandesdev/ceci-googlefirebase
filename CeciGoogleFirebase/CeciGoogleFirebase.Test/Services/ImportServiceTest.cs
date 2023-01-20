using AutoMapper;
using CeciGoogleFirebase.Domain.DTO.Import;
using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using CeciGoogleFirebase.Domain.Mapping;
using CeciGoogleFirebase.Service.Services;
using CeciGoogleFirebase.Test.Fakers.Commons;
using CeciGoogleFirebase.Test.Fakers.Email;
using CeciGoogleFirebase.Test.Fakers.Role;
using CeciGoogleFirebase.Test.Fakers.User;
using ClosedXML.Excel;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace CeciGoogleFirebase.Test.Services
{
    public class ImportServiceTest
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IBackgroundJobClient> _mockBackgroundJobClient;
        private readonly IMapper _mapper;

        public ImportServiceTest()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockEmailService = new Mock<IEmailService>();
            _mockBackgroundJobClient = new Mock<IBackgroundJobClient>();

            //Auto mapper configuration
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task Import_users_successfully()
        {
            //Arrange
            var emailRequestDTOFaker = EmailFaker.EmailRequestDTO().Generate();
            var listUserFaker = UserFaker.UserImportDTO().Generate(3);

            //Setup mock file using a memory stream
            var workbookBytes = Array.Empty<byte>();

            using (var workbook = new XLWorkbook())
            {
                var currentLine = 2;

                var worksheet = workbook.Worksheets.Add("Users");

                worksheet.Cell("A1").Style.Font.SetBold();
                worksheet.Cell("B1").Style.Font.SetBold();
                worksheet.Cell("C1").Style.Font.SetBold();

                worksheet.Cell("A1").Value = "ID";
                worksheet.Cell("B1").Value = "Name";
                worksheet.Cell("C1").Value = "Email";

                worksheet.Cell("A" + currentLine).Value = "1";
                worksheet.Cell("B" + currentLine).Value = "John";
                worksheet.Cell("C" + currentLine).Value = "john@email.com";

                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    workbookBytes = ms.ToArray();
                }
            }

            var stream = new MemoryStream(workbookBytes);

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", "report.xlsx");

            //add attachment to request
            var fileUploadDto = new FileUploadDTO
            {
                File = file
            };

            _mockUnitOfWork.Setup(x => x.Role.GetBasicProfile())
                .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            _mockUnitOfWork.Setup(x => x.User.AddRangeAsync(_mapper.Map<IEnumerable<User>>(listUserFaker)))
                .ReturnsAsync(new List<User>());

            _mockEmailService.Setup(x => x.SendEmailAsync(emailRequestDTOFaker))
                .ReturnsAsync(ResultResponseFaker.ResultResponse(HttpStatusCode.OK).Generate());

            var importService = ImportServiceConstrutor();

            //Act
            var result = await importService.ImportUsersAsync(fileUploadDto);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Import_users_successfully_csv_file()
        {
            //Arrange
            var emailRequestDTOFaker = EmailFaker.EmailRequestDTO().Generate();

            //Setup mock file using a memory stream
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.WriteLine("Id;Name;Email");
            writer.WriteLine("1;John;john@email.com");
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", "report.csv");

            //add attachment to request
            var fileUploadDto = new FileUploadDTO
            {
                File = file
            };

            _mockUnitOfWork.Setup(x => x.Role.GetBasicProfile())
                .ReturnsAsync(RoleFaker.RoleEntity().Generate());

            _mockUnitOfWork.Setup(x => x.User.AddRangeAsync(new List<User>()))
                .ReturnsAsync(new List<User>());

            _mockEmailService.Setup(x => x.SendEmailAsync(emailRequestDTOFaker))
                .ReturnsAsync(ResultResponseFaker.ResultResponse(HttpStatusCode.OK).Generate());

            var importService = ImportServiceConstrutor();

            //Act
            var result = await importService.ImportUsersAsync(fileUploadDto);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.OK));
        }

        [Fact]
        public async Task Import_users_exception()
        {
            //Arrange
            var emailRequestDTOFaker = EmailFaker.EmailRequestDTO().Generate();

            //Setup mock file using a memory stream
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.WriteLine("Id;Name;Email");
            writer.WriteLine("1;John;john@email.com");
            writer.Flush();
            stream.Position = 0;

            //create FormFile with desired data
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", "report.csv");

            //add attachment to request
            var fileUploadDto = new FileUploadDTO
            {
                File = file
            };

            _mockUnitOfWork.Setup(x => x.Role.GetBasicProfile())
               .ThrowsAsync(new Exception());

            var importService = ImportServiceConstrutor();

            //Act
            var result = await importService.ImportUsersAsync(fileUploadDto);

            //Assert
            Assert.True(result.StatusCode.Equals(HttpStatusCode.InternalServerError));
        }

        private ImportService ImportServiceConstrutor()
        {
            return new ImportService(
                _mockEmailService.Object,
                _mockUnitOfWork.Object,
                _mapper,
                _mockBackgroundJobClient.Object);
        }
    }
}
