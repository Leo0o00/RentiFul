using Properties.Contracts;
using Properties.Data.Helpers;
using Properties.Domain;

namespace Properties.Data.Repositories;

public interface IPropertyRepository
{
    Task<PropertiesQueryResultDto> GetPropertiesByFilters(PropertiesQueryFilters propertiesQueryFilters);
    Task<PropertiesQueryResultDto> GetAllPropertiesInAnIdList(List<Guid> propertiesIdList);
    Task<Guid> CreateProperty(Property property);
    Task<Property?> GetPropertyById(Guid propertyId, bool isTracking = false);
    Task<bool> CheckManagerExistence(string managerCognitoId);
    Task<Manager?> GetManager(string managerCognitoId);
    Task<PropertiesQueryResultDto> GetPropertiesByManagerId(string cognitoId);

    Task<int> SaveChangesAsync();
    Task<PropertyMinInfoDto?> GetPropertyMinimalInfoById(Guid propertyId);
    Task<PropertyMinInfoDto?> GetPropertyInfoById(Guid propertyId);

}