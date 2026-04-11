namespace Tenants.Contracts;

public record CreateTenantResponseDto(
    string CognitoId,
    string Name,
    string Email,
    string PhoneNumber,
    DateTime CreatedAt,
    DateTime? UpdatedAt
    );