using Applications.Data.Configurations;
using Applications.Domain;
using Applications.Sagas;
using Microsoft.EntityFrameworkCore;

namespace Applications.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Lease> Leases => Set<Lease>();
    public DbSet<UpdateApplicationSagaData> UpdateApplicationSagaData => Set<UpdateApplicationSagaData>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):  base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.ModuleName.ToLower());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationEntityConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantEntityConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PropertyEntityConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(LeaseEntityConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UpdateApplicationSagaDataEntityConfiguration).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}