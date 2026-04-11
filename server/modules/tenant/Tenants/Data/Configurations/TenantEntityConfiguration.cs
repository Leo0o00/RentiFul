using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tenants.Domain;

namespace Tenants.Data.Configurations;

internal class TenantEntityConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
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
            .HasMany(typeof(Property), nameof(Tenant.FavoriteProperties))
            .WithMany()
            .UsingEntity(
                "FavoriteProperties",
                r => r.HasOne(typeof(Property)).WithMany().HasForeignKey("PropertyId").OnDelete(DeleteBehavior.Cascade),
                l => l.HasOne(typeof(Tenant)).WithMany().HasForeignKey("TenantId").OnDelete(DeleteBehavior.Cascade),
                je => 
                {
                    je.HasKey("TenantId", "PropertyId");
                    je.HasIndex("TenantId", "PropertyId").IsUnique();
                });
        
        builder.Navigation(nameof(Tenant.FavoriteProperties))
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        
        // builder
        //     .HasMany(t => t.OwnedProperties)
        //     .WithMany()
        //     .UsingEntity<OwnedProperties>();
        
        builder
            .HasMany(typeof(Property), nameof(Tenant.OwnedProperties))
            .WithMany()
            .UsingEntity(
                "OwnedProperties",
                r => r.HasOne(typeof(Property)).WithMany().HasForeignKey("PropertyId").OnDelete(DeleteBehavior.Cascade),
                l => l.HasOne(typeof(Tenant)).WithMany().HasForeignKey("TenantId").OnDelete(DeleteBehavior.Cascade),
                je => 
                {
                    je.HasKey("TenantId", "PropertyId");
                    je.HasIndex("TenantId", "PropertyId").IsUnique();
                });
        
        builder.Navigation(nameof(Tenant.OwnedProperties))
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder
            .HasIndex(t => t.Email)
            .IsUnique();

    }
}
