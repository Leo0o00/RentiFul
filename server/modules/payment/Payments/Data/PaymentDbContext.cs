using Microsoft.EntityFrameworkCore;
using Payments.Data.Configurations;
using Payments.Domain;

namespace Payments.Data;

public class PaymentDbContext : DbContext
{
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Lease> Leases => Set<Lease>();

    public PaymentDbContext(DbContextOptions<PaymentDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Constants.ModuleName.ToLower());
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PaymentEntityConfiguration).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}