using Leases.Domain;

namespace Leases.Data.Repositories;

public interface ITenantRepository
{
    Task AddTenant(Guid tenantCognitoId, string name, string email, string phoneNumber, CancellationToken cancellationToken);
    Task UpdateTenant(Tenant tenantToUpdate, CancellationToken cancellationToken);
}