using Ardalis.Result;
using Leases.Contracts;
using Leases.Domain;
using Microsoft.EntityFrameworkCore;

namespace Leases.Data.Repositories;

public class LeaseRepository : ILeaseRepository
{
    private readonly LeaseDbContext _context;

    public LeaseRepository(LeaseDbContext context)
    {
        _context = context;
    }

    public IQueryable<Tenant> GetAllTenants(bool noTracking = true)
    {
        var entityDbSet = _context.Set<Tenant>();

        if (noTracking)
        {
            return entityDbSet.AsNoTracking();
        }

        return entityDbSet;
    }

    public IQueryable<Lease> GetAllLeases(bool noTracking = true)
    {
        var entityDbSet = _context.Set<Lease>();

        if (noTracking)
        {
            return entityDbSet.AsNoTracking();
        }

        return entityDbSet;
    }
    public IQueryable<Property> GetAllProperties(bool noTracking = true)
    {
        var entityDbSet = _context.Set<Property>();

        if (noTracking)
        {
            return entityDbSet.AsNoTracking();
        }

        return entityDbSet;
    }


    public async Task<bool> CheckTenantExistence(Guid tenantCognitoId)
    {
        return await GetAllTenants()
            .Where(t => t.Id == tenantCognitoId)
            .AnyAsync();
    }

    public async Task<bool> CheckPropertyExistence(Guid propertyId)
    {
        return await GetAllProperties()
            .Where(p => p.Id == propertyId)
            .AnyAsync();
    }

    public async Task<Lease?> GetLeaseById(Guid leaseId, bool noTracking = true)
    {
        return await GetAllLeases(noTracking)
            .FirstOrDefaultAsync(l => l.Id == leaseId);
    }

    public async Task<LeasesResponseDto> GetLeasesByTenantIdAndPropertyId(Guid requestCognitoId, Guid propertyId, int pageSize)
    {
        var query = GetAllLeases()
            .Where(l => l.TenantId == requestCognitoId && l.PropertyId == propertyId);

        var total = await query.CountAsync();

        var rows = await query
            .OrderByDescending(t => t.CreatedAt)
            .Take(pageSize)
            .Select(t => new LeaseDto
            {
                Id = t.Id,
                StartDate = t.StartDate,
                EndDate = t.EndDate,
                Rent = t.Rent,
                Deposit = t.Deposit,
                PropertyId = t.PropertyId,
                TenantId = t.TenantId,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt

            })
            .ToListAsync();

        return new LeasesResponseDto
        {
            count = total,
            leases = rows
        };
    }

    public async Task<PropertyLeasesDto> GetLeasesByPropertyId(Guid propertyId, int pageSize)
    {
        var query = GetAllLeases()
            .Where(l => l.PropertyId == propertyId);

        var total = await query.CountAsync();

        var rows = await query
            .OrderByDescending(t => t.CreatedAt)
                .Take(pageSize)
                .Include(l => l.Tenant)
                .Select(l => new PropertyLeaseDto
                {
                    Id = l.Id,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Rent = l.Rent,
                    Tenant = new TenantDto
                    {
                        Id = l.Tenant.Id,
                        Name = l.Tenant.Name,
                        Email = l.Tenant.Email,
                        PhoneNumber = l.Tenant.PhoneNumber,
                    },
                    CreatedAt = l.CreatedAt,
                    UpdatedAt = l.UpdatedAt
                })
                .ToListAsync();

        return new PropertyLeasesDto
        {
            count = total,
            leases = rows
        };
    }

    public async Task<Guid> CreateLease(Lease lease)
    {
        var result = await _context.Leases.AddAsync(lease);
        await _context.SaveChangesAsync();
        return result.Entity.Id;
    }

    public async Task<Property?> GetPropertyById(Guid propertyId, bool noTracking = true)
    {
        return await GetAllProperties(noTracking)
            .FirstOrDefaultAsync(p => p.Id == propertyId);
    }

    public async Task<Tenant?> GetTenantByCognitoId(Guid tenantCognitoId, bool noTracking = true)
    {
        return await GetAllTenants(noTracking)
            .FirstOrDefaultAsync(t => t.Id == tenantCognitoId);
    }

    public async Task RemoveLease(Lease lease)
    {
        _context.Leases.Remove(lease);
        await _context.SaveChangesAsync();
    }
}