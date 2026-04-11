namespace Leases.Contracts;

public class LeasesResponseDto
{
    public int count {get; set;}
    public IEnumerable<LeaseDto> leases {get; set;}
}