using CeciGoogleFirebase.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace CeciGoogleFirebase.Infra.Data.Mapping
{
    [ExcludeFromCodeCoverage]
    public class AddressMap : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Address");

            builder.HasKey(prop => prop.Id);

            builder.Property(prop => prop.ZipCode)
                .IsRequired()
                .HasColumnName("ZipCode")
                .HasColumnType("varchar(8)");

            builder.Property(prop => prop.Street)
                 .IsRequired()
                 .HasColumnName("Street")
                 .HasColumnType("varchar(100)");

            builder.Property(prop => prop.District)
                .IsRequired()
                .HasColumnName("District")
                .HasColumnType("varchar(50)");

            builder.Property(prop => prop.Locality)
                .IsRequired()
                .HasColumnName("Locality")
                .HasColumnType("varchar(50)");

            builder.Property(prop => prop.Number)
                .IsRequired()
                .HasColumnName("Number")
                .HasColumnType("int");

            builder.Property(prop => prop.Complement)
                .HasColumnName("Complement")
                .HasColumnType("varchar(50)");

            builder.Property(prop => prop.Uf)
                .IsRequired()
                .HasColumnName("Uf")
                .HasColumnType("varchar(2)");

            builder.HasOne(d => d.User)
                .WithMany(p => p.Address)
                .HasForeignKey(p => p.UserId)
                .IsRequired();
        }
    }
}
