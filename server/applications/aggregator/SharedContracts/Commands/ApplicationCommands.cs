namespace SharedContracts.Commands;

public record LinkLeaseToApplication(Guid ApplicationId, Guid LeaseId);
public record UnlinkLeaseToApplication(Guid ApplicationId, Guid LeaseId);


public record UpdateApplicationStatusRollback(Guid ApplicationId);