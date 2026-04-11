namespace SharedContracts.Events;


public class TenantOwnedPropertyAdded
{
    public Guid ApplicationId { get; init; }
    public Guid PropertyId { get; init; }
    public Guid TenantCognitoId { get; init; }
}

public class AddTenantOwnedPropertyFailed
{
    public Guid ApplicationId { get; init; }
    public Guid PropertyId { get; init; }
    public Guid TenantCognitoId { get; init; }
}

public class TenantCreated
{
    public Guid TenantId { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
}
public class TenantUpdated
{
    public Guid TenantId { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string PhoneNumber { get; init; }
}