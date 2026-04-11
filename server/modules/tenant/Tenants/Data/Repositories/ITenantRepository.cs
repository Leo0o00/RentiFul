using Tenants.Contracts;
using Tenants.Domain;

namespace Tenants.Data.Repositories;

public interface ITenantRepository
{
    IQueryable<Tenant> GetAll(bool noTracking = true);
    Task<bool> CheckTenantEmailAvailability(string cognitoId, string email);
    Task<bool> CheckTenantEmailAvailability( string email);
    Task Create(Tenant tenant);
    
    Task<Tenant?> GetByCognitoId(string cognitoId, bool noTracking = true);

    Task<Tenant?> GetWithFavoritesByCognitoId(string cognitoId, bool noTracking = true);
    Task<Tenant?> GetWithOwnedPropertiesByCognitoId(string cognitoId, bool noTracking = true);
    Task<TenantDetailsDto?> GetByIdDetailsWithFavoriteProperties(string cognitoId, bool noTracking = true);
    Task<TenantOwnedPropertiesDto?> GetByIdWithOwnedProperties(string cognitoId);
    
    
    Task<int> SaveChangesAsync();
    Task<Tenant?> GetById(Guid tenantId);
}