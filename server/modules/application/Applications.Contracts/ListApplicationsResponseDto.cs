namespace Applications.Contracts;

public record ListApplicationsResponseDto(int Count, List<ApplicationReponseDto> Applications);

public class ApplicationReponseDto
{
    public Guid Id { get; set; }
    public DateTime SubmitedAt { get; set; }
    public string Status { get; set; }
    public PropertyInfoDto? Property { get; set; }
    public TenantInfoDto? Tenant { get; set; }
    public ManagerInfoDto? Manager { get; set; }
    public LeaseInfoDto? Lease { get; set; }

}