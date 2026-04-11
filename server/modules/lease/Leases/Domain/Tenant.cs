namespace Leases.Domain;

// public record Tenant(
//     Guid Id,
//     string Name,
//     string Email,
//     string PhoneNumber,
//     List<Lease> Leases
// );

public class Tenant
{
    private Tenant() { }

    public Tenant(Guid tenantId, string name, string email, string phoneNumber)
    {
        Id = tenantId;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
    }

    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<Lease> Leases { get; set; } = [];
}