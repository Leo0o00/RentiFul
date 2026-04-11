using Managers.Domain;

namespace Managers.Data.Repositories;

public interface IManagerRepository
{
    IQueryable<Manager> GetAll(bool noTracking = true);
    Task<bool> CheckManagerAvailability(string cognitoId, string email);
    Task<bool> CheckManagerAvailability(string email);
    Task Create(Manager manager);
    Task<Manager?> GetDetailsByCognitoId(string cognitoId, bool noTracking = true);
    Task<Manager?> GetDetailsById(Guid managerId);
    // Task<Tenant?> GetWithFavoritesByCognitoId(string cognitoId, bool noTracking = true);
    // Task<TenantDetailsDto?> GetByIdDetailsWithFavoriteProperties(string cognitoId, bool noTracking = true);
    // Task<TenantOwnedPropertiesDto?> GetByIdWithOwnedProperties(string cognitoId, bool noTracking = true);


    Task<int> SaveChangesAsync();
}