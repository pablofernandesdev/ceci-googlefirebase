using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.Import;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Service
{
    public interface IImportService
    {
        Task<ResultResponse> ImportUsersAsync(FileUploadDTO model);
    }
}
