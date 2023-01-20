using CeciGoogleFirebase.Domain.DTO.Commons;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Service.External
{
    public interface IFirebaseService
    {
        Task<ResultResponse> SendNotificationAsync(string token, string title, string body);
    }
}
