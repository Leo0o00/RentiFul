using Properties.Domain;

namespace Properties.Data.Repositories;

public interface IManagerRepository
{
    Task Create(Manager manager, CancellationToken cancellationToken);
}