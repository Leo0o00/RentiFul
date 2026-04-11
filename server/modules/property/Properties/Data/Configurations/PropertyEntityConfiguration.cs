using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Properties.Domain;

namespace Properties.Data.Configurations;

public class PropertyEntityConfiguration : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder
            .Property(p => p.Id)
            .ValueGeneratedNever();
        
        builder
            .Property(p => p.Name)
            .HasMaxLength(256)
            .IsRequired();
        
        builder
            .Property(p => p.Description)
            .HasMaxLength(1000)
            .IsRequired();
        
        builder
            .Property(p => p.PricePerMonth)
            .HasPrecision(14, 2)
            .IsRequired();
        
        builder
            .Property(p => p.SecurityDeposit)
            .HasPrecision(14, 2)
            .IsRequired();
        
        builder
            .Property(p => p.ApplicationFee)
            .HasPrecision(14, 2)
            .IsRequired();
        
        #region Enums Configuration

        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };

        #region Amenities

        var amenityConverter = new EnumListJsonValueConverter<Amenity>(jsonOptions);
        var amenityComparer = new CollectionValueComparer<Amenity>();
        
        builder
            .Property(p => p.Amenities)
            .HasConversion(amenityConverter)
            .IsRequired();

        builder
            .Property(p => p.Amenities)
            .Metadata
            .SetValueComparer(comparer: amenityComparer);
        
        #endregion Amenities

        #region Highlights

        var highlightsConverter = new EnumListJsonValueConverter<Highlight>(jsonOptions);
        var highlightsComparer = new CollectionValueComparer<Highlight>();
        
        builder
            .Property(p => p.Highlights)
            .HasConversion(highlightsConverter)
            .IsRequired();

        builder
            .Property(p => p.Highlights)
            .Metadata
            .SetValueComparer(comparer: highlightsComparer);
        
        #endregion Highlights
        
        #region Property Type
        
        builder
            .Property(p => p.Type)
            .HasConversion<string>()
            .IsRequired();
        
        #endregion Property Type
        
        #endregion Enums Configuration
        
        builder
            .Property(p => p.Baths)
            .HasPrecision(2,1)
            .IsRequired();
        
        builder
            .Property(p => p.SquareFeet)
            .HasPrecision(7,2)
            .IsRequired();
        
        builder
            .Property(p => p.AverageRating)
            .HasPrecision(1,1)
            .IsRequired();

        #region Relationships Configuration

        #region With PropertyMedia

        builder
            .HasMany(e => e.PhotoKeys)
            .WithOne()
            .HasForeignKey(e => e.PropertyId)
            .IsRequired();
        
        #endregion With PropertyMedia
        
        #endregion Relationships Configuration
        
        
    }
}

public class CollectionValueComparer<T>() : ValueComparer<List<T>>((a, b) => a.SequenceEqual(b),
    v => v.Aggregate(0, (hash, item) => HashCode.Combine(hash, item.GetHashCode())),
    v => v.ToList());

public class EnumListJsonValueConverter<T>(JsonSerializerOptions jsonOptions) : ValueConverter<List<T>, string>(
    v => JsonSerializer.Serialize(v, jsonOptions),
    v => string.IsNullOrWhiteSpace(v)
        ? new List<T>()
        : JsonSerializer.Deserialize<List<T>>(v, jsonOptions) ?? new List<T>());