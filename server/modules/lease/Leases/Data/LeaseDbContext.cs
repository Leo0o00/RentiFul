using Leases.Data.Configurations;
using Leases.Domain;
using Microsoft.EntityFrameworkCore;

namespace Leases.Data;

public class LeaseDbContext : DbContext
{
    public DbSet<Lease> Leases => Set<Lease>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Tenant> Tenants => Set<Tenant>();

    public LeaseDbContext(DbContextOptions<LeaseDbContext> options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.ModuleName.ToLower());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LeaseEntityConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PropertyEntityConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantEntityConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}