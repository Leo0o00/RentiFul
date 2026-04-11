namespace Leases.Contracts;

public class PropertyLeaseDto
{
    public Guid Id {get; set;}
    public DateTime StartDate {get; set;}
    public DateTime EndDate {get; set;}
    public decimal Rent {get; set;}
    public TenantDto Tenant {get; set;}
    public DateTime CreatedAt {get; set;}
    public DateTime? UpdatedAt {get; set;}

}