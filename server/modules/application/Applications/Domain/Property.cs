namespace Applications.Domain;

public record Property
{
    public Guid Id { get; set; }
    public Guid? ManagerId { get; set; }
    public List<Application> Applications { get; set; } = [];
}