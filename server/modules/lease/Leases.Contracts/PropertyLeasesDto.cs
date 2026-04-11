namespace Leases.Contracts;

public class PropertyLeasesDto
{
    public int count {get; set;}
    public IEnumerable<PropertyLeaseDto> leases {get; set;}
}