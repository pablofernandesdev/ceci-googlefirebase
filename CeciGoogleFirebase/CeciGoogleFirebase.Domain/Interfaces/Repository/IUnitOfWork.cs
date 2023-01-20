using System;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Domain.Interfaces.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository User { get; }
        IRoleRepository Role { get; }
        IRefreshTokenRepository RefreshToken { get; }
        IRegistrationTokenRepository RegistrationToken { get; }
        IValidationCodeRepository ValidationCode { get; }
        IAddressRepository Address { get; }
        Task CommitAsync();
    }
}
