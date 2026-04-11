namespace Applications.Domain;

public class Lease
{
    private Lease() { }

    public Lease(Guid leaseId)
    {
        Id = leaseId;
    }

    public Guid Id { get; init; }
}