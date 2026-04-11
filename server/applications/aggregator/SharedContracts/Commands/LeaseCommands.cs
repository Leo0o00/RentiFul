namespace SharedContracts.Commands;

public record CreateLease(Guid ApplicationId, Guid PropertyId, Guid TenantCognitoId, DateTime StartDate, DateTime EndDate);

public record CreateLeaseRollback(Guid ApplicationId, Guid LeaseId);