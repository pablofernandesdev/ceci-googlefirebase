using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Domain.Interfaces.Repository;
using CeciGoogleFirebase.Infra.Data.Context;
using System.Diagnostics.CodeAnalysis;

namespace CeciGoogleFirebase.Infra.Data.Repository
{
    [ExcludeFromCodeCoverage]
    public class ValidationCodeRepository : BaseRepository<ValidationCode>, IValidationCodeRepository
    {
        public ValidationCodeRepository(AppDbContext appDbcontext) : base(appDbcontext)
        {
        }
    }
}
