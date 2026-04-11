using Leases.Domain;

namespace Leases.Data.Repositories;

public class PropertyRepository  : IPropertyRepository
{
    private readonly LeaseDbContext _db;

    public PropertyRepository(LeaseDbContext db)
    {
        _db = db;
    }

    public async Task Create(Property property)
    {
        _db.Properties.Add(property);
        await _db.SaveChangesAsync();
    }
}