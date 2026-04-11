namespace Tenants.Contracts;

public record AddOwnedPropertyResponse(Guid TenantCognitoId, Guid PropertyId);