using Payments.Domain;

namespace Payments.Data.Repositories;

public interface ILeaseRepository
{
    Task Create(Lease lease);
    Task Remove(Lease lease);
    Task<Lease?> GetLeaseById(Guid leaseId);
}