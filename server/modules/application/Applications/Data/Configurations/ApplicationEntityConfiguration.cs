using Applications.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Applications.Data.Configurations;

public class ApplicationEntityConfiguration : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.Message).HasMaxLength(1000);

        builder.HasIndex(a => a.LeaseId).IsUnique();

        #region Enum Configuration

        builder
            .Property(a => a.Status)
            .HasConversion<string>()
            .IsRequired();

        #endregion Enum Configuration
    }
}