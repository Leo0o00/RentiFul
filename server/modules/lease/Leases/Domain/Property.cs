namespace Leases.Domain;

// public record Property(
//     Guid Id,
//     List<Lease> Leases
// );

public class Property
{
    private Property(){}

    public Property(Guid id, decimal pricePerMonth, decimal securityDeposit)
    {
        Id = id;
        PricePerMonth = pricePerMonth;
        SecurityDeposit = securityDeposit;
    }

    public Guid Id { get; set; }
    public decimal PricePerMonth { get; set; }
    public decimal SecurityDeposit { get; set; }
    public List<Lease> Leases { get; set; } = [];
}