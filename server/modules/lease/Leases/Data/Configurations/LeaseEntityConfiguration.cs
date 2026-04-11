using Leases.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Leases.Data.Configurations;

public class LeaseEntityConfiguration : IEntityTypeConfiguration<Lease>
{
    public void Configure(EntityTypeBuilder<Lease> builder)
    {
        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.Rent)
            .HasPrecision(14, 2)
            .IsRequired();

        builder.Property(p => p.Deposit)
            .HasPrecision(14, 2)
            .IsRequired();

    }
}