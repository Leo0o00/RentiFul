using Leases.Domain;

namespace Leases.Data.Repositories;

public interface IPropertyRepository
{

    Task Create(Property property);
}