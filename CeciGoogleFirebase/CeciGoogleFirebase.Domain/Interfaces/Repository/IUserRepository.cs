using CeciGoogleFirebase.Domain.DTO.User;
using CeciGoogleFirebase.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByExternalIdAsync(string externalId);
        Task<IEnumerable<User>> GetByFilterAsync(UserFilterDTO filter);
        Task<int> GetTotalByFilterAsync(UserFilterDTO filter);
    }
}
