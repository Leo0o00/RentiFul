using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Properties.Domain;

namespace Properties.Data.Configurations;

public class LocationEntityConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.Property(p => p.Id)
            .ValueGeneratedNever();
        
        builder.Property(p => p.Address)
            .HasMaxLength(256)
            .IsRequired();
        
        builder.Property(p => p.City)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(p => p.State)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(p => p.Country)
            .HasMaxLength(50)
            .IsRequired();
        
        builder.Property(p => p.PostalCode)
            .HasMaxLength(5)
            .IsRequired();
        
        builder.Property(p => p.Coordinates)
            .HasColumnType("geography (Point, 4326)");
        
        
    }
}