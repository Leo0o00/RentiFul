using Microsoft.EntityFrameworkCore;
using Payments.Contracts;
using Payments.Domain;

namespace Payments.Data.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _context;

    public PaymentRepository(PaymentDbContext context)
    {
        _context = context;
    }

    public IQueryable<Payment> GetAllPayments(bool noTracking = true)
    {
        var entityDbSet = _context.Set<Payment>();

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

    public async Task<bool> CheckLeaseExistence(Guid leaseId)
    {
        return await GetAllLeases()
            .Where(l => l.Id == leaseId)
            .AnyAsync();
    }

    public async Task<PaymentStatusDto> GetLeasePaymentStatusByDate(Guid leaseId, DateOnly dueDate)
    {
        var result = await GetAllPayments()
            .Where(p => p.LeaseId == leaseId)
            .FirstOrDefaultAsync(p => p.DueDate.Month == dueDate.Month && p.DueDate.Year == dueDate.Year);

        var status = result?.PaymentStatus.ToString();

        return new PaymentStatusDto
        {
            PaymentStatus = status
        };

    }
}