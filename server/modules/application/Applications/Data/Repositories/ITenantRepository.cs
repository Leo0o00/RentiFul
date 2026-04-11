using Applications.Features.Tenant.CreateTenant;

namespace Applications.Data.Repositories;

public interface ITenantRepository
{
    Task AddTenant(Guid tenantCognitoId, CancellationToken cancellationToken);
}