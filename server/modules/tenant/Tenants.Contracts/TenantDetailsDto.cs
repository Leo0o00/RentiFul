

namespace Tenants.Contracts;

public record TenantDetailsDto(
    string CognitoId,
    string Name,
    string Email,
    string PhoneNumber,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IEnumerable<Guid> FavoriteProperties
    );