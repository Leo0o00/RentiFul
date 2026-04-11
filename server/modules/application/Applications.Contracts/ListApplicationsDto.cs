namespace Applications.Contracts;

public record ListApplicationsDto(
    int Count,
    List<ApplicationDto> Applications
    );