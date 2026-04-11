using Tenants.Domain;

namespace Tenants.Data.Repositories;

public interface IPropertyRepository
{
    IQueryable<Property> GetAll(bool noTracking = true);
    Task<Property?> GetById(string propertyId, bool noTracking = true);
    Task<Property?> GetById(Guid propertyId, bool noTracking = true);
    Task Create(Property property);
}