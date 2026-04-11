using Managers.Domain;
using Microsoft.EntityFrameworkCore;

namespace Managers.Data.Repositories;

public class ManagerRepository : IManagerRepository
{
    private readonly ManagerDbContext _db;

    public ManagerRepository(ManagerDbContext db)
    {
        _db = db;
    }

    public IQueryable<Manager> GetAll(bool noTracking = true)
    {
        var entityDbSet = _db.Set<Manager>();

        if (noTracking)
        {
            return entityDbSet.AsNoTracking();
        }
        
        return entityDbSet;
    }

    public async Task<bool> CheckManagerAvailability(string cognitoId, string email)
    {
        return await GetAll()
            .Where(t => t.Email == email || t.CognitoId == cognitoId)
            .AnyAsync();
    }

    public async Task<bool> CheckManagerAvailability(string email)
    {
        return await GetAll()
            .Where(t => t.Email == email)
            .AnyAsync();
    }

    public async Task Create(Manager manager)
    {
        await _db.Managers.AddAsync(manager);
        await _db.SaveChangesAsync();
    }

    public async Task<Manager?> GetDetailsByCognitoId(string cognitoId, bool noTracking = true)
    {
        return await GetAll(noTracking)
            .FirstOrDefaultAsync(t => t.CognitoId == cognitoId);
    }

    public async Task<Manager?> GetDetailsById(Guid managerId)
    {
        return await GetAll()
            .FirstOrDefaultAsync(t => t.Id == managerId);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }
}