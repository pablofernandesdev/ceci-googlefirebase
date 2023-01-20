using CeciGoogleFirebase.Domain.Entities;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Repository
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        Task<Role> GetBasicProfile();
    }
}
