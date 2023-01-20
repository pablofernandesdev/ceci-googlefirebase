using CeciGoogleFirebase.Domain.DTO.Commons;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Service.External
{
    public interface ISendGridService
    {
        Task<ResultResponse> SendEmailAsync(string email, string subject, string message);
    }
}
