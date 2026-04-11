namespace SharedContracts.Events;

#region Success Events

public class ApplicationApproved
{
    public Guid ApplicationId { get; init; }
    public Guid PropertyId { get; init; }
    public Guid TenantCognitoId { get; init; }
    public DateTime ApprovedAt { get; init; }
}


public class LeaseLinkedToApplication
{
    public Guid ApplicationId { get; init; }
}

public class ApproveApplicationProcessCompleted
{
    public Guid ApplicationId { get; set; }

    public Guid TenantCognitoId { get; set; }

    public Guid PropertyId { get; set; }
}

#endregion Success Events

#region Failure Events

public class LinkLeaseToApplicationFailed
{
    public Guid ApplicationId { get; init; }
    public Guid LeaseId { get; init; }
}

public class UnlinkedLeaseToApplication
{
    public Guid ApplicationId { get; init; }
    public Guid LeaseId { get; init; }

}

public class UpdateApplicationStatusRolledBack
{
    public Guid ApplicationId { get; init; }

}

#endregion Failure Events