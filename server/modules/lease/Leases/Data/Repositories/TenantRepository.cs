using Leases.Domain;
using Microsoft.EntityFrameworkCore;

namespace Leases.Data.Repositories;

internal class TenantRepository: ITenantRepository
{
    private readonly LeaseDbContext _db;

    public TenantRepository(LeaseDbContext context)
    {
        _db = context;
    }

    public async Task AddTenant(Guid tenantCognitoId, string name, string email, string phoneNumber,
        CancellationToken cancellationToken)
    {
        var tenantToCreate = new Tenant(tenantCognitoId, name, email, phoneNumber);

        _db.Tenants.Add(tenantToCreate);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateTenant(Tenant tenantToUpdate, CancellationToken cancellationToken)
    {
        _db.Tenants.Update(tenantToUpdate);
        await _db.SaveChangesAsync(cancellationToken);
    }
}