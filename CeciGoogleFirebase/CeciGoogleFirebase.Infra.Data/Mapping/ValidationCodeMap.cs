using CeciGoogleFirebase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace CeciGoogleFirebase.Infra.Data.Mapping
{
    [ExcludeFromCodeCoverage]
    public class ValidationCodeMap : IEntityTypeConfiguration<ValidationCode>
    {
        public void Configure(EntityTypeBuilder<ValidationCode> builder)
        {
            builder.ToTable("ValidationCode");

            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Code)
                .IsRequired()
                .HasColumnName("Code")
                .HasColumnType("varchar(2000)");

            builder.Property(prop => prop.Expires)
                .IsRequired()
                .HasColumnName("Expires")
                .HasColumnType("datetime");

            builder.HasOne(d => d.User)
                       .WithMany(p => p.ValidationCode)
                       .HasForeignKey(p => p.UserId)
                       .IsRequired();
        }
    }
}
