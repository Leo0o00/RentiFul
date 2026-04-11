using Managers.Data.Configurations;
using Managers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Managers.Data;

public class ManagerDbContext : DbContext
{
    public DbSet<Manager> Managers => Set<Manager>();
    
    public ManagerDbContext(DbContextOptions<ManagerDbContext> options) : base(options){}
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.ModuleName.ToLower());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ManagerEntityConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}