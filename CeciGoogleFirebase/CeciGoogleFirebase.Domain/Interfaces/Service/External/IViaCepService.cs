using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.ViaCep;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Service.External
{
    public interface IViaCepService
    {
        Task<ResultResponse<ViaCepAddressResponseDTO>> GetAddressByZipCodeAsync(string zipCode);
    }
}
