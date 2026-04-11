namespace SharedContracts.Commands;

public record AddTenantOwnedProperty(Guid ApplicationId, Guid PropertyId, Guid TenantCognitoId);