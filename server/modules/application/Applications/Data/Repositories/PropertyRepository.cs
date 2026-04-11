using Applications.Domain;

namespace Applications.Data.Repositories;

public class PropertyRepository  : IPropertyRepository
{
    private readonly ApplicationDbContext _db;

    public PropertyRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task Create(Property property)
    {
        _db.Properties.Add(property);
        await _db.SaveChangesAsync();
    }
}