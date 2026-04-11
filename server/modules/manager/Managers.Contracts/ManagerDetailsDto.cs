namespace Managers.Contracts;

public record class ManagerDetailsDto(
    string CognitoId,
    string Name,
    string Email,
    string PhoneNumber,
    DateTime CreatedAt,
    DateTime? UpdatedAt
    );