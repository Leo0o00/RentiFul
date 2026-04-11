using Applications.Domain;
using Microsoft.EntityFrameworkCore;

namespace Applications.Data.Repositories;

internal class TenantRepository: ITenantRepository
{
    private readonly ApplicationDbContext _db;

    public TenantRepository(ApplicationDbContext context)
    {
        _db = context;
    }

    public async Task AddTenant(Guid tenantCognitoId, CancellationToken cancellationToken)
    {
        var tenantToCreate = new Tenant
        {
            Id = tenantCognitoId
        };

        _db.Tenants.Add(tenantToCreate);
        await _db.SaveChangesAsync(cancellationToken);
    }
}