namespace Applications.Contracts;

public record ApplicationDto(
    Guid Id,
    string Status,
    DateTime SubmitedAt,
    Guid PropertyId,
    Guid TenantCognitoId,
    Guid? ManagerId,
    Guid? LeaseId
    );