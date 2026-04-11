using Microsoft.EntityFrameworkCore;
using Payments.Domain;

namespace Payments.Data.Repositories;

public class LeaseRepository: ILeaseRepository
{
    private readonly PaymentDbContext _context;

    public LeaseRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public async Task Create(Lease lease)
    {
        _context.Leases.Add(lease);
        await _context.SaveChangesAsync();
    }

    public async Task Remove(Lease lease)
    {
        _context.Leases.Remove(lease);
        await _context.SaveChangesAsync();
    }

    public async Task<Lease?> GetLeaseById(Guid leaseId)
    {
        return await _context.Leases
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Id == leaseId);
    }
}