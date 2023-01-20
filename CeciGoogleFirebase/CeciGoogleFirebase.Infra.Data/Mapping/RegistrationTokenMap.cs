using CeciGoogleFirebase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CeciGoogleFirebase.Infra.Data.Mapping
{
    [ExcludeFromCodeCoverage]
    public class RegistrationTokenMap : IEntityTypeConfiguration<RegistrationToken>
    {
        public void Configure(EntityTypeBuilder<RegistrationToken> builder)
        {
            builder.ToTable("RegistrationToken");

            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.Token)
                .IsRequired()
                .HasColumnName("Token")
                .HasColumnType("varchar(2000)");

            builder.HasOne(d => d.User)
                       .WithMany(p => p.RegistrationToken)
                       .HasForeignKey(p => p.UserId)
                       .IsRequired();
        }
    }
}
