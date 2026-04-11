namespace SharedContracts.Events;


public class LeaseCreated
{
    public Guid ApplicationId { get; init; }
    public Guid LeaseId { get; init; }
}

public class CreateLeaseFailed
{
    public Guid ApplicationId { get; init; }

}

public class LeaseRemoved
{
    public Guid ApplicationId { get; init; }
    public Guid LeaseId { get; init; }

};