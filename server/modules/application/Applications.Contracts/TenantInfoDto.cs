namespace Applications.Contracts;

public class TenantInfoDto
{
    public Guid CongitoId { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
}