using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.DTO.Commons;
using CeciGoogleFirebase.Domain.DTO.Register;
using CeciGoogleFirebase.Domain.DTO.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Service
{
    public interface IRegisterService
    {
        Task<ResultResponse<UserResultDTO>> GetLoggedInUserAsync();
        Task<ResultResponse> SelfRegistrationAsync(UserSelfRegistrationDTO obj);
        Task<ResultResponse> UpdateLoggedUserAsync(UserLoggedUpdateDTO obj);
        Task<ResultResponse> RedefinePasswordAsync(UserRedefinePasswordDTO obj);
        Task<ResultResponse> AddLoggedUserAddressAsync(AddressLoggedUserAddDTO obj);
        Task<ResultResponse> UpdateLoggedUserAddressAsync(AddressLoggedUserUpdateDTO obj);
        Task<ResultResponse> InactivateLoggedUserAddressAsync(AddressDeleteDTO obj);
        Task<ResultDataResponse<IEnumerable<AddressResultDTO>>> GetLoggedUserAddressesAsync(AddressFilterDTO filter);
        Task<ResultResponse<AddressResultDTO>> GetLoggedUserAddressAsync(AddressIdentifierDTO obj);
        Task<ResultResponse> RegisterWithLoginProviderAsync();
    }
}
