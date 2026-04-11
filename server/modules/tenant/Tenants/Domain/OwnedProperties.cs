namespace Tenants.Domain;

internal record class OwnedProperties(Guid TenantId, Guid PropertyId);