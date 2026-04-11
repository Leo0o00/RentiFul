using Microsoft.EntityFrameworkCore;
using Properties.Data.Configurations;
using Properties.Domain;

namespace Properties.Data;

public class PropertyDbContext: DbContext
{
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Manager> Managers => Set<Manager>();

    public PropertyDbContext(DbContextOptions<PropertyDbContext> options) : base(options){}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.ModuleName.ToLower());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PropertyEntityConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}