using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Infra.CrossCutting.Settings;
using CeciGoogleFirebase.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Infra.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        private readonly RoleSettings _roleSettings;

        public RoleRepository(AppDbContext appDbcontext, IOptions<RoleSettings> roleSettings) : base(appDbcontext)
        {
            _roleSettings = roleSettings.Value;
        }

        public async Task<Role> GetBasicProfile()
        {
            var basicProfile = await _appDbContext.Set<Role>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Name.Equals(_roleSettings.BasicRoleName));

            if (basicProfile == null)
            {
                throw new System.Exception("Basic profile not configured.");
            }

            return basicProfile;
        }
    }
}
