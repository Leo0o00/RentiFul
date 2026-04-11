using Ardalis.Result;
using Leases.Contracts;
using Leases.Domain;
using Leases.Features.Lease.CreateLease.Handler;

namespace Leases.Data.Repositories;

public interface ILeaseRepository
{
    Task<bool> CheckTenantExistence(Guid tenantCognitoId);
    Task<bool> CheckPropertyExistence(Guid propertyId);
    Task<Lease?> GetLeaseById(Guid leaseId, bool noTracking = true);
    Task<LeasesResponseDto> GetLeasesByTenantIdAndPropertyId(Guid tenantCognitoId, Guid propertyId, int pageSize);
    Task<PropertyLeasesDto> GetLeasesByPropertyId(Guid propertyId, int pageSize);

    Task<Guid> CreateLease(Lease lease);
    Task<Property?> GetPropertyById(Guid propertyId, bool noTracking = true);
    Task<Tenant?> GetTenantByCognitoId(Guid tenantCognitoId, bool noTracking = true);

    Task RemoveLease(Lease lease);
}