using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payments.Domain;

namespace Payments.Data.Configurations;

public class PaymentEntityConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.Property(p => p.Id)
            .ValueGeneratedNever();

        builder.Property(p => p.AmountDue)
            .HasPrecision(14, 2)
            .IsRequired();

        builder.Property(p => p.AmountPaid)
            .HasPrecision(14, 2)
            .IsRequired();

        #region Enum Configuration

        builder
            .Property(p => p.PaymentStatus)
            .HasConversion<string>()
            .IsRequired();

        #endregion Enum Configuration
    }
}