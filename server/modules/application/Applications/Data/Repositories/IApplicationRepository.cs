using Applications.Contracts;
using Applications.Domain;

namespace Applications.Data.Repositories;

public interface IApplicationRepository
{
    Task<Guid> CreateApplication(Application application);
    Task<Property?> GetProperty(Guid propertyId);
    Task<Application?> GetApplication(Guid applicationId, bool noTracking = true);
    Task<Tenant?> GetTenant(Guid tenantCognitoId);

    Task<Lease?> GetLease(Guid leaseId, bool noTracking = true);

    Task<int> SaveChangesAsync();
    Task RemoveLease(Lease lease);
    Task<ListApplicationsDto> ListTenantApplications(Guid tenantCognitoId, int? page = null, int? limit = null);
    Task<ListApplicationsDto> ListManagerApplications(Guid managerCognitoId, int? page = null, int? limit = null);
}