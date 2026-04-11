using Properties.Domain;

namespace Properties.Data.Repositories;

public class ManagerRepository: IManagerRepository
{
    private readonly PropertyDbContext _context;

    public ManagerRepository(PropertyDbContext context)
    {
        _context = context;
    }

    public async Task Create(Manager manager, CancellationToken cancellationToken)
    {
        _context.Managers.Add(manager);
        await _context.SaveChangesAsync(cancellationToken);
    }
}