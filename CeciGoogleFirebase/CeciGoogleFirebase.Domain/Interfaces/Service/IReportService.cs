using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Service
{
    public interface IReportService
    {
        Task<ResultResponse<byte[]>> GenerateUsersReport(UserFilterDTO filter);
    }
}
