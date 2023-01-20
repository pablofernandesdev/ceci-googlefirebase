using CeciGoogleFirebase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace CeciGoogleFirebase.Infra.Data.Mapping
{
    [ExcludeFromCodeCoverage]
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("varchar(100)");

            builder.Property(prop => prop.Email)
               .IsRequired()
               .HasColumnName("Email")
               .HasColumnType("varchar(100)");

            builder.Property(prop => prop.Password)
                .HasColumnName("Password")
                .HasColumnType("varchar(100)");

            builder.Property(prop => prop.Validated)
                .HasColumnName("Validated")
                .HasColumnType("tinyint(1)")
                .HasDefaultValue(0);

            builder.Property(prop => prop.ChangePassword)
                .HasColumnName("ChangePassword")
                .HasColumnType("tinyint(1)")
                .HasDefaultValue(0);

            builder.Property(prop => prop.ExternalId)
               .HasColumnName("ExternalId")
               .HasColumnType("varchar(100)");

            builder.Property(prop => prop.IsExternalProvider)
                .HasColumnName("IsExternalProvider")
                .HasColumnType("tinyint(1)")
                .HasDefaultValue(0);

            builder.HasOne(d => d.Role)
                .WithMany(p => p.User)
                .HasForeignKey(p => p.RoleId)
                .IsRequired();
        }
    }
}
