using Applications.Contracts;
using Applications.Domain;
using Microsoft.EntityFrameworkCore;

namespace Applications.Data.Repositories;

public class ApplicationRepository : IApplicationRepository
{
    private readonly ApplicationDbContext _db;

    public ApplicationRepository(ApplicationDbContext context)
    {
        _db = context;
    }

    public IQueryable<T> GetAll<T>(bool noTracking = true) where T : class
    {
        var entityDbSet = _db.Set<T>();

        if (noTracking)
        {
            return entityDbSet.AsNoTracking();
        }

        return entityDbSet;
    }

    public async Task<Guid> CreateApplication(Application application)
    {
        var result = await _db.Applications.AddAsync(application);
        await _db.SaveChangesAsync();
        return result.Entity.Id;
    }

    public async Task<Property?> GetProperty(Guid propertyId)
    {
        return await GetAll<Property>()
            .FirstOrDefaultAsync(p => p.Id == propertyId);
    }


    public async Task<Application?> GetApplication(Guid applicationId, bool noTracking = true)
    {
        return await GetAll<Application>(noTracking)
            .Include(a => a.Lease)
            .FirstOrDefaultAsync(p => p.Id == applicationId);
    }

    public async Task<Tenant?> GetTenant(Guid tenantCognitoId)
    {
        return await GetAll<Tenant>()
            .FirstOrDefaultAsync(t => t.Id == tenantCognitoId);
    }

    public async Task<Lease?> GetLease(Guid leaseId, bool noTracking = true)
    {
        return await GetAll<Lease>()
            .FirstOrDefaultAsync(l => l.Id == leaseId);

    }

    public async Task<int> SaveChangesAsync()
    {
        return await _db.SaveChangesAsync();
    }

    public async Task RemoveLease(Lease lease)
    {
        _db.Leases.Remove(lease);
        await SaveChangesAsync();
    }

    public async Task<ListApplicationsDto> ListTenantApplications(Guid tenantCognitoId, int? page, int? limit)
    {
        int take = limit ?? 10;
        int skip = ((page ?? 1) - 1) * take;

        var query = GetAll<Application>(noTracking: true)
            .Include(a => a.Lease)
            .Include(a => a.Property)
            .Where(a => a.TenantId == tenantCognitoId);

        var count = await query.CountAsync();

            var applications = await query.OrderByDescending(a => a.SubmittedAt)
            .Skip(skip)
            .Take(take)
            .Select(a => new ApplicationDto(
                a.Id,
                a.Status.ToString(),
                a.SubmittedAt,
                a.PropertyId,
                a.TenantId,
                a.Property.ManagerId,
                a.LeaseId
                ))
            .ToListAsync();

            return new ListApplicationsDto(Count: count, Applications: applications);
    }

    public async Task<ListApplicationsDto> ListManagerApplications(Guid managerCognitoId, int? page = null, int? limit = null)
    {
        int take = limit ?? 10;
        int skip = ((page ?? 1) - 1) * take;

        var query = GetAll<Application>(noTracking: true)
            .Include(a => a.Lease)
            .Include(a => a.Property)
            .Where(a => a.Property.ManagerId == managerCognitoId);

        var count = await query.CountAsync();

        var applications = await query
            .OrderByDescending(a => a.SubmittedAt)
            .Skip(skip)
            .Take(take)
            .Select(a => new ApplicationDto(
                a.Id,
                a.Status.ToString(),
                a.SubmittedAt,
                a.PropertyId,
                a.TenantId,
                a.Property.ManagerId,
                a.LeaseId
            ))
            .ToListAsync();

        return new ListApplicationsDto(Count: count, Applications: applications);
    }
}