using Microsoft.EntityFrameworkCore;
using Tenants.Data.Configurations;
using Tenants.Domain;

namespace Tenants.Data;

public class TenantDbContext : DbContext
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Property> Properties => Set<Property>();
    
    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options){}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.ModuleName.ToLower());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantEntityConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}