using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Domain.Interfaces.Service;
using ClosedXML.Excel;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _uow;

        public ReportService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task <ResultResponse<byte[]>> GenerateUsersReport(UserFilterDTO filter)
        {
            var response = new ResultResponse<byte[]>();

            try
            {
                var workbookBytes = Array.Empty<byte>();

                var users = await _uow.User.GetByFilterAsync(filter);

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

                    if (users.Any())
                    {
                        foreach (var item in users)
                        {
                            worksheet.Cell("A" + currentLine).Value = item.Id;
                            worksheet.Cell("B" + currentLine).Value = item.Name;
                            worksheet.Cell("C" + currentLine).Value = item.Email;

                            currentLine++;
                        }
                    }

                    using (var ms = new MemoryStream())
                    {
                        workbook.SaveAs(ms);
                        workbookBytes = ms.ToArray();
                    }
                }

                response.Data = workbookBytes;
            }
            catch (Exception ex)
            {
                response.Message = "Unable to generate report.";
                response.Exception = ex;
            }

            return response;
        }
    }
}
