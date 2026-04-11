namespace Leases.Domain;

public class Lease
{
    private Lease(){}

    public Lease(DateTime startDate, DateTime endDate, decimal rent, decimal deposit, Guid propertyId, Guid tenantId)
    {
        StartDate = startDate;
        EndDate = endDate;
        Rent = rent;
        Deposit = deposit;
        PropertyId = propertyId;
        TenantId = tenantId;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public decimal Rent { get; private set; }
    public decimal Deposit { get; private set; }
    public Guid PropertyId { get; private set; }
    public Guid TenantId { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public Property Property { get; private set; }
    public Tenant Tenant { get; private set; }

}