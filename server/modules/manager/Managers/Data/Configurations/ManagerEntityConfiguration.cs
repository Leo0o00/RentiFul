using Managers.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Managers.Data.Configurations;

public class ManagerEntityConfiguration : IEntityTypeConfiguration<Manager>
{
    public void Configure(EntityTypeBuilder<Manager> builder)
    {
        builder.HasAlternateKey(t => t.CognitoId);
        
        builder
            .Property(t => t.Id)
            .ValueGeneratedNever();

        builder
            .Property(t => t.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder
            .Property(t => t.Email)
            .HasMaxLength(256)
            .IsRequired();

        builder
            .Property(t => t.CognitoId)
            .HasMaxLength(256)
            .IsRequired();
        
        builder
            .Property(t => t.PhoneNumber)
            .HasMaxLength(256)
            .IsRequired();

        builder
            .HasIndex(t => t.Email)
            .IsUnique();

    }
}