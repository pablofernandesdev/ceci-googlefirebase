using CeciGoogleFirebase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace CeciGoogleFirebase.Infra.Data.Mapping
{
    [ExcludeFromCodeCoverage]
    public class RefreshTokenMap : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken");

            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Token)
                .IsRequired()
                .HasColumnName("Token")
                .HasColumnType("varchar(2000)");

            builder.Property(prop => prop.Expires)
                .IsRequired()
                .HasColumnName("Expires")
                .HasColumnType("datetime");

            builder.Property(prop => prop.CreatedByIp)
                .IsRequired()
                .HasColumnName("CreatedByIp")
                .HasColumnType("varchar(100)");

            builder.Property(prop => prop.Revoked)
                .IsRequired(false)
                .HasColumnName("Revoked")
                .HasColumnType("datetime");

            builder.Property(prop => prop.RevokedByIp)
                .IsRequired(false)
                .HasColumnName("RevokedByIp")
                .HasColumnType("varchar(100)");

            builder.Property(prop => prop.ReplacedByToken)
                .IsRequired(false)
                .HasColumnName("ReplacedByToken")
                .HasColumnType("varchar(2000)");

            builder.HasOne(d => d.User)
                       .WithMany(p => p.RefreshToken)
                       .HasForeignKey(p => p.UserId)
                       .IsRequired();
        }
    }
}
