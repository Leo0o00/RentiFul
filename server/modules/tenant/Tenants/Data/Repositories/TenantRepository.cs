using Microsoft.EntityFrameworkCore;
using Tenants.Contracts;
using Tenants.Domain;

namespace Tenants.Data.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly TenantDbContext _db;

    public TenantRepository(TenantDbContext db)
    {
        _db = db;
    }

    public IQueryable<Tenant> GetAll(bool noTracking = true)
    {
        var entityDbSet = _db.Set<Tenant>();

        if (noTracking)
        {
            return entityDbSet.AsNoTracking();
        }
        
        return entityDbSet;
    }

    public async Task<bool> CheckTenantEmailAvailability(string cognitoId, string email)
    {
        return await GetAll()
            .Where(t => t.Email == email || t.CognitoId == cognitoId)
            .AnyAsync();
    }

    public async Task<bool> CheckTenantEmailAvailability(string email)
    {
        return await GetAll()
            .Where(t => t.Email == email)
            .AnyAsync();
    }

    public async Task Create(Tenant tenant)
    {
        await _db.Tenants.AddAsync(tenant);
        await _db.SaveChangesAsync();
    }

    public async Task<Tenant?> GetByCognitoId(string cognitoId, bool noTracking = true)
    {
        return await GetAll(noTracking)
            .FirstOrDefaultAsync(t => t.CognitoId == cognitoId);
    }
    public async Task<Tenant?> GetWithFavoritesByCognitoId(string cognitoId, bool noTracking = true)
    {
        return await GetAll(noTracking)
            .Include(t => t.FavoriteProperties)
            .FirstOrDefaultAsync(t => t.CognitoId == cognitoId);
    }
    public async Task<Tenant?> GetWithOwnedPropertiesByCognitoId(string cognitoId, bool noTracking = true)
    {
        return await GetAll(noTracking)
            .Include(t => t.OwnedProperties)
            .FirstOrDefaultAsync(t => t.CognitoId == cognitoId);
    }

    public async Task<TenantDetailsDto?> GetByIdDetailsWithFavoriteProperties(string cognitoId, bool noTracking = true)
    {
       return await GetAll(noTracking)
            .Where(t => t.CognitoId == cognitoId)
            .Select(t => 
                new TenantDetailsDto(
                    t.CognitoId, 
                    t.Name, 
                    t.Email, 
                    t.PhoneNumber, 
                    t.CreatedAt, 
                    t.UpdatedAt,
                    t.FavoriteProperties.Select(p => p.Id).ToList()
                    )
            )
            .FirstOrDefaultAsync();
    }

    public async Task<TenantOwnedPropertiesDto?> GetByIdWithOwnedProperties(string cognitoId)
    {
        return await GetAll()
            .Where(t => t.CognitoId == cognitoId)
            .Select(t => 
                new TenantOwnedPropertiesDto(
                    t.CognitoId,
                    t.OwnedProperties.Select(p => p.Id).ToList()
                )
            )
            .FirstOrDefaultAsync();
    }


    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public async Task<Tenant?> GetById(Guid tenantId)
    {
        return await GetAll()
            .FirstOrDefaultAsync(t => t.Id == tenantId);
    }
}