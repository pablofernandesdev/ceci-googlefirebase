using CeciGoogleFirebase.Domain.Entities;
using CeciGoogleFirebase.Infra.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CeciGoogleFirebase.Infra.Data.Context
{
    [ExcludeFromCodeCoverage]
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }
        public DbSet<RegistrationToken> RegistrationToken { get; set; }
        public DbSet<ValidationCode> ValidationCode { get; set; }
        public DbSet<Address> Address { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(new UserMap().Configure);
            modelBuilder.Entity<Role>(new RoleMap().Configure);
            modelBuilder.Entity<RefreshToken>(new RefreshTokenMap().Configure);
            modelBuilder.Entity<RegistrationToken>(new RegistrationTokenMap().Configure);
            modelBuilder.Entity<ValidationCode>(new ValidationCodeMap().Configure);
            modelBuilder.Entity<Address>(new AddressMap().Configure);
        }

        public void Commit()
        {
            base.SaveChanges();
        }
    }
}
