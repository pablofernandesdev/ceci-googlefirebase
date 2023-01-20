using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Infra.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class RegistrationTokenRepository : BaseRepository<RegistrationToken>, IRegistrationTokenRepository
    {
        public RegistrationTokenRepository(AppDbContext appDbcontext) : base(appDbcontext)
        {
        }
    }
}
