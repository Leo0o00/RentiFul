namespace Applications.Domain;

public record Tenant
{
    public Guid Id { get; set; }

    public List<Application> Applications { get; set; } = [];
}