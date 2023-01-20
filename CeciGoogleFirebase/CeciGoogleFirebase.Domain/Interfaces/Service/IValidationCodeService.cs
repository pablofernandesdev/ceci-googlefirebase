using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.ValidationCode;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Service
{
    public interface IValidationCodeService
    {
        Task<ResultResponse> SendAsync();
        Task<ResultResponse> ValidateCodeAsync(ValidationCodeValidateDTO obj);
    }
}
