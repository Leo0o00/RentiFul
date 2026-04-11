namespace Applications.Domain;

public class Application
{
    private Application(){}
    public Application(DateTime submittedAt, ApplicationStatus status, Property property, Tenant tenant, string? message)
    {
        SubmittedAt = submittedAt;
        Status = status;
        PropertyId = property.Id;
        TenantId = tenant.Id;
        Message = message;
    }



    public Guid Id { get; private set; } = Guid.NewGuid();
    public DateTime SubmittedAt { get; private set; }
    public ApplicationStatus Status { get; private set; }

    public Guid PropertyId { get; private set; }
    public Property Property { get; private set; }

    public Guid TenantId { get; private set; }
    public Tenant Tenant { get; private set; }

    public string? Message { get; private set; }

    public Guid? LeaseId { get; private set; }
    public Lease? Lease { get; private set; }

    public DateTime? StatusChangedAt { get; private set; }

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public void UpdateApplicationStatus(ApplicationStatus applicationStatus)
    {
        Status = applicationStatus;
        StatusChangedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddLease(Lease lease)
    {
        Lease = lease;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UnlinkLease()
    {
        Lease = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RollbackApplicationStatus()
    {
        StatusChangedAt = null;
        Status = ApplicationStatus.Pending;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum ApplicationStatus
{
    Denied,
    Pending,
    Approved
}