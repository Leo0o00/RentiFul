using Leases.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Leases.Data.Configurations;

public class PropertyEntityConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.Property(p => p.Id)
            .ValueGeneratedNever();
        builder
            .Property(p => p.PricePerMonth)
            .HasPrecision(14, 2)
            .IsRequired();

        builder
            .Property(p => p.SecurityDeposit)
            .HasPrecision(14, 2)
            .IsRequired();
    }
}