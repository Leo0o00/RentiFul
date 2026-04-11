namespace Tenants.Contracts;

public record TenantOwnedPropertiesDto(
    string CognitoId,
    IEnumerable<Guid> OwnedProperties
    );