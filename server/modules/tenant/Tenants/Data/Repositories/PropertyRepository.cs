using Microsoft.EntityFrameworkCore;
using Tenants.Domain;

namespace Tenants.Data.Repositories;

public class PropertyRepository  : IPropertyRepository
{
    private readonly TenantDbContext _db;

    public PropertyRepository(TenantDbContext db)
    {
        _db = db;
    }
    
    public IQueryable<Property> GetAll(bool noTracking = true)
    {
        var entityDbSet = _db.Set<Property>();

        if (noTracking)
        {
            return entityDbSet.AsNoTracking();
        }
        
        return entityDbSet;
    }

    public async Task<Property?> GetById(string propertyId, bool noTracking = true)
    {
        return await GetAll(noTracking)
            .FirstOrDefaultAsync(p => p.Id == Guid.Parse(propertyId));
    }
    public async Task<Property?> GetById(Guid propertyId, bool noTracking = true)
    {
        return await GetAll(noTracking)
            .FirstOrDefaultAsync(p => p.Id == propertyId);
    }

    public async Task Create(Property property)
    {
        _db.Properties.Add(property);
        await _db.SaveChangesAsync();
    }
}