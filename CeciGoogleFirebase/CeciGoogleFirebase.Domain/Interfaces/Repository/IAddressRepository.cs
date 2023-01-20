using CeciGoogleFirebase.Domain.DTO.Address;
using CeciGoogleFirebase.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Repository
{
    public interface IAddressRepository : IBaseRepository<Address>
    {
        Task<IEnumerable<Address>> GetLoggedUserAddressesAsync(int userId, AddressFilterDTO filter);
        Task<int> GetTotalLoggedUserAddressesAsync(int userId, AddressFilterDTO filter);
        Task<IEnumerable<Address>> GetByFilterAsync(AddressFilterDTO filter);
        Task<int> GetTotalByFilterAsync(AddressFilterDTO filter);
    }
}
